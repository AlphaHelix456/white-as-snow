using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour {

    // Use this for initialization
    public Image image;
    public bool reversed;
    public float duration;
    public bool blocking = true;
    private float totalDuration;
    private bool isRunning;
    private Color clearColor;
    private Color blackColor;
	void Start () {
        isRunning = false;
        totalDuration = duration;
        clearColor = new Color(0, 0, 0, 0);
        blackColor = new Color(0, 0, 0, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            image.enabled = true;
            if(totalDuration <= 0)
            {
                if (reversed)
                {
                    image.color = clearColor;
                } else
                {
                    image.color = blackColor;
                }
            } else
            {
                if (reversed)
                {
                    image.color = Color.Lerp(clearColor, blackColor, duration / totalDuration);
                }
                else
                {
                    image.color = Color.Lerp(blackColor, clearColor, duration / totalDuration);
                }
            }
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                deleteSelf();
            }

        }
	}
    private void deleteSelf()
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
