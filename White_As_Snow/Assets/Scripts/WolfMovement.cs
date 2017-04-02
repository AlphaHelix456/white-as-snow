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
    private InventoryMenuController inventory;
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
		footsteps = gameObject.GetComponent<AudioSource> ();
        inventory = GameObject.Find("InventoryController").GetComponent<InventoryMenuController>();
	}
	
	void FixedUpdate () {
        //Movement
        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !inventory.isOpen()) {
            isMoving = true;
            rb.velocity = new Vector2(rb.velocity.x, SPEED);
            facingup = true;
        }
        else if((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !inventory.isOpen()) {
            isMoving = true;
            rb.velocity = new Vector2(rb.velocity.x, -SPEED);
            facingup = false;
        }
        else {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !inventory.isOpen()) {
            isMoving = true;
            rb.velocity = new Vector2(SPEED, rb.velocity.y);
            facingright = true;
            facingup = false;
        }
        else if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !inventory.isOpen()) {
            isMoving = true;
            rb.velocity = new Vector2(-SPEED, rb.velocity.y);
            facingright = false;
            facingup = false;
        }
        else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
		if (rb.velocity.x == 0 && rb.velocity.y == 0)
			isMoving = false;
        /*
        if(isMoving) {
            footsteps.enabled = true;
            footsteps.loop = true;
        }
        else {
            footsteps.enabled = false;
            footsteps.loop = false;
        }
        */
    }
}
   