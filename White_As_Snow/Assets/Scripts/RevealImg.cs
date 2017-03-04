using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevealImg : MonoBehaviour {

    // Use this for initialization
    public SpriteRenderer spriteRenderer;
    public float duration;
    public bool blocking = true;
    private bool isRunning;
	void Start () {
        isRunning = false;
        spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            spriteRenderer.enabled = true;
            duration -= Time.deltaTime;
            if(duration < 0)
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
