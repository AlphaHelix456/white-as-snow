using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMovement : MonoBehaviour {

    Rigidbody2D rb;
    private readonly float SPEED = 5.3f;
    protected bool facingright = true;
    protected bool facingup = false;
    public AudioSource footsteps;
    private bool isMoving;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        //Movement
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            isMoving = true;
            rb.velocity = new Vector2(rb.velocity.x, SPEED);
            facingup = true;
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            isMoving = true;
            rb.velocity = new Vector2(rb.velocity.x, -SPEED);
            facingup = false;
        }
        else {
            isMoving = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            isMoving = true;
            rb.velocity = new Vector2(SPEED, rb.velocity.y);
            facingright = true;
            facingup = false;
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            isMoving = true;
            rb.velocity = new Vector2(-SPEED, rb.velocity.y);
            facingright = false;
            facingup = false;
        }
        else {
            isMoving = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if(isMoving == true) {
            footsteps.enabled = true;
            footsteps.loop = true;
        }
        else {
            footsteps.enabled = false;
            footsteps.loop = false;
        }
    }
}
   