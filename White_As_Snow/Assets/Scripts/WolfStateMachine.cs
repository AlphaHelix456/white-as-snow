using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public WolfCombat wolf;
    private GameData gameData;


    public enum TurnState
    {
        PROCESSING, //Wait time bar is filling
        ADDTOLIST, //Add wolf to a "readied" list
        WAITING, //IDLING
        SELECTING, //NAVIGATING through menus
        ACTION, //
        DEAD
    }
    public TurnState currentState;

    public float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    private float max_size = 1f; //largest size the ATB gague should grow to

    public GameObject waitBar;
    public GameObject healthBar;
    public GameObject selector;

    //IEnumerator for Combat Movement
    public GameObject EnemyToAttack;
    public bool isHostile;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animSpeed = 10f;

    //Sound effects
    private AudioSource audioWolfReady;
    private AudioSource audioWolfDeath;

    //Wolf death
    private bool alive = true;

    void Start() {
        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();

        startPosition = transform.position;

        healthBar = this.transform.FindChild("health_bar").gameObject;
        waitBar = this.transform.FindChild("wait_fill").gameObject;
        selector = this.transform.FindChild("selector").gameObject;
        max_cooldown = wolf.currentSPD;

        selector.SetActive(false);

        UpdateHealthBar();

        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();

        audioWolfReady = BSM.AddAudio((AudioClip)Resources.Load("Audio/wolf_ready"), false, false, .9f);
        audioWolfDeath = BSM.AddAudio((AudioClip)Resources.Load("Audio/wolf_death"), false, false, .9f);

        currentState = TurnState.PROCESSING;
    }

    void Update() {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                if (BSM.currentGame == BattleStateMachine.WinLose.COMPLETE)
                {
                    currentState = TurnState.WAITING;
                }
                else
                {
                    UpdateProgressBar();
                }
                break;
            case (TurnState.ADDTOLIST):
                BSM.WolvesToManage.Add(this.gameObject);
                audioWolfReady.Play();
                currentState = TurnState.WAITING;
                break;
            case (TurnState.WAITING):

                break;
            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;
            case (TurnState.DEAD):

                if (!alive)
                {
                    return;
                }
                else
                {
                    //change tag
                    this.gameObject.tag = "deadwolf";
                    //not attackable by enemy
                    BSM.WolvesInBattle.Remove(this.gameObject);
                    //not managable by player
                    BSM.WolvesToManage.Remove(this.gameObject);
                    //deactivate selector
                    selector.SetActive(false);

                    //remove from PerformList
                    for (int i = 0; i < BSM.PerformList.Count; i++)
                    {
                        if (BSM.PerformList[i].AttackerGameObject == this.gameObject)
                        {
                            BSM.PerformList.Remove(BSM.PerformList[i]);
                        }
                    }
                    //change color / [later] play death animation to signify dead wolf
                    StartCoroutine(fadeToNothing());
                    audioWolfDeath.Play();
                    //reset wolfinput
                    BSM.WolfInput = BattleStateMachine.WolfGUI.ACTIVATE;
                    alive = false;
                }
                break;
        }
    }

    void UpdateProgressBar()
    {
        cur_cooldown += Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        waitBar.transform.localScale = new Vector2(Mathf.Clamp(calc_cooldown, 0, 1) * max_size, waitBar.transform.localScale.y);
        if (cur_cooldown >= max_cooldown)
            currentState = TurnState.ADDTOLIST;
    }

    void UpdateHealthBar()
    {
        float calc_health = wolf.currentHP / wolf.baseHP;
        healthBar.transform.localScale = new Vector2(Mathf.Clamp(calc_health, 0, 1) * max_size, healthBar.transform.localScale.y);

    }
    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        //animate the wolf to move to the wolf to attack
        Vector3 enemyPosition;
        if (isHostile) enemyPosition = new Vector3(EnemyToAttack.transform.position.x + 3.7f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
        else enemyPosition = new Vector3(EnemyToAttack.transform.position.x - 3.7f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);

        while (MoveTowardsTarget(enemyPosition))
        {
            //waits until MoveTowardsEnemy returns
            yield return null;
        }

        //wait a bit
        yield return new WaitForSeconds(0.5f);

        //deal damage
        if (isHostile) doDamage();
        else handleFriendly();

        //animate back to start position
        Vector3 firstPosition = startPosition;
        while (MoveTowardsTarget(firstPosition))
        {
            //waits until MoveTowardsEnemy returns
            yield return null;
        }

        endMove();
    }
    private IEnumerator ResetDiveDef()
    {
        yield return new WaitForSeconds(5.5f);
        EnemyToAttack.GetComponent<WolfStateMachine>().wolf.currentDEF = EnemyToAttack.GetComponent<WolfStateMachine>().wolf.baseDEF;
        EnemyToAttack.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
    private IEnumerator HitFrame()
    {
        this.GetComponent<SpriteRenderer>().color = new Color32(200, 0, 0, 255);
        yield return new WaitForSeconds(.2f);
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
    private IEnumerator HealFrame()
    {
        this.GetComponent<SpriteRenderer>().color = new Color32(153, 255, 153, 255);
        yield return new WaitForSeconds(.2f);
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
    private bool MoveTowardsTarget(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    public void takeDamage(float incomingDamage)
    {
        wolf.currentHP -= Mathf.Floor(incomingDamage * (10 / wolf.currentDEF));
        if (wolf.currentHP <= 0)
        {
            currentState = TurnState.DEAD;
        }
        else
        {
            if (wolf.currentDEF == 9000) //If Fen is currently blocking 
            {
                BSM.WolvesOrderedByDataIndex[0].GetComponent<WolfStateMachine>().takeDamage(incomingDamage);
            }
            else
            {
                StartCoroutine(HitFrame());
            }
        }
        UpdateHealthBar();
    }

    void doDamage()
    {
        float calc_damage = wolf.currentATK + BSM.PerformList[0].chosenMove.moveValue;
        EnemyToAttack.GetComponent<EnemyStateMachine>().takeDamage(calc_damage);
    }

    public void endMove()
    {
        //remove this attacker from the BSM list
        if (BSM.PerformList.Count > 0)
        {
            BSM.PerformList.RemoveAt(0);
        }

        //reset BSM -> Wait
        BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
        actionStarted = false;

        //reset this wolf's state and ATB gauge 
        cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;
    }

    public void receiveItemHealing(float healValue, float hungerValue)
    {
        wolf.currentHunger = Mathf.Min(wolf.currentHunger + hungerValue, wolf.baseHunger);
        receiveHealing(healValue);
    }

    public void receiveHealing(float healValue)
    {
        wolf.currentHP = Mathf.Min(wolf.currentHP + healValue, wolf.baseHP);
        StartCoroutine(HealFrame());
        UpdateHealthBar();
    }

    void handleFriendly()
    //The method assumes that each wolf only has one friendly-targted ability.
    {
        if (wolf.name == "Fen")
        {
            EnemyToAttack.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 102, 255);
            EnemyToAttack.GetComponent<WolfStateMachine>().wolf.currentDEF = 9000;
            StartCoroutine(ResetDiveDef());
        }
        if (wolf.name == "Lycia")
        {
            EnemyToAttack.GetComponent<WolfStateMachine>().receiveHealing(BSM.PerformList[0].chosenMove.moveValue);
        }
    }

    private IEnumerator fadeToNothing()
    {
        byte currentAlpha = 255;
        while (currentAlpha > 0)
        {
            yield return new WaitForSeconds(.006f);
            currentAlpha--;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, currentAlpha);
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    child.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, currentAlpha);
                }

            }
        }
    }

    public void playWolfReady()
    {
        audioWolfReady.Play();
    }
}



