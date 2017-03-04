using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAnimation : MonoBehaviour {

    // Use this for initialization
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public string animationName;
    public bool flipX = false;
    public float duration;
    public bool blocking = true;
    private bool isRunning;
    private Sprite originalSprite;
    void Start()
    {
        isRunning = false;
		animator = gameObject.GetComponent<Animator> ();
        animator.enabled = false;
        originalSprite = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            duration -= Time.deltaTime;
            if(duration < 0)
            {
                deleteSelf();
            }
        }
    }
    public void deleteSelf()
    {
        animator.enabled = false;
        spriteRenderer.sprite = originalSprite;
        DestroyObject(gameObject);
    }
    public void activate()
    {
        if (!isRunning)
        {
            isRunning = true;
            animator.enabled = true;
            animator.Play(animationName);
            if (flipX)
            {
                spriteRenderer.flipX = true;
            }
        }
    }
    public bool isBlocking()
    {
        return blocking;
    }
}
