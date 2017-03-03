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
    private float timer;
    private bool isRunning;
    private Sprite originalSprite;
    void Start()
    {
        timer = 0;
        isRunning = false;
        animator.enabled = false;
        originalSprite = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
            if(timer > duration)
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
