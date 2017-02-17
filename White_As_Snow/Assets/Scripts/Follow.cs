using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    private readonly float START_D = 7.1f;
    private readonly float MIN_D = 5.5f;
    private GameObject player;
    private bool moving;
    private float angle;

    void Start()
    {
        player = GameObject.Find("wolf");
        moving = false;
    }

    void FixedUpdate()
    {
        if (!moving && toMove())
        {
            angle = getAngle();
        }
    }

    private bool toMove()
    {
        if (getDist() > START_D)
        {
            return true;
        }
        else return false;
    }

    private float getDist()
    {
        return (this.transform.position - player.transform.position).magnitude;
    }

    private float getAngle() //Returns the angle measured counterclockwise from the x-axis.
    {
        return Mathf.Acos(Mathf.Abs(this.transform.position.x - player.transform.position.x) / getDist()) * 180 / Mathf.PI;
    }
}

   