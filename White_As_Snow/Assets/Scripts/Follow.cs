using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    private readonly float START_D = 7.1f;
    private readonly float MIN_D = 5.5f;
    private readonly float SPEED = 4.5f;
    private GameObject player;
    private GameObject otherFollower;
    private Rigidbody2D rb;
    private bool moving;
    private float angle;

    void Start()
    {
        player = GameObject.Find("wolf");
        moving = false;
        rb = GetComponent<Rigidbody2D>();
        if (gameObject.name == "follower1")
        {
            otherFollower = GameObject.Find("follower2");
        }
        else
        {
            otherFollower = GameObject.Find("follower1");
        }
    }

    void FixedUpdate()
    {
        if (!moving && toMove())
        {
            moving = true;
        }

        if (moving)
        {
            angle = getAngle();
            if (angle < 67.5f)
            {
                if (getXDiff() > 0) //If the follower is to the right of the player
                {
                    //Move left
                    rb.velocity = new Vector2(-SPEED, rb.velocity.y);
                }
                else
                {
                    //Move right
                    rb.velocity = new Vector2(SPEED, rb.velocity.y);
                }
            }
            else if (angle > 22.5f)
            {
                if (getYDiff() > 0) //If the follower is above the player
                {
                    //Move down
                    rb.velocity = new Vector2(rb.velocity.x, -SPEED);
                }
                else
                {
                    //Move up
                    rb.velocity = new Vector2(rb.velocity.x, SPEED);
                }
            }
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
        return Mathf.Acos(Mathf.Abs(getXDiff()) / getDist()) * 180 / Mathf.PI;
    }

    private float getXDiff()
    {
        return this.transform.position.x - player.transform.position.x;
    }

    private float getYDiff()
    {
        return this.transform.position.y - player.transform.position.y;
    }
}