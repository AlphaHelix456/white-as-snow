using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRail : MonoBehaviour {
    public bool revealed;
    public Transform origin;
    public Transform dest;
    public float speedFactor;
    private bool isMoving;
    public bool debug = true;
	// Use this for initialization
	void Start () {
        isMoving = false;
        if(!revealed){
            transform.position = origin.position;
        } else
        {
            transform.position = dest.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (isMoving)
        {
            if (revealed)
            {
                moveToLocation(dest.position);
            } else
            {
                moveToLocation(origin.position);
            }
        } else
        {

        }
        if(Input.GetKeyDown("space") && debug)
        {
            if (revealed)
            {
                hideUI();
            }
            else
            {
                revealUI();
            }
        }
	}
    public void moveToLocation(Vector3 target)
    {
        //moves towards location, snaps to location once close enough
        transform.position += (target - transform.position) * (speedFactor / 100f) * (1 - Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 1)
        {
            isMoving = false;
            transform.position = target;
        }
    }
    public void revealUI()
    {
        isMoving = true;
        revealed = true;
    }
    public void hideUI()
    {
        isMoving = true;
        revealed = false;
    }
}
