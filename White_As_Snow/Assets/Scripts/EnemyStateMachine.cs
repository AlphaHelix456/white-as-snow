using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float max_size = 3f;

    private Vector2 startPosition;
    //Timeforaction
    private bool actionStarted = false;
    public GameObject WolfToAttack;
    //Speed to walk to the target
    private float animSpeed = 10f;
    

    void Start () {
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

                break;
        }
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
        //CHANGE THIS FOR DYANMIC AI (BASED ON LOWEST HP, HEALER, THREAT, ETC.)
        myAttack.AttackersTarget = BSM.WolvesInBattle[Random.Range(0, BSM.WolvesInBattle.Count)];
        

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
    /*
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    */
}
