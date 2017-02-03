using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    // Use this for initialization
    private UIRail UI;
	void Start () {
        UI = GetComponent<UIRail>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnSelect(BaseEventData eventData)
    {
        UI.revealUI();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        UI.hideUI();
    }
}
