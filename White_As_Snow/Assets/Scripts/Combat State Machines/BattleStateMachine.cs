using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour {

    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }

    public PerformAction battleStates;

    public List<HandleTurns> PerformList = new List<HandleTurns>();
    public List<GameObject> WolvesInBattle = new List<GameObject>(); // to be updated on death
    public List<GameObject> EnemiesInBattle = new List<GameObject>();

    public enum WolfGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1, //Ability name
        INPUT2, //Enemy target
        DONE
    }

    public WolfGUI WolfInput;
    public List<GameObject> WolvesToManage = new List<GameObject>();
    private HandleTurns WolfChoice;

    public GameObject enemyButton;

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;
	// Use this for initialization
	void Start () {
        battleStates = PerformAction.WAIT;
        WolvesInBattle.AddRange(GameObject.FindGameObjectsWithTag("wolf"));
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("enemy"));
        WolfInput = WolfGUI.ACTIVATE;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);

        EnemyButtons();
	}
	
	// Update is called once per frame
	void Update () {
		switch(battleStates)
        {
            case (PerformAction.WAIT):
                if (PerformList.Count > 0)
                {
                    battleStates = PerformAction.TAKEACTION;
                }
                break;
            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if(PerformList[0].Type == "enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.WolfToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }
                if (PerformList[0].Type == "wolf")
                {
                    WolfStateMachine HSM = performer.GetComponent<WolfStateMachine>();
                    HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = WolfStateMachine.TurnState.ACTION;
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
                    if(WolvesToManage.Count > 0)
                    {
                        WolvesToManage[0].transform.FindChild("selector").gameObject.SetActive(true); //Indicator appears in-game
                        WolfChoice = new HandleTurns();

                        AttackPanel.SetActive(true); //Right grey attack panel appears
                        WolfInput = WolfGUI.WAITING; //Idle state for Wolf Input
                    }
                    break;
                }
                
            case (WolfGUI.WAITING):
                {
                    //idling
                    break;
                }
                
            case (WolfGUI.DONE):
                {
                    WolfInputDone();
                    break;
                }
        }
	}
    public void CollectActions(HandleTurns input) //When a unit's StateMachine issues an input, add it to global list of queued actions
    {
        PerformList.Add(input);
    }

    void EnemyButtons()
    {
        foreach (GameObject enemy in EnemiesInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject; //Find enemy buttons as prefab
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();
            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.transform.FindChild("Text").gameObject.GetComponent<Text>();
            buttonText.text = cur_enemy.enemy.theName;

            button.EnemyPrefab = enemy;
            newButton.transform.SetParent(EnemySelectPanel.transform,false);
        }
    }

    public void Input1() //attack choice in the UI
    {
        WolfChoice.Attacker = WolvesToManage[0].name;
        WolfChoice.AttackerGameObject = WolvesToManage[0];
        WolfChoice.Type = "wolf";

        AttackPanel.SetActive(false); //after choosing, disable attack panel on the right
        EnemySelectPanel.SetActive(true); //and enable the enemy's panel on the left

    }

    public void Input2(GameObject chosenEnemy) //Enemy selection
    {
        WolfChoice.AttackersTarget = chosenEnemy;
        WolfInput = WolfGUI.DONE;
    }

    void WolfInputDone()
    {
        PerformList.Add(WolfChoice); //Command sent to BSM list of commands
        EnemySelectPanel.SetActive(false);
        WolvesToManage[0].transform.FindChild("selector").gameObject.SetActive(false); //Indicator disappears in-game
        WolvesToManage.RemoveAt(0); //Cycle into the next wolf's input handling
        WolfInput = WolfGUI.ACTIVATE;
    }
}
