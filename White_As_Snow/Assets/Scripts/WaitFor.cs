using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitFor : MonoBehaviour {

    // Use this for initialization
    public float duration;
    public bool blocking = true;
    private float timer;
    private bool isRunning;
    
	void Start () {
        isRunning = false;
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            timer += Time.deltaTime;
            if(timer > duration)
            {
                deleteSelf();
            }
        }
	}
    public void activate()
    {
        isRunning = true;
    }
    public bool isBlocking()
    {
        return blocking;
    }
    private void deleteSelf()
    {
        DestroyObject(gameObject);
    }
}
