using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


	// Use this for initialization
	void Start () {
        battleStates = PerformAction.WAIT;
        WolvesInBattle.AddRange(GameObject.FindGameObjectsWithTag("wolf"));
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("enemy"));
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

                }
                battleStates = PerformAction.PERFORMACTION;
                break;
            case (PerformAction.PERFORMACTION):

                break;
        }
	}
    public void CollectActions(HandleTurns input)
    {
        PerformList.Add(input);
    }
}
