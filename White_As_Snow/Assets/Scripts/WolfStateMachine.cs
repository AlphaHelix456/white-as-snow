using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfStateMachine : MonoBehaviour {

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
    private float max_size = 3f;

    public GameObject waitBar;
	void Start () {
        waitBar = this.transform.GetChild(0).gameObject;
        currentState = TurnState.PROCESSING;
	}
	
	void Update () {
		switch(currentState)
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
        float calc_cooldown = cur_cooldown / max_cooldown;
        waitBar.transform.localScale = new Vector2(Mathf.Clamp(calc_cooldown, 0, 1)*max_size, waitBar.transform.localScale.y);
        if (cur_cooldown >= max_cooldown)
            currentState = TurnState.ADDTOLIST;

    }
}
