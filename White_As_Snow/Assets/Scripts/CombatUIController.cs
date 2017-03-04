using System.Collections;
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
    
    public Text leftUIMessage;
    public Sprite whiteImg;
    public Sprite emptyImg;
    private Inventory inventory;
    private int inventoryIndex;
    public int menuState;

    private string chooseString;
    private BattleStateMachine BSM;
    public GameObject AttackPanel;

    private const int CHOOSE_ACTION = 0;
    private const int CHOOSE_ATTACK = 1;
    private const int CHOOSE_TARGET = 2;
    private const int CHOOSE_ITEM = 3;
    private const int CHOOSE_ITEM_TARGET = 4;
    private const int CHOOSE_FRIENDLY_TARGET = 5;
    private const int NOT_PLAYER_TURN = 6;
    public bool debug = false;
    private GameObject lastSelectedRightButton;
    private GameObject lastSelectedLeftButton;
    private int row;
    private GameData gameData;
    private WolfCombat[] wolfStats;
    private const int NO_ITEM = 0;
    private const int FOX_MEAT = 1;            //to be changed as real items are designed
    private const int ELK_MEAT = 2;
    private const int SQUIRREL_MEAT = 3;

    private const int FOX_MEAT_HP = 150;    //change these to change item effect
    private const int ELK_MEAT_HP = 200;
    private const int SQUIRREL_MEAT_HP = 50;

    private const int FOX_MEAT_HUNGER = 30;
    private const int ELK_MEAT_HUNGER = 50;
    private const int SQUIRREL_MEAT_HUNGER = 15;
    void Start () {

        menuState = CHOOSE_ACTION;
        inventory = new Inventory();
        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        if(gameData == null)
        {
            print("GameData not found");
        }
        wolfStats = new WolfCombat[3];
        try
        {
            wolfStats[0] = gameData.getWolfStats(0);
            wolfStats[1] = gameData.getWolfStats(1);
            wolfStats[2] = gameData.getWolfStats(2);
        }
        catch
        {
            print("WolfCombat not found");
            wolfStats[0] = new WolfCombat();
            wolfStats[1] = new WolfCombat();
            wolfStats[2] = new WolfCombat();
        }
        try
        {
            
            inventory = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().getInventory();
        }
        catch
        {
            print("Inventory not found");
            inventory = new Inventory(new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 });

        }
        if (debug)
        {
            inventory = new Inventory(new int[8] {1, 3, 2, 2, 2, 1, 2, 3 });
        }
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();

        inventoryIndex = 0;
        row = 0;
        updateButtons();

    }
    
    // Update is called once per frame
    void Update () {
        if (debug && Input.GetKeyDown(KeyCode.I))
        {
            startTurn("Test");
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
            if (menuState == chooseAttack)
            {

            }
        }

    }
    public void startTurn(string wolfName)
    {
        //When a player's turn starts

        menuState = CHOOSE_ACTION;
        chooseString = "Choose an action for " + wolfName + ".";
        leftUIMessage.text = chooseString;
        lastSelectedLeftButton = leftUIButtons[0];
        lastSelectedRightButton = rightUIButtons[0];

        eventSystem.SetSelectedGameObject(rightUIButtons[0]);
    }
    public void fightPress()
    {
        //when the fight button is pressed
        menuState = CHOOSE_ATTACK;
        BSM.WolfInput = BattleStateMachine.WolfGUI.INPUT1;

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
            if (eventSystem.currentSelectedGameObject.CompareTag("Attack 0"))
                chooseAttack(0);
            else if (eventSystem.currentSelectedGameObject.CompareTag("Attack 1"))
                chooseAttack(1);
            else if (eventSystem.currentSelectedGameObject.CompareTag("Attack 2"))
                chooseAttack(2);
            else if (eventSystem.currentSelectedGameObject.CompareTag("Attack 3"))
                chooseAttack(3);
            else 
                chooseAttack(999);
        }
        else if(menuState == CHOOSE_TARGET)
        {
            pickEnemyTarget();
        } else if(menuState == CHOOSE_ITEM)
        {
            chooseItem();
        } else if(menuState == CHOOSE_ITEM_TARGET)
        {
            pickFriendlyTarget();
        } else if (menuState == CHOOSE_FRIENDLY_TARGET)
        {
            pickFriendlyAbilityTarget();
        }

    }
    public void chooseAttack(int attackIndex)
    {
        //Will be called by optionPress depending on menu_state
        bool validChoice = true;  //check to see if this is legal
        if (validChoice)
        {
            if (BSM.WolvesToManage[0].GetComponent<WolfStateMachine>().wolf.availableAttacks[attackIndex].allyTargeted)
            {
                menuState = CHOOSE_FRIENDLY_TARGET;
            }
            else
            {
                menuState = CHOOSE_TARGET;
            }
            lastSelectedLeftButton = eventSystem.currentSelectedGameObject;
            eventSystem.SetSelectedGameObject(leftUIButtons[0]);

            BSM.Input1(attackIndex);
            BSM.WolfInput = BattleStateMachine.WolfGUI.INPUT2;
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
            BSM.Input2(BSM.EnemiesInBattle[0], true);
            BSM.WolfInput = BattleStateMachine.WolfGUI.DONE;

            updateButtons();
            AttackPanel.SetActive(false);

        }
        else
        {

        }
    }
    public void pickFriendlyAbilityTarget()
    {
        //Will be called by optionPress depending on menu_state
        //Determines which wolf was selected in the menu by checking button ID. Searches for the wolf from that button's text.
        bool validChoice = true;  //check to see if this is legal
        if (validChoice)
        {
            menuState = NOT_PLAYER_TURN;
            if (eventSystem.currentSelectedGameObject == leftUIButtons[0])
            {
                BSM.Input2(GameObject.Find(leftUIText[0].text), false);
            }
            else if (eventSystem.currentSelectedGameObject == leftUIButtons[1])
            {
                BSM.Input2(GameObject.Find(leftUIText[1].text), false);
            }
            eventSystem.SetSelectedGameObject(null);
            BSM.WolfInput = BattleStateMachine.WolfGUI.DONE;

            updateButtons();
            AttackPanel.SetActive(false);
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
            int inventorySlot = inventoryIndex + int.Parse(lastSelectedLeftButton.name.Substring(15));
            useInventoryItem(inventorySlot, int.Parse(eventSystem.currentSelectedGameObject.name.Substring(15)));
            eventSystem.SetSelectedGameObject(null);

            BSM.WolfInput = BattleStateMachine.WolfGUI.DONE;

            updateButtons();
            AttackPanel.SetActive(false);


        }
        else
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

                leftUIMessage.enabled = true;
                leftUIMessage.text = chooseString;
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

                leftUIMessage.enabled = true;
                leftUIMessage.text = chooseString;
                eventSystem.SetSelectedGameObject(lastSelectedRightButton);
                lastSelectedRightButton = rightUIButtons[0];
                inventoryIndex = 0;
                break;
            case CHOOSE_ITEM_TARGET:
                menuState = CHOOSE_ITEM;
                lastSelectedLeftButton = leftUIButtons[0];
                eventSystem.SetSelectedGameObject(lastSelectedLeftButton);
                break;
            case CHOOSE_FRIENDLY_TARGET:
                menuState = CHOOSE_ATTACK;
                lastSelectedLeftButton = leftUIButtons[0];
                eventSystem.SetSelectedGameObject(lastSelectedLeftButton);
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
                return "Fox Meat";
            case 2:
                return "Elk Meat";
            case 3:
                return "Squirrel Meat";
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
        //leftUIMessage.enabled = false;
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
                int numAvail = BSM.WolvesToManage[0].GetComponent<WolfStateMachine>().wolf.availableAttacks.Count;
                for (int i = 0; i < numAvail; i++)
                {
                    leftUIButtons[i].tag = "Attack " + i.ToString();
                    leftUIText[i].text = BSM.WolvesToManage[0].GetComponent<WolfStateMachine>().wolf.availableAttacks[i].moveName;
                }
                for (int i = 0; i < 3 - numAvail; i++)
                {
                    leftUIButtons[i+numAvail].tag = "Untagged";
                    leftUIText[i+numAvail].text = "";
                }
                leftUIText[numAvail].text = "Back";
                leftUIButtons[numAvail].tag = "Back";
                setNumOptions(numAvail + 1);
                break;
            case CHOOSE_TARGET:
                int enemiesAvail = BSM.EnemiesInBattle.Count;
                for (int i = 0; i < enemiesAvail; i++)
                {
                    leftUIText[i].text = BSM.EnemiesInBattle[i].name;
                }
                for (int i = 0; i < 3 - enemiesAvail; i++)
                {
                    leftUIText[i + enemiesAvail].text = "";
                }
                leftUIText[enemiesAvail].text = "Back";
                leftUIButtons[enemiesAvail].tag = "Back";
                setNumOptions(enemiesAvail + 1);
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
            case CHOOSE_FRIENDLY_TARGET:
                //Similar to CHOOSE_ITEM_TARGET but the current wolf can't be selected.
                int alliesAvail = BSM.WolvesInBattle.Count - 1;
                int buttonIndex = 0;
                foreach (GameObject wolf in BSM.WolvesInBattle)
                {
                    if (wolf.GetComponent<WolfStateMachine>().wolf.name != BSM.WolvesToManage[0].GetComponent<WolfStateMachine>().wolf.name)
                    {
                        leftUIText[buttonIndex].text = wolf.GetComponent<WolfStateMachine>().wolf.name;
                        buttonIndex++;
                    }
                }
                
                for (int i = 0; i < 3 - alliesAvail; i++)
                {
                    leftUIText[i + alliesAvail].text = "";
                }
                leftUIText[alliesAvail].text = "Back";
                leftUIButtons[alliesAvail].tag = "Back";
                setNumOptions(3);
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
    public void useInventoryItem(int itemToUse, int wolfSelected)
    { //Uses item, removes it from the inventory, and updates inventory

        int itemID = inventory.get(itemToUse);
        //WolfCombat wolf = wolfStats[wolfSelected];
        WolfStateMachine wolf = BSM.WolvesOrderedByDataIndex[wolfSelected].GetComponent<WolfStateMachine>();
        switch (itemID)
        {
            case NO_ITEM:
                throw new System.ArgumentException("CANNOT USE NO_ITEM");
            case FOX_MEAT:
                //conditional assignment, prevents hp from going above maximum
                //wolf.currentHP = Mathf.Min(wolf.currentHP + FOX_MEAT_HP, wolf.baseHP);
                //wolf.currentHunger = Mathf.Min(wolf.currentHunger + FOX_MEAT_HUNGER, wolf.baseHunger);
                wolf.receiveItemHealing(FOX_MEAT_HP, FOX_MEAT_HUNGER);
                break;
            case ELK_MEAT:
                //wolf.currentHP = Mathf.Min(wolf.currentHP + ELK_MEAT_HP, wolf.baseHP);
                //wolf.currentHunger = Mathf.Min(wolf.currentHunger + ELK_MEAT_HUNGER, wolf.baseHunger);
                wolf.receiveItemHealing(ELK_MEAT_HP, ELK_MEAT_HUNGER);
                break;
            case SQUIRREL_MEAT:
                //wolf.currentHP = Mathf.Min(wolf.currentHP + ELK_MEAT_HP, wolf.baseHP);
                //wolf.currentHunger = Mathf.Min(wolf.currentHunger + ELK_MEAT_HUNGER, wolf.baseHunger);
                wolf.receiveItemHealing(SQUIRREL_MEAT_HP, SQUIRREL_MEAT_HUNGER);
                break;
            default:
                throw new System.ArgumentException("ITEM_ID NOT VALID ITEM");
        }
        if (debug)
        {
            print(wolf.name + " HP: " + wolf.wolf.currentHP);
            print(wolf.name + " Hunger: " + wolf.wolf.currentHunger);
        }

        inventory.removeItem(itemToUse);
        wolf.endMove();

    }
}
