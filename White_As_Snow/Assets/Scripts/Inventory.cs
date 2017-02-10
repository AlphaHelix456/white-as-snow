using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Inventory
{

    // Use this for initialization
    public int[] inventory;
    private const int NO_ITEM = 0;
    private const int APPLE = 1;
    private const int GRAPES = 2;
    private const int MEAT = 3;
    public Inventory(int[] items = null)
    {
        inventory = new int[8];
        if (items != null)
        {
            items.CopyTo(inventory, 0);
        }
        updateInventory(inventory);
    }
    public void updateInventory(int[] newInventory)
    {
        this.inventory = ordered(newInventory);
    }

    private int[] ordered(int[] newInventory)
    {
        //Moves 0's to the end of the array
        List<int> result = new List<int>(newInventory);
        for (int i = result.Count - 1; i >= 0; i--)
        {
            if (result[i] == 0)
            {
                result.RemoveAt(i);
                result.Add(0);
            }
        }
        return result.ToArray();
    }
    public void removeItem(int index)
    {
        /*
         * Uses item (to be coded), removes it from the inventory, and updates inventory
         */
        List<int> newInventory = new List<int>(inventory);
        newInventory.RemoveAt(index);
        newInventory.Add(0);
        updateInventory(ordered(newInventory.ToArray()));


    }

    public int get(int index)
    {
        return inventory[index];
    }
    public int[] toArray()
    {
        return inventory;
    }

}
