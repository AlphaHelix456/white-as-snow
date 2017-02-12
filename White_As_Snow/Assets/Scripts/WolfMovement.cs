using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMovement : MonoBehaviour {

    Rigidbody2D rb;
    private readonly float SPEED = 4.5f;
    protected bool facingright = true;
    protected bool facingup = false;

	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        //Movement
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            rb.velocity = new Vector2(rb.velocity.x, SPEED);
            facingup = true;
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            rb.velocity = new Vector2(rb.velocity.x, -SPEED);
            facingup = false;
        }
        else {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            rb.velocity = new Vector2(SPEED, rb.velocity.y);
            facingright = true;
            facingup = false;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            rb.velocity = new Vector2(-SPEED, rb.velocity.y);
            facingright = false;
            facingup = false;
        }
        else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
   