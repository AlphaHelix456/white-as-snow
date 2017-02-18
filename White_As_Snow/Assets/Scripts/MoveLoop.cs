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
    private float timer;

	// Use this for initialization
	void Start () {
        moveVector = Vector3.left * speed;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        myWidth = (float)GetComponent<RectTransform>().rect.width;
        loopLocation = treeTarget.transform.position;
        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += moveVector * Time.deltaTime;
        if(timer > 0.1f)
        {
            if (cam.WorldToScreenPoint(transform.position).x < -myWidth / 2)
            {
                transform.position = loopLocation;
            }
        } else
        {
            timer += Time.deltaTime;
        }
        
	}
}
