using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour {

    // Use this for initialization
    public UIRevealer[] nameList;
    public UIRevealer[] roleList;
    public UIRevealer blackPanel;
    public float initialDelay;
    public float gapDelay;
    private IEnumerator creditsEntrance;
	void Start () {
        creditsEntrance = CreditsEntrance();
	}
	
    public void revealCredits()
    {
        creditsEntrance = CreditsEntrance();
        StartCoroutine(creditsEntrance);
    }
    public void reset()
    {
        StopCoroutine(creditsEntrance);
        blackPanel.hideUI();
        for (int i = 0; i < nameList.Length; i++)
        {
            nameList[i].hideUI();
            roleList[i].hideUI();
        }
    }
    IEnumerator CreditsEntrance()
    {
        blackPanel.revealUI();
        yield return new WaitForSeconds(initialDelay);
        for(int i = 0; i < nameList.Length; i++)
        {
            nameList[i].revealUI();
            roleList[i].revealUI();
            yield return new WaitForSeconds(gapDelay);
        }
    }
}
