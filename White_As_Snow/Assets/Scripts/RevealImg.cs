using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevealImg : MonoBehaviour {

    // Use this for initialization
    public SpriteRenderer spriteRenderer;
    public float duration;
    public bool blocking = true;
    private float timer;
    private bool isRunning;
	void Start () {
        timer = 0;
        isRunning = false;
        spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            spriteRenderer.enabled = true;
            timer += Time.deltaTime;
            if(timer > duration)
            {
                spriteRenderer.enabled = false;
                deleteSelf();
            }
        }
	}
    public void deleteSelf()
    {
        DestroyObject(gameObject);
    }
    public void activate()
    {
        isRunning = true;
    }
    public bool isBlocking()
    {
        return blocking;
    }
}
