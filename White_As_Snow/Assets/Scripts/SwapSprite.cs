using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapSprite : MonoBehaviour {

    // Use this for initialization
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public Color color = Color.white;
    public bool flipX = false;
    
    
    void Start () {
		
	}
	public void deleteSelf()
    {
        DestroyObject(gameObject);
    }
    public void activate()
    {
        spriteRenderer.sprite = newSprite;
        spriteRenderer.color = color;
        spriteRenderer.flipX = flipX;
        deleteSelf();
    }
    public bool isBlocking()
    {
        //Swapsprite doesn't have any duration, so it doesn't make sense to block
        return false;
    }
}
