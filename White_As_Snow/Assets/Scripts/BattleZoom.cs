using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZoom : MonoBehaviour {

    private Camera myCamera; 

	// Use this for initialization
	void Start () {
        myCamera = this.gameObject.GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void Update () {
        if (myCamera.orthographicSize > 5)
            decreaseSize();
        if (myCamera.orthographicSize < 5)
        {
            myCamera.orthographicSize = 5;
        }
    }
    void decreaseSize()
    {
        myCamera.orthographicSize -= 5 * Time.deltaTime;
    }
}
