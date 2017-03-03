using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour {

    // Use this for initialization
    public Transform objectToMove;
    public Transform destination;
    public float duration;
    public bool blocking = true;
    private float speed;
    private Vector3 directionVector;
    private bool isRunning;
	void Start () {
        if(duration == 0)
        {
            speed = 0;
        } else
        {
            speed = Vector3.Distance(objectToMove.position, destination.position) / duration;
        }
        directionVector = (destination.position - objectToMove.position).normalized;

    }

    // Update is called once per frame
    void Update () {
        if (isRunning)
        {
            move();
        }
	}
    private void move()
    {
        if(duration != 0) //if duration is zero, just teleport
        {
            objectToMove.position += directionVector * speed * Time.deltaTime;
            if (Vector3.Distance(objectToMove.position, destination.position) < speed*Time.deltaTime)
            {
                objectToMove.position = destination.position;
                deleteSelf();
            }
        } else
        {
            objectToMove.position = destination.position;
            deleteSelf();
        }
        
    }
    private void deleteSelf()
    {
        DestroyObject(gameObject);
    }
    public bool isBlocking()
    {
        //used in cutscene controller
        return blocking;
    }
    public void activate()
    {
        if (!isRunning)
        {
            isRunning = true;
            directionVector = (destination.position - objectToMove.position).normalized;
            if (duration == 0)
            {
                speed = 0;
            }
            else
            {
                speed = Vector3.Distance(objectToMove.position, destination.position) / duration;
            }
        }
        
    }
}
