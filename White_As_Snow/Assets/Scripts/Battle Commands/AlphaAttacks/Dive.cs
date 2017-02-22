using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dive : BaseAttack { 
    public Dive()
    {
        moveName = "Dive";
        moveDescription = "Dive in front of an ally wolf, taking their next instance of damage.";
        moveValue = 0f;
        allyTargeted = true;
    }
	
}
