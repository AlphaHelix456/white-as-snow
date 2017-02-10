using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySpriteManager : MonoBehaviour {

    // Use this for initialization
    public List<Sprite> itemSprites;   //Sprite location in the list must match its ID (ex: ID of 1 is in 0th position)
                                       //ID of 0 is NO ITEM
    public List<Image> invSquares;    //Image components of all inventory squares

	void Start () {

	}
    
    public void updateSprites(int[] newInventory)
    {
        for(int i = 0; i < invSquares.Count; i++)
        {
            invSquares[i].sprite = itemSprites[newInventory[i]];
        }
    }

}
