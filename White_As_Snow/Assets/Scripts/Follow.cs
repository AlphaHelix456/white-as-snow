using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    private readonly float START_D = 6.5f;
    private readonly float STOP_D = 5f; //When the wolves will stop walking
    private readonly float FOLLOW_D = 4.2f; //Followers must stay at least this distance apart
    private readonly float SPEED = 4.5f;
    private GameObject player;
    private GameObject otherFollower;
    private Rigidbody2D rb;
    private bool moving;
    private float angle;
    private bool[][] dirs = new bool[2][];

    void Start()
    {
        for (int i = 0; i < 2; i++)
            dirs[i] = new bool[4];
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
            if (getDist(player) <= STOP_D)
            {
                moving = false;
                rb.velocity = new Vector2(0, 0);
            }
            else
            {
                dirs[0] = walkDir(player);
                dirs[1] = walkDir(otherFollower);

                //Left=0, right=1, up=2, down=3
                //Movement in direction of player unless there is another wolf in the way
                if (getDist(otherFollower) > FOLLOW_D || true)
                {
                    if (dirs[0][0])
                        rb.velocity = new Vector2(-SPEED, rb.velocity.y);
                    else if (dirs[0][1])
                        rb.velocity = new Vector2(SPEED, rb.velocity.y);
                    else
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    if (dirs[0][2])
                        rb.velocity = new Vector2(rb.velocity.x, SPEED);
                    else if (dirs[0][3])
                        rb.velocity = new Vector2(rb.velocity.x, -SPEED);
                    else
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                }
                else
                {
                    
                }
            }
        }
    }


    private bool toMove()
    {
        if (getDist(player) > START_D)
        {
            return true;
        }
        else return false;
    }

    private float getDist(GameObject target)
    {
        return (this.transform.position - player.transform.position).magnitude;
    }

    private float getDist(GameObject start, GameObject target)
    {
        return (start.transform.position - player.transform.position).magnitude;
    }

    private float getAngle(GameObject target) //Returns the angle measured counterclockwise from the x-axis.
    {
        return Mathf.Acos(Mathf.Abs(getXDiff(player)) / getDist(player)) * 180 / Mathf.PI;
    }

    private float getXDiff(GameObject target)
    {
        return this.transform.position.x - target.transform.position.x;
    }

    private float getYDiff(GameObject target)
    {
        return this.transform.position.y - target.transform.position.y;
    }

    private bool[] walkDir(GameObject target)
    {
        bool[] dirs = new bool[4]; //Left=0, right=1, up=2, down=3
        angle = getAngle(target);
        if (angle < 67.5f)
        {
            if (getXDiff(target) > 0) //If the follower is to the right of the player
            {
                //Move left
                dirs[0] = true;
                dirs[1] = false;
            }
            else
            {
                //Move right
                dirs[0] = false;
                dirs[1] = true;
            }
        }
        else
        {
            dirs[0] = false;
            dirs[1] = false;
        }
        if (angle > 22.5f)
        {
            if (getYDiff(target) > 0) //If the follower is above the player
            {
                //Move down
                dirs[2] = false;
                dirs[3] = true;
            }
            else
            {
                //Move up
                dirs[2] = true;
                dirs[3] = false;
            }
        }
        else
        {
            dirs[2] = false;
            dirs[3] = false;
        }
        return dirs;
    }
}
