using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimate : MonoBehaviour {

    public Animator wolfAnimator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool moving = false;
    private bool changeAnim = false;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (rb.velocity.x > 0) sr.flipX = true;
        else if(rb.velocity.x < 0) sr.flipX = false;
        if (rb.velocity.magnitude > 0.01f && !moving)
        {
            moving = true;
            changeAnim = true;
        }
        if(moving && rb.velocity.magnitude == 0)
        {
            moving = false;
            changeAnim = true;
        }
        if (changeAnim) changeAnims();
	}

    private void changeAnims()
    {
        if (moving) wolfAnimator.Play("WolfRunLeft");
        else wolfAnimator.Play("Wolf1Idle");
        changeAnim = false;
    }
}
