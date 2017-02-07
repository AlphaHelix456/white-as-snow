using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySpriteManager : MonoBehaviour {

    // Use this for initialization
    public List<Sprite> itemSprites;   //Sprite location in the list must match its ID (ex: ID of 1 is in 0th position)
                                       //ID of 0 is NO ITEM
    public List<Image> invSquares;    //Image components of all inventory squares
    private const int EMPTY = 0;       //ID of 0 is NO ITEM
	void Start () {
        updateInventory(new int[8] { 0, 0, 0, 1, 2, 3, 3, 3 }); //remove later
	}
    
    public void updateInventory(int[] newInventory)
    {
        updateSprites(ordered(newInventory));
    }
    private void updateSprites(int[] newInventory)
    {
        for(int i = 0; i < invSquares.Count; i++)
        {
            invSquares[i].sprite = itemSprites[newInventory[i]];
        }
    }
    private int[] ordered(int[] newInventory)
    {
        //Moves 0's to the end of the array
        List<int> result = new List<int>(newInventory);
        for(int i = result.Count-1; i >= 0; i--)
        {
            if(result[i] == EMPTY)
            {
                result.RemoveAt(i);
                result.Add(EMPTY);
            }
        }
        return result.ToArray();
    }
}
