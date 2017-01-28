using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour {

    // Use this for initialization
    private int menuState;
    private const int MAIN_MENU = 0;  //Use these constants to represent menustate more easily
    private const int CREDITS = 1;
    void Start () {
        menuState = MAIN_MENU;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void startGame()
    {
        //Runs when start button is pressed; switches to game scene

        //SceneManager.LoadScene(SCENE_NAME);  Uncomment once scene is created
    }
    public void quitGame()
    {
        //Quits the game when quit button is pressed
        Application.Quit();
    }
    public void goToCredits()
    {
        //Activates when credits is pressed, moves to credit screen
        menuState = CREDITS;
    }
    public void returnToMainMenu()
    {
        //Activates when the back button is pressed, returns to main menu screen
        menuState = MAIN_MENU;
    }
}
