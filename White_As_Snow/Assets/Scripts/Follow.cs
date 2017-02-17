using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	private readonly float START_D = 7.1f;
	private readonly float MIN_D = 5.5f;
	private GameObject player;
	private bool moving;
	private float angle;

	void Start () {
		player = GameObject.Find("wolf");
		moving = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
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
		return((this.transform.position - player.transform.position).magnitude;

	}
	
			private float.getAngle() //Returns the angle measured counterclockwise from the x-axis
			{
				return Mathf.Acos(Mathf.Abs(this.transform.position.x - player.transform.position.x)/getDist())*180/Mathf.PI;

			}
}