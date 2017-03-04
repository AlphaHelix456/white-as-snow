using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour {


    public GameObject[] cutsceneList;
    private bool isRunning;
    private bool isEnded;
    private List<GameObject> cutsceneInstructions;
    private int currentCutscene;
    private int instructionsLength;
    public bool debug = false;
    private GameObject[] inventoryUI;
	// Use this for initialization
	void Start () {
        isRunning = false;
        isEnded = false;
        inventoryUI = GameObject.FindGameObjectsWithTag("Inventory");
        cutsceneInstructions = new List<GameObject>();
        currentCutscene = -1;
        if (debug)
        {
            startCutscene(0);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        if(isRunning)
        {
            

            if (cutsceneInstructions.Count > 0)
            {
                if (cutsceneInstructions[0] == null)
                {
                    cutsceneInstructions = buildCutsceneInstructions(currentCutscene);
                }

                int index = 0;
                while(index < cutsceneInstructions.Count)
                {
                    //If an instruction isn't blocking, activate it and continue to the next
                    //continue until you reach a blocking instruction
                    bool blocking = false;
                    
                    switch (cutsceneInstructions[index].tag)
                    {
                        case "MoveTo":
                            cutsceneInstructions[index].GetComponent<MoveTo>().activate();
                            if (cutsceneInstructions[index].GetComponent<MoveTo>().isBlocking())
                            {
                                blocking = true;
                            }
                            break;
                            
                        case "RevealImg":
                            cutsceneInstructions[index].GetComponent<RevealImg>().activate();
                            if (cutsceneInstructions[index].GetComponent<RevealImg>().isBlocking())
                            {
                                blocking = true;
                            }
                            break;
                        case "WaitFor":
                            cutsceneInstructions[index].GetComponent<WaitFor>().activate();
                            if (cutsceneInstructions[index].GetComponent<WaitFor>().isBlocking())
                            {
                                blocking = true;
                            }
                            break;
                        case "RunAnimation":
                            cutsceneInstructions[index].GetComponent<RunAnimation>().activate();
                            if (cutsceneInstructions[index].GetComponent<RunAnimation>().isBlocking())
                            {
                                blocking = true;
                            }
                            break;
                        case "Fade":
                            cutsceneInstructions[index].GetComponent<FadeToBlack>().activate();
                            if (cutsceneInstructions[index].GetComponent<FadeToBlack>().isBlocking())
                            {
                                blocking = true;
                            }
                            break;
                    }
                    index++;
                    if (blocking)
                    {
                        break;
                    }
                    
                } 
                
            }
            else
            {
                
                endCutscene();
            }
        }
         
    }
    public void startCutscene(int sceneNum)
    {
        isRunning = true;
        isEnded = false;
        currentCutscene = sceneNum;
        cutsceneInstructions = buildCutsceneInstructions(sceneNum);
        for (int i = 0; i < inventoryUI.Length; i++)
        {
            inventoryUI[i].SetActive(false);
        }
    }
    public void endCutscene()
    {
        isEnded = true;
        isRunning = false;
        for (int i = 0; i < inventoryUI.Length; i++)
        {
            inventoryUI[i].SetActive(true);
        }
    }
    public List<GameObject> buildCutsceneInstructions(int sceneNum)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (Transform child in cutsceneList[sceneNum].transform)
        {

            result.Add(child.gameObject);
            
        }
        return result;
    }
}
