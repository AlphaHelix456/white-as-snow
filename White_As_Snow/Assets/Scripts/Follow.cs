using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    private readonly float START_D = 6.5f;
    private readonly float STOP_D = 5f; //When the wolves will stop walking
    private readonly float MIN_D = 2.5f; //Wolves must stay at least this distance apart
    private readonly float MIN_D_SOFT = 3.3f; //Followers will try to stay at least this far apart when walking
    private float SPEED = 5.3f;

    private GameObject player;
    private GameObject otherFollower;
    private Rigidbody2D rb;

    private bool needsToMove;
    private bool adjusting;
    private float angle;
    private bool[][] dirs = new bool[2][]; //Directions, where left=0, right=1, up=2, down=3

    void Start()
    {
        for (int i = 0; i < 2; i++)
            dirs[i] = new bool[4];
        player = GameObject.Find("wolf");
        needsToMove = false;
        adjusting = false;
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
        if (adjusting)
        {
            if (getDist(otherFollower) >= MIN_D_SOFT)
            {
                adjusting = false;
                rb.velocity = new Vector2(0, 0);
            }
            else
            {
                angleSeparate();
            }
        }
        if (!needsToMove && toMove())
        {
            needsToMove = true;
        }

        if (needsToMove)
        {
            if (getDist(player) <= STOP_D)
            {
                needsToMove = false;
                rb.velocity = new Vector2(0, 0);
            }
            else
            {
                dirs[0] = walkDir(player);
                dirs[1] = walkDir(otherFollower);

                //Left=0, right=1, up=2, down=3
                //Movement in direction of player
                if (isClosest() || getDist(otherFollower) > MIN_D)
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
                    rb.velocity = new Vector2(0, 0);
                    otherFollower.SendMessage("adjust");
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
        return (this.transform.position - target.transform.position).magnitude;
    }

    private float getDist(GameObject start, GameObject target)
    {
        return (start.transform.position - target.transform.position).magnitude;
    }

    private float getAngle(GameObject target) //Returns the angle measured counterclockwise from the x-axis, although
                                              //it reflects across x- and y- axes to find the angle in the first quadrant.
    {
        return Mathf.Acos(Mathf.Abs(getXDiff(target)) / getDist(target)) * 180 / Mathf.PI;
    }

    private float getAngle(GameObject start, GameObject target) //Returns the angle measured counterclockwise from the x-axis, although
                                              //it reflects across x- and y- axes to find the angle in the first quadrant.
    {
        return Mathf.Acos(Mathf.Abs(getXDiff(start,target)) / getDist(start,target)) * 180 / Mathf.PI;
    }

    private float getXDiff(GameObject target)
    {
        return this.transform.position.x - target.transform.position.x;
    }

    private float getXDiff(GameObject start, GameObject target)
    {
        return start.transform.position.x - target.transform.position.x;
    }

    private float getYDiff(GameObject target)
    {
        return this.transform.position.y - target.transform.position.y;
    }

    private float getYDiff(GameObject start, GameObject target)
    {
        return start.transform.position.y - target.transform.position.y;
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

    private bool isClosest()
    {
        if (getDist(player) < getDist(otherFollower, player)) return true;
        else return false;
    }

    private void adjust()
    {
        adjusting = true;
    }

    private void angleSeparate()
    {
        adjusting = true;
        float a1 = getAngle(player), a2 = getAngle(otherFollower, player);
        bool greaterA = a1 - a2 >= 0 ? true : false;
        bool above = getYDiff(player) >= 0;
        bool right = getXDiff(player) >= 0;
        bool above2 = getYDiff(otherFollower, player) >= 0;
        bool right2 = getXDiff(otherFollower, player) >= 0;
        int quad = above ? (right ? 1 : 2) : (right ? 4 : 3);
        int quad2 = above2 ? (right2 ? 1 : 2) : (right2 ? 4 : 3);
        if(quad==quad2)
        {
            switch(quad)
            {
                case 1:
                    rb.velocity = greaterA ? new Vector2(-SPEED, 0) : new Vector2(0, -SPEED);
                    break;
                case 2:
                    rb.velocity = greaterA ? new Vector2(SPEED, 0) : new Vector2(0, -SPEED);
                    break;
                case 3:
                    rb.velocity = greaterA ? new Vector2(SPEED, 0) : new Vector2(0, SPEED);
                    break;
                case 4:
                    rb.velocity = greaterA ? new Vector2(-SPEED, 0) : new Vector2(0, SPEED);
                    break;
            }
        }
    }
}
