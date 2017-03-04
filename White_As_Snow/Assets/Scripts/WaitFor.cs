using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitFor : MonoBehaviour {

    // Use this for initialization
    public float duration;
    public bool blocking = true;

    private bool isRunning;
    
	void Start () {
        isRunning = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            duration -= Time.deltaTime;
            if(duration < 0)
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
