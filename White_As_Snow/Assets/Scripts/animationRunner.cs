using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class animationRunner : MonoBehaviour {
    public Sprite[] sprites;
    public float animationSpeed;
    private Image image;
    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
        StartCoroutine(playAnimation());
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
}
