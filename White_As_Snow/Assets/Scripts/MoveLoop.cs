using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoop : MonoBehaviour {

    public GameObject treeTarget;
    public float speed;
    public Camera cam;
    float camHeight;
    float camWidth;
    private Vector3 loopLocation;
    private float myWidth;
    private Vector3 moveVector;

	// Use this for initialization
	void Start () {
        moveVector = Vector3.left * speed;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        print(camWidth);
        myWidth = (float)GetComponent<RectTransform>().rect.width;
        loopLocation = treeTarget.transform.position;
        
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += moveVector * Time.deltaTime;
        if (cam.WorldToScreenPoint(transform.position).x < -myWidth/2)
        {
            //transform.position = new Vector2(cam.ViewportToWorldPoint(new Vector3(1,0,0)).x+myWidth/2,transform.position.y);
            transform.position = loopLocation;
        }
	}
}
