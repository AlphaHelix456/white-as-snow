using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoop : MonoBehaviour {

    public float speed;
    private Vector3 loopLocation;
    private float myWidth;
    private Vector3 moveVector;
	// Use this for initialization
	void Start () {
        moveVector = Vector3.left * speed;
        myWidth = (float)GetComponent<RectTransform>().rect.width;
        loopLocation = new Vector3(Screen.width + myWidth/2, transform.position.y,0);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += moveVector * Time.deltaTime;
		if(transform.position.x < 0 - myWidth/2)
        {
            transform.position = loopLocation;
        }
	}
}
