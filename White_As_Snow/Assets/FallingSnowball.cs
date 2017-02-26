using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingSnowballs : MonoBehaviour {

	Rigidbody2D rb;
	private float speed = 2.0f;
	private float deltaX = 0.5f; // Amount to move left and right from the start point
	private bool curve = false;
	private float startPos;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
		snowFallStraight();
		Wait2Seconds();
		curve = true;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void snowFallStraight(){
		rb.velocity = new Vector2 (rb.velocity.x, -speed);
	}

	IEnumerator Wait2Seconds(){
		yield return new WaitForSeconds(2);
	}
}