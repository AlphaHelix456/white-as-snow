using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class InventoryMenuController : MonoBehaviour
{

    // Use this for initialization
    public EventSystem eventSystem;
    public GameObject[] inventoryPanels;
    public GameObject[] itemOptionButtons;
    public GameObject backButton;
    public GameObject selectionMenu;
    public GameObject itemDesc;
    public UIRail inventoryUI;
    public InventorySpriteManager spriteManager;
    public Sprite whiteImg;
    public Sprite emptyImg;

    public string[] itemText;  //slot 0 is intentionally left blank
    public string[] itemUseText;
    private int selectedItemID;
    private int selectedItemSlot;
    private Inventory inventory;

    private int menuState;
    private const int NOT_IN_MENU = 0;
    private const int INVENTORY_PANELS = 1;
    private const int ITEM_OPTION_SELECT = 2;

    private const int NO_ITEM = 0;
    private const int APPLE = 1;            //to be changed as real items are designed
    private const int GRAPES = 2;
    private const int MEAT = 3;

    public bool debug = false;
	void Start () {
        selectedItemID = 0;
        selectedItemSlot = 0;
        inventory = new Inventory();
        if (debug)
        {
            inventory = new Inventory(new int[8] { 1, 2, 3, 0, 0, 2, 3, 1 });  //remove later
        }
        spriteManager.updateSprites(inventory.toArray());
        menuState = NOT_IN_MENU;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (menuState == NOT_IN_MENU)
            { openInventory(); }
            else
            { closeInventory(); }
        }
        if (menuState != NOT_IN_MENU)
        {
            if (menuState == INVENTORY_PANELS)
            {
                if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)
                    || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    //If any directional input is done
                    GameObject buttonSelected = eventSystem.currentSelectedGameObject;
                    if (buttonSelected.name != "BackButton")
                    {
                        itemDesc.GetComponent<Text>().text = itemText[inventory.get(int.Parse(buttonSelected.name.Substring(3)))];
                    }
                }
                
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    closeInventory();
                }
            } else if (menuState == ITEM_OPTION_SELECT)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cancelPress();
                }
            }
            
        } else
        {

        }

    }
    
    public void openInventory()
    {
        inventoryUI.revealUI();
        eventSystem.SetSelectedGameObject(inventoryPanels[0]);
        itemDesc.SetActive(true);
        selectionMenu.SetActive(false);
        menuState = INVENTORY_PANELS;
        itemDesc.GetComponent<Text>().text = itemText[inventory.get(0)];

    }
    public void closeInventory()
    {
        inventoryUI.hideUI();
        eventSystem.SetSelectedGameObject(null);
        itemDesc.SetActive(true);
        selectionMenu.SetActive(false);
        menuState = NOT_IN_MENU;
    }
    public void backPress()
    {
        closeInventory();
    }
    public void itemPress()
    {
        GameObject itemSelected = eventSystem.currentSelectedGameObject;
        int tempSelectedItemID = inventory.get(int.Parse(itemSelected.name.Substring(3))); //only "box[01234567]" will call this function
                                                                                       //This takes the number on the end and uses it as an index for inventory
                                                                                       //This returns the item ID in that inventory slot
        if(tempSelectedItemID != 0)         //Don't do anything if the inventory slot is an empty slot
        {
            itemSelected.GetComponent<Image>().sprite = whiteImg;          //Keeps the inv slot outlined in white
            selectedItemID = tempSelectedItemID;
            selectedItemSlot = int.Parse(itemSelected.name.Substring(3));  //remembers what inventory panel is selected for if you cancel item use
            itemDesc.SetActive(false);
            selectionMenu.SetActive(true);                                        //It will return you to the item you had selected instead of the top left item
            eventSystem.SetSelectedGameObject(itemOptionButtons[0]); //Places button selection on option 1 in textregion
            menuState = ITEM_OPTION_SELECT;
        }
    }
    public void optionPress()
    {
        this.useInventoryItem(selectedItemSlot, int.Parse(eventSystem.currentSelectedGameObject.name.Substring(14)));
        itemDesc.SetActive(true);
        selectionMenu.SetActive(false);
        inventoryPanels[selectedItemSlot].GetComponent<Image>().sprite = emptyImg; //Removes manual white outline and returns to automatic mode for outlines
        eventSystem.SetSelectedGameObject(inventoryPanels[selectedItemSlot]);
        menuState = INVENTORY_PANELS;
        itemDesc.GetComponent<Text>().text = itemUseText[(int)(Random.value * 4)];
    }
    public void cancelPress()
    {

        inventoryPanels[selectedItemSlot].GetComponent<Image>().sprite = emptyImg; //Removes manual white outline and returns to automatic mode for outlines

        eventSystem.SetSelectedGameObject(inventoryPanels[selectedItemSlot]);
        itemDesc.SetActive(true);
        selectionMenu.SetActive(false);
        menuState = INVENTORY_PANELS;
    }
    public void updateInventory(int[] newInventory)
    {
        inventory.updateInventory(newInventory);
        spriteManager.updateSprites(inventory.toArray());
    }
    public void useInventoryItem(int itemToUse, int optionSelected)
    { //Uses item (to be coded), removes it from the inventory, and updates inventory

        int itemID = inventory.get(itemToUse);
        switch (itemID)
        {
            case NO_ITEM:
                throw new System.ArgumentException("CANNOT USE NO_ITEM");
            case APPLE:
                break;
            case GRAPES:
                break;
            case MEAT:
                break;
            default:
                throw new System.ArgumentException("ITEM_ID NOT VALID ITEM");
        }
        inventory.removeItem(itemToUse);
        spriteManager.updateSprites(inventory.toArray());
    }
    
}
