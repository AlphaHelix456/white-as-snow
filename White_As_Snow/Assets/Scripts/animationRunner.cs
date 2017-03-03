using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class animationRunner : MonoBehaviour {
    public Sprite[] sprites;
    public float animationSpeed;
    public bool isRunning = true;
    private Image image;
    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
        if (isRunning)
        {
            StartCoroutine(playAnimation());
        } else
        {
            image.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
       
	}
    

    public IEnumerator playAnimation()
    {
        //destroy all game objects
        for (int i = 0; i < sprites.Length; i++)
        {
            image.sprite = sprites[i];
            yield return new WaitForSeconds(animationSpeed);
        }
        StartCoroutine(playAnimation());
    }
    public void activate()
    {
        if (!isRunning)
        {
            isRunning = true;
            image.enabled = true;
            StartCoroutine(playAnimation());
        }
        
    }
}
