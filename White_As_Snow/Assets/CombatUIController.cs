﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CombatUIController : MonoBehaviour {

    // Use this for initialization
    public EventSystem eventSystem;
    public GameObject[] leftUIButtons;
    public GameObject[] rightUIButtons;
    public Text[] leftUIText;
    public WolfCombat[] wolves;
    public Text leftUIMessage;
    public Sprite whiteImg;
    public Sprite emptyImg;
    private Inventory inventory;
    private int inventoryIndex;
    public int menuState;
    private const int CHOOSE_ACTION = 0;
    private const int CHOOSE_ATTACK = 1;
    private const int CHOOSE_TARGET = 2;
    private const int CHOOSE_ITEM = 3;
    private const int CHOOSE_ITEM_TARGET = 4;
    private const int NOT_PLAYER_TURN = 5;
    public bool debug = false;
    private GameObject lastSelectedRightButton;
    private GameObject lastSelectedLeftButton;
    private int row;
    void Start () {
        menuState = CHOOSE_ACTION;
        if (debug)
        {
            inventory = new Inventory(new int[8] {1, 3, 2, 2, 2, 1, 2, 3 });
        }
        inventoryIndex = 0;
        row = 0;
        updateButtons();
	}
	
	// Update is called once per frame
	void Update () {
        if (debug && Input.GetKeyDown(KeyCode.I))
        {
            startTurn();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            back();
        }
        if (menuState == CHOOSE_ITEM)
        {
            if (row == 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {

                    if (inventoryIndex >= 2)
                    {
                        inventoryIndex -= 2;
                        updateButtons();
                    }
                }

            }
            else if (row == 1)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    if (inventoryIndex <= inventory.size()-4)
                    {
                        inventoryIndex += 2;
                        updateButtons();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                row = 0; //This fixes an issue where Unity would switch buttons, then run code to check what is the currently selected button afterwards
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                row = 1;
            }
            if (eventSystem.currentSelectedGameObject.activeSelf == false)
            {
                //Fixes case where moving down the inventory would place your pointer on a deactivated button
                if (eventSystem.currentSelectedGameObject == leftUIButtons[2])
                {
                    eventSystem.SetSelectedGameObject(leftUIButtons[0]);
                }
                else if (eventSystem.currentSelectedGameObject == leftUIButtons[3])
                {
                    eventSystem.SetSelectedGameObject(leftUIButtons[1]);
                }
                row = 0;
            }
        }
	}
    public void startTurn()
    {
        //When a player's turn starts
        menuState = CHOOSE_ACTION;
        leftUIMessage.text = "Choose an action";
        lastSelectedLeftButton = leftUIButtons[0];
        lastSelectedRightButton = rightUIButtons[0];
        eventSystem.SetSelectedGameObject(rightUIButtons[0]);
    }
    public void fightPress()
    {
        //when the fight button is pressed
        menuState = CHOOSE_ATTACK;
        updateButtons();
        lastSelectedRightButton = eventSystem.currentSelectedGameObject;
        eventSystem.SetSelectedGameObject(leftUIButtons[0]);
        leftUIMessage.enabled = false;
    }
    public void itemPress()
    {
        //when the item button is pressed
        menuState = CHOOSE_ITEM;
        updateButtons();
        lastSelectedRightButton = eventSystem.currentSelectedGameObject;
        eventSystem.SetSelectedGameObject(leftUIButtons[0]);
        leftUIMessage.enabled = false;
    }
    public void optionPress()
    {
        //All the left buttons go through this function.  What happens depends on the menuState
        if (eventSystem.currentSelectedGameObject.CompareTag("Back"))
        {
            //if the button is a back button, none of the other methods get called
            back();
        }
        else if (menuState == CHOOSE_ATTACK)
        {
            chooseAttack();
        } else if(menuState == CHOOSE_TARGET)
        {
            pickEnemyTarget();
        } else if(menuState == CHOOSE_ITEM)
        {
            chooseItem();
        } else if(menuState == CHOOSE_ITEM_TARGET)
        {
            pickFriendlyTarget();
        }
    }
    public void chooseAttack()
    {
        //Will be called by optionPress depending on menu_state
        bool validChoice = true;  //check to see if this is legal
        if (validChoice)
        {
            menuState = CHOOSE_TARGET;
            lastSelectedLeftButton = eventSystem.currentSelectedGameObject;
            eventSystem.SetSelectedGameObject(leftUIButtons[0]);
            updateButtons();
        }
        else
        {

        }
    }
    public void chooseItem()
    {
        //Will be called by optionPress depending on menu_state
        bool validChoice = true;  //check to see if this is legal
        if (validChoice)
        {
            menuState = CHOOSE_ITEM_TARGET;
            lastSelectedLeftButton = eventSystem.currentSelectedGameObject;
            eventSystem.SetSelectedGameObject(leftUIButtons[0]);
            updateButtons();
            inventoryIndex = 0;
        }
        else
        {

        }
    }
   
    public void pickEnemyTarget()
    {
        //Will be called by optionPress depending on menu_state
        bool validChoice = true;  //check to see if this is legal
        if (validChoice)
        {
            menuState = NOT_PLAYER_TURN;
            eventSystem.SetSelectedGameObject(null);
            updateButtons();
        }
        else
        {

        }
    }
    public void pickFriendlyTarget()
    {
        //Will be called by optionPress depending on menu_state
        bool validChoice = true;  //check to see if this is legal
        if (validChoice)
        {
            menuState = NOT_PLAYER_TURN;
            eventSystem.SetSelectedGameObject(null);
            updateButtons();
            int inventorySlot = inventoryIndex + int.Parse(lastSelectedLeftButton.name.Substring(15));
            inventory.removeItem(inventorySlot);
        } else
        {

        }
  
      
    }
    public void back()
    {
        //Will be called by chooseattack,chooseitem,pickenemytarget,pickfriendlytarget 
        switch (menuState)
        {
            case CHOOSE_ATTACK:
                menuState = CHOOSE_ACTION;
                eventSystem.SetSelectedGameObject(lastSelectedRightButton);
                lastSelectedRightButton = rightUIButtons[0];
                break;
            case CHOOSE_TARGET:
                menuState = CHOOSE_ATTACK;
                eventSystem.SetSelectedGameObject(lastSelectedLeftButton);
                lastSelectedLeftButton = leftUIButtons[0];
                break;
            case CHOOSE_ITEM:
                menuState = CHOOSE_ACTION;
                eventSystem.SetSelectedGameObject(lastSelectedRightButton);
                lastSelectedRightButton = rightUIButtons[0];
                inventoryIndex = 0;
                break;
            case CHOOSE_ITEM_TARGET:
                menuState = CHOOSE_ITEM;
                eventSystem.SetSelectedGameObject(lastSelectedRightButton);
                lastSelectedLeftButton = leftUIButtons[0];
                break;


        }
        updateButtons();
    }
    
    public void setNumOptions(int numButtons)
    {
        //If there are less than 4 options, some buttons will be disabled
        for(int i = 0; i < leftUIButtons.Length; i++)
        {
            leftUIButtons[i].SetActive(true);
            leftUIText[i].enabled = true;
        }
        for(int i = 0; i < (4-numButtons); i++)
        {
            leftUIButtons[3-i].SetActive(false);
            leftUIText[3 - i].enabled = false;
        }
    }
    public void displayInventory()
    {
        int errorCount = 0;
        for(int i = 0; i < 4; i++)
        {
            try
            {
                leftUIText[i].text = getItemName(inventory.get(inventoryIndex + i));
            }
            catch
            {
                //will happen if inventory slot is empty or index+i goes out of inventory array length
                leftUIText[i].text = "";
                errorCount++;
            }

        }
        if(errorCount > 0) //if we've hit empty inventory slots or out of bounds
        {
            //set up a back button
            setNumOptions(4 - (errorCount - 1));
            leftUIText[4 - errorCount].text = "Back";
            leftUIButtons[4 - errorCount].tag = "Back";
        } else
        {
            setNumOptions(4);
        }
    }
    public string getItemName(int itemID)
    {
        switch (itemID)
        {
            case 0:
                throw new System.ArgumentException();
            case 1:
                return "Apple";
            case 2:
                return "Grapes";
            case 3:
                return "Meat";
            default:
                throw new System.ArgumentException("ITEM_ID NOT VALID ITEM");
        }
        

    }
    public void updateButtons()
    {
        setNumOptions(4);
        leftUIButtons[0].tag = "Untagged";
        leftUIButtons[1].tag = "Untagged";
        leftUIButtons[2].tag = "Untagged";
        leftUIButtons[3].tag = "Untagged";
        leftUIMessage.enabled = false;
        switch (menuState)
        {
            case CHOOSE_ACTION:
                leftUIText[0].text = "";
                leftUIText[1].text = "";
                leftUIText[2].text = "";
                leftUIText[3].text = "";
                setNumOptions(0);
                break;
            case CHOOSE_ATTACK:
                leftUIText[0].text = "Bite";
                leftUIText[1].text = "Swipe";
                leftUIText[2].text = "Back";
                leftUIText[3].text = "";
                leftUIButtons[2].tag = "Back";
                setNumOptions(3);
                break;
            case CHOOSE_TARGET:
                leftUIText[0].text = "Tom Nook";
                leftUIText[1].text = "Back";
                leftUIText[2].text = "";
                leftUIText[3].text = "";
                leftUIButtons[1].tag = "Back";
                setNumOptions(2);
                break;
            case CHOOSE_ITEM:
                leftUIText[0].text = "";
                leftUIText[1].text = "";
                leftUIText[2].text = "";
                leftUIText[3].text = "";
                displayInventory();
                break;
            case CHOOSE_ITEM_TARGET:
                leftUIText[0].text = "Alpha";
                leftUIText[1].text = "Caution";
                leftUIText[2].text = "Hyper";
                leftUIText[3].text = "Back";
                leftUIButtons[3].tag = "Back";
                setNumOptions(4);
                break;
            case NOT_PLAYER_TURN:
                leftUIText[0].text = "";
                leftUIText[1].text = "";
                leftUIText[2].text = "";
                leftUIText[3].text = "";
                setNumOptions(0);
                leftUIMessage.enabled = true;
                leftUIMessage.text = "This is not the player's turn";
                break;
        }
    }
}
