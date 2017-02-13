using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFader : MonoBehaviour {

    public float treeFadeAlpha = 0.5f;

	void Start() {
		
	}

	void Update() {
		
	}

    void OnTriggerEnter2D(Collider2D c) {
        if(c.gameObject.tag == "wolf") {
            StartCoroutine(Fade(treeFadeAlpha, 0.15f));
        }
    }

    void OnTriggerExit2D(Collider2D c) {
        if(c.gameObject.tag == "wolf") {
            StartCoroutine(Fade(1, 0.15f));
        }
    }

    IEnumerator Fade(float aValue, float aTime) {
        float alpha = gameObject.GetComponent<SpriteRenderer>().color.a;
        for(float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            gameObject.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
    }
}
