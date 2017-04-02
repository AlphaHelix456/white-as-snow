using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CutsceneController : MonoBehaviour {


    public GameObject[] cutsceneList;
    private bool isRunning;
    private bool isEnded;
    private List<GameObject> cutsceneInstructions;
    private int currentCutscene;
    private int instructionsLength;
    public bool debug = false;
    private GameData gameData;
    
	// Use this for initialization
	void Start () {
        isRunning = false;
        isEnded = false;
        cutsceneInstructions = new List<GameObject>();
        currentCutscene = -1;
        if (debug)
        {
            startCutscene(0);
        }
        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        if (gameData == null)
        {
            print("GameData not found");
        }
        startCutscene((int)(gameData.getGameProgress()/2));
    }
	
	// Update is called once per frame
	void Update () {
        if(isRunning)
        {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                endCutscene();
            }

            if (cutsceneInstructions.Count > 0)
            {
                
                cutsceneInstructions = buildCutsceneInstructions(currentCutscene);
           

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
                        case "SwapSprite":
                            cutsceneInstructions[index].GetComponent<SwapSprite>().activate();
                            //SwapSprite doesn't have duration, so it can't block
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
        
    }
    public void endCutscene()
    {
        isEnded = true;
        isRunning = false;
        SceneManager.LoadScene("World");
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
