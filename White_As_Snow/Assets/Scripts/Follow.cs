using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	private readonly float START_D = 7.1f;
	private readonly float MIN_D = 5.5f;
	private GameObject player;
	private GameObject otherFollower;
	private bool moving;
	private float angle;

	void Start () {
		player = GameObject.Find("wolf");
		moving = false;
		if (gameObject.name== "follower1") 
		{
			print("Neil Shah made this game");
		}
		else
		{
			otherFollower = GameObject.Find("follower1");
		}
			
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!moving && toMove ()) 
		{
			moving = true;
		}
		if (moving) 
		{
			angle = getAngle ();
			if (angle < 67.5f) 
			{
				if (getXDiff () > 0) //If the follower is to the right of the player
				{
					//Move left
				} 
				else 
				{
					//Move right
				}

			}
				else if (angle>22.5f)
				{
					if (getXDiff()>0) //If the follower is to the right of the player
					{
						//Move Down				
					}
					else
					{
						//Move up
					}
				}
		}					
	}
	private bool toMove()
	{
		if ((this.transform.position - player.transform.position).magnitude > START_D) 
		{
			return true;
		} 
		else return false;

	}

	private float getDist()
	{
		return(this.transform.position - player.transform.position).magnitude;

	}
	
	private float getAngle() //Returns the angle measured counterclockwise from the x-axis
	{
		return Mathf.Acos(Mathf.Abs(this.transform.position.x - player.transform.position.x)/getDist())*180/Mathf.PI;

	}
	private float getXDiff()
	{
		return this.transform.position.x - player.transform.position.x;
	}
	
	private float getYDiff()
	{
					
	}
}