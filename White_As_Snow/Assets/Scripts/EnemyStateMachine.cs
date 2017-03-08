using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public EnemyCombat enemy;

    public enum TurnState
    {
        PROCESSING, //Wait time bar is filling
        CHOOSEACTION, //CHOOSE action/target
        WAITING, //IDLING
        ACTION, //
        DEAD
    }
    public TurnState currentState;

    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    private float max_size = 1.27f;

    public GameObject healthBar;

    private Vector2 startPosition;
    //Timeforaction
    private bool actionStarted = false;
    public GameObject WolfToAttack;
    private bool alive = true;

    //Speed to walk to the target
    private float animSpeed = 10f;
    

    void Start () {
        healthBar = this.transform.FindChild("health_bar").gameObject;
        UpdateHealthBar();
        max_cooldown = enemy.currentSPD;

        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update () {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpdateProgressBar();
                break;
            case (TurnState.CHOOSEACTION):
                ChooseAction();
                currentState = TurnState.WAITING;
                break;
            case (TurnState.WAITING): //idle state

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
                    BSM.EnemiesInBattle.Remove(this.gameObject);

                    //remove from PerformList
                    for (int i = 0; i < BSM.PerformList.Count; i++)
                    {
                        if (BSM.PerformList[i].AttackerGameObject == this.gameObject)
                        {
                            BSM.PerformList.Remove(BSM.PerformList[i]);
                        }
                    }
                    //change color / [later] play death animation to signify dead enemy
                    this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(105, 105, 105, 255);

                    alive = false;

                }
                break;
        }
    }

    private IEnumerator HitFrame()
    {
        this.GetComponent<SpriteRenderer>().color = new Color32(200, 0, 0, 255);
        yield return new WaitForSeconds(.2f);
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    private IEnumerator HealFrame()
    {
        this.GetComponent<SpriteRenderer>().color = new Color32(0, 180, 0, 255);
        yield return new WaitForSeconds(.2f);
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    void UpdateProgressBar()
    {
        //Updates an inisible ATB progress bar for enemies
        cur_cooldown += Time.deltaTime;

        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.CHOOSEACTION;

        }
    }
    void ChooseAction()
    {
        HandleTurns myAttack = new HandleTurns();
        myAttack.Attacker = enemy.name;
        myAttack.Type = "enemy";
        myAttack.AttackerGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.WolvesInBattle[Random.Range(0, BSM.WolvesInBattle.Count)]; //CHANGE THIS FOR DYANMIC AI (BASED ON LOWEST HP, HEALER, THREAT, ETC.)

        int num = Random.Range(0, enemy.availableAttacks.Count);
        myAttack.chosenMove = enemy.availableAttacks[num];

        BSM.CollectActions(myAttack);
    }

    private IEnumerator TimeForAction()
    {
        if(actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        //animate the enemy to move to the player to attack
        Vector3 targetPosition = new Vector3(WolfToAttack.transform.position.x-3.7f, WolfToAttack.transform.position.y, WolfToAttack.transform.position.z);
        while (MoveTowardsTarget(targetPosition))
        {
            //waits until MoveTowardsEnemy returns
            yield return null;
        }

        //wait a bit
        yield return new WaitForSeconds(0.5f);

        //deal damage
        doDamage();

        //animate back to start position
        Vector3 firstPosition = startPosition;
        while (MoveTowardsTarget(firstPosition))
        {
            //waits until MoveTowardsEnemy returns
            yield return null;
        }

        //remove this attacker from the BSM list
        BSM.PerformList.RemoveAt(0);

        //reset BSM -> Wait
        BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
        actionStarted = false;

        //reset this enemy state 
        cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;
    }
    private bool MoveTowardsTarget(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    void UpdateHealthBar()
    {
        float calc_health = enemy.currentHP / enemy.baseHP;
        healthBar.transform.localScale = new Vector2(Mathf.Clamp(calc_health, 0, 1) * max_size, healthBar.transform.localScale.y);

    }
    public void takeDamage(float incomingDamage)
    {
        enemy.currentHP -= Mathf.Floor(incomingDamage * (10 / enemy.currentDEF));
        if (enemy.currentHP <= 0)
        {
            currentState = TurnState.DEAD;
        }
        else
        {
            StartCoroutine(HitFrame());
        }
        UpdateHealthBar();
    }
    void doDamage()
    {
        float calc_damage = enemy.currentATK + BSM.PerformList[0].chosenMove.moveValue;
        WolfToAttack.GetComponent<WolfStateMachine>().takeDamage(calc_damage);
    }
}
