using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitality : BaseAttack {

	public Vitality()
    {
        moveName = "Vitality Howl";
        moveDescription = "Heal target wolf for a small amount.";
        moveValue = 35f;
        allyTargeted = true;
    }
}
