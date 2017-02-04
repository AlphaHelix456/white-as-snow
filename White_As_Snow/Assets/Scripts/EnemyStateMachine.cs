using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

    public EnemyCombat enemy;

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
    private float max_size = 3f;

    // Use this for initialization
    void Start () {
        currentState = TurnState.PROCESSING;

    }

    // Update is called once per frame
    void Update () {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpdateProgressBar();
                break;
            case (TurnState.ADDTOLIST):

                break;
            case (TurnState.WAITING):

                break;
            case (TurnState.SELECTING):

                break;
            case (TurnState.ACTION):

                break;
            case (TurnState.DEAD):

                break;
        }
    }
    void UpdateProgressBar()
    {
        cur_cooldown += Time.deltaTime;
        
        if (cur_cooldown >= max_cooldown)
            currentState = TurnState.ADDTOLIST;

    }
}
