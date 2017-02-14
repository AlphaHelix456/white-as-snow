using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public WolfCombat wolf;

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

    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    private float max_size = 3f; //largest size the ATB gague should grow to

    private float animSpeed = 10f;

    public GameObject waitBar;
    public GameObject selector;

    //IEnumerator for Combat Movement
    public GameObject EnemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;

    void Start () {
        startPosition = transform.position;

        //cur_cooldown = Random.Range(0, 2.5f); If one class (Hyper) should use his crit/hunger to start attacking first
        waitBar = this.transform.FindChild("wait_fill").gameObject;
        selector = this.transform.FindChild("selector").gameObject;
        selector.SetActive(false);

        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.PROCESSING;
	}
	
	void Update () {
		switch(currentState)
        {
            case (TurnState.PROCESSING):
                UpdateProgressBar();
                break;
            case (TurnState.ADDTOLIST):
                BSM.WolvesToManage.Add(this.gameObject);
                currentState = TurnState.WAITING;
                break;
            case (TurnState.WAITING):

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
        cur_cooldown += Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        waitBar.transform.localScale = new Vector2(Mathf.Clamp(calc_cooldown, 0, 1)*max_size, waitBar.transform.localScale.y);
        if (cur_cooldown >= max_cooldown)
            currentState = TurnState.ADDTOLIST;

    }
    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        //animate the wolf to move to the wolf to attack
        Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x + 3.7f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
        while (MoveTowardsTarget(enemyPosition))
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

        //reset this wolf's state and ATB gague 
        cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;
    }
    private bool MoveTowardsTarget(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
}
