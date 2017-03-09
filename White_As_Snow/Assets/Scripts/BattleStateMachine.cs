using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BattleStateMachine : MonoBehaviour
{
    private GameData gameData;

    public enum PerformAction
    {
        STARTING,
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }


    public PerformAction battleStates;

    public List<HandleTurns> PerformList = new List<HandleTurns>();
    public List<GameObject> WolvesInBattle = new List<GameObject>();
    public List<GameObject> WolvesOrderedByDataIndex;
    public List<GameObject> EnemiesInBattle = new List<GameObject>();

    public enum WolfGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

    public WolfGUI WolfInput;
    public List<GameObject> WolvesToManage = new List<GameObject>(); //Wolves that are ready to attack
    private HandleTurns WolfChoice;

    public GameObject enemyButton;
    public GameObject AttackPanel;

    private CombatUIController CombatUI;
    public GameObject initSelector;

    public enum WinLose
    {
        INPROGRESS,
        FINISHING,
        COMPLETE
    }

    public WinLose currentGame;

    private AudioSource audioVictory;
    private AudioSource audioDefeat;

    // Use this for initialization
    void Start()
    {
        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        if(gameData == null)
        {
            print("No gameData found");
        }
        CombatUI = GameObject.Find("CombatUIController").GetComponent<CombatUIController>();

        battleStates = PerformAction.WAIT;
        WolvesInBattle.AddRange(GameObject.FindGameObjectsWithTag("wolf"));
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("enemy"));

        WolvesOrderedByDataIndex.Add(GameObject.Find("Fen"));
        WolvesOrderedByDataIndex.Add(GameObject.Find("Lycia"));
        WolvesOrderedByDataIndex.Add(GameObject.Find("Eyr"));

        audioVictory = AddAudio((AudioClip)Resources.Load("Audio/WANO-FULL-LEADS-[AudioTrimmer.com]"), false, false, 1f);
        audioDefeat = AddAudio((AudioClip)Resources.Load("Defeat"), false, false, 1f);

        loadStats();
        WolfInput = WolfGUI.ACTIVATE;
        currentGame = WinLose.INPROGRESS;
    }

    // Update is called once per frame
    void Update()
    {
        checkDeaths();
        switch (battleStates)
        {
            case (PerformAction.WAIT):
                if (PerformList.Count > 0)
                {
                    battleStates = PerformAction.TAKEACTION;
                }
                else if (WolvesToManage.Count == 0)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
                break;
            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if (PerformList[0].Type == "enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    for (int i = 0; i < WolvesInBattle.Count; i++) //allows support for group battles (multi vs multi)
                    {
                        if (PerformList[0].AttackersTarget == WolvesInBattle[i])
                        {
                            ESM.WolfToAttack = PerformList[0].AttackersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                            break;
                        }
                        else
                        {
                            PerformList[0].AttackersTarget = WolvesInBattle[Random.Range(0, WolvesInBattle.Count)]; //or whatever selection AI used in the future
                            ESM.WolfToAttack = PerformList[0].AttackersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                        }
                    }
                }
                if (PerformList[0].Type == "wolf")
                {
                    WolfStateMachine WSM = performer.GetComponent<WolfStateMachine>();
                    WSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    WSM.isHostile = PerformList[0].hostileAttack;
                    WSM.currentState = WolfStateMachine.TurnState.ACTION;
                }
                battleStates = PerformAction.PERFORMACTION;
                break;
            case (PerformAction.PERFORMACTION):
                //idle
                break;
        }
        switch (WolfInput)
        {
            case (WolfGUI.ACTIVATE):
                {
                    if (WolvesToManage.Count > 0)
                    {
                        WolvesToManage[0].transform.FindChild("selector").gameObject.SetActive(true); //Indicator appears in-game
                        WolfChoice = new HandleTurns();

                        AttackPanel.SetActive(true); //attack panel appears
                        CombatUI.startTurn(WolvesToManage[0].name);

                        WolfInput = WolfGUI.WAITING; //Idle state for Wolf Input
                    }
                    break;
                }

            case (WolfGUI.WAITING):
                {
                    //idling
                    break;
                }
            case (WolfGUI.INPUT1):
                {
                    break;
                }
            case (WolfGUI.INPUT2):
                {
                    break;
                }

            case (WolfGUI.DONE):
                {
                    WolfInputDone();
                    break;
                }
        }
        switch (currentGame)
        {
            case (WinLose.INPROGRESS):
                {
                    if (EnemiesInBattle.Count == 0)
                    {
                        audioVictory.Play();
                        StartCoroutine(victorySequence());
                        currentGame = WinLose.COMPLETE;
                    }
                    else if (WolvesOrderedByDataIndex[0].GetComponent<WolfStateMachine>().currentState == WolfStateMachine.TurnState.DEAD) 
                    {
                        audioDefeat.Play();
                        StartCoroutine(defeatSequence());
                        currentGame = WinLose.COMPLETE;
                    }
                    break;
                }
            case (WinLose.COMPLETE):
                {
                    break;
                }
        }

    }
    public void CollectActions(HandleTurns input) //When a unit's StateMachine issues an input, add it to global list of queued actions
    {
        PerformList.Add(input);
    }

    public void Input1(int attackIndex) //attack choice in the UI
    {
        WolfChoice.Attacker = WolvesToManage[0].name;
        WolfChoice.AttackerGameObject = WolvesToManage[0];
        WolfChoice.Type = "wolf";
        WolfChoice.chosenMove = WolvesToManage[0].GetComponent<WolfStateMachine>().wolf.availableAttacks[attackIndex];
    }

    public void Input2(GameObject chosenEnemy, bool isHostile) //Enemy selection
    {
        WolfChoice.AttackersTarget = chosenEnemy;
        WolfChoice.hostileAttack = isHostile;
    }

    void WolfInputDone()
    {
        PerformList.Add(WolfChoice); //Command sent to BSM list of commands
        WolvesToManage[0].transform.FindChild("selector").gameObject.SetActive(false); //Indicator disappears in-game
        WolvesToManage.RemoveAt(0); //Cycle into the next wolf's input handling
        WolfInput = WolfGUI.ACTIVATE;
    }
    void loadStats()
    {
        WolfCombat currentData;
        for (int i = 0; i < WolvesOrderedByDataIndex.Count; i++)
        {
            currentData = gameData.getWolfStats(i);
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.name = currentData.name;
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.baseHP = currentData.baseHP;
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.currentHP = currentData.currentHP;
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.baseATK = currentData.baseATK;
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.currentATK = currentData.currentATK;
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.baseDEF = currentData.baseDEF;
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.currentDEF = currentData.currentDEF;
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.baseSPD = currentData.baseSPD;
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.currentSPD = currentData.currentSPD;
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.baseCRIT = currentData.baseCRIT;
            WolvesOrderedByDataIndex[i].GetComponent<WolfStateMachine>().wolf.currentCRIT = currentData.currentCRIT;
        }
    }
    void checkDeaths()
    {
        if (EnemiesInBattle.Count == 0)
        {
            StartCoroutine(victorySequence());
        }
        else if (WolvesInBattle.Count == 0)
        {
            StartCoroutine(defeatSequence());
        }
    }
    IEnumerator victorySequence()
    {
        yield return new WaitForSeconds(6.5f);
        gameData.setGameProgress(gameData.getGameProgress() + 1);
        SceneManager.LoadScene("Cutscenes");
    }
    IEnumerator defeatSequence()
    {
        //Will eventually fade into a "Retry Battle?" canvas. 
        yield return new WaitForSeconds(6.5f);
        SceneManager.LoadScene("World");
    }
    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();

        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;

        return newAudio;
    }
}
