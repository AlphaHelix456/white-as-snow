using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimate : MonoBehaviour {

    public Animator wolfAnimator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool moving = false;
    private bool changeAnim = false;
    private string idleAnim;
    private string runAnim;
    private string name;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        startRunning();
        name = gameObject.name;
        if (name == "wolf" || name == "Alpha")
        {
            idleAnim = "Wolf1Idle";
            runAnim = "WolfRunLeft";
        }
        else if (name == "follower1" || name == "Hyper")
        {
            idleAnim = "Wolf2Idle";
            runAnim = "Wolf2RunLeft";
        }
        else if (name == "follower2" || name == "Caution")
        {
            idleAnim = "Wolf3Idle";
            runAnim = "Wolf3RunLeft";
        }
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
        if (moving) wolfAnimator.Play(runAnim);
        else wolfAnimator.Play(idleAnim);
        changeAnim = false;
    }
    void startRunning()
    {
        rb.velocity = new Vector2(.05f, 0);
        StartCoroutine(stopRunning());
    }
    IEnumerator stopRunning()
    {
        yield return new WaitForSeconds(0.02f);
        rb.velocity = new Vector2(0, 0);

    }

}
