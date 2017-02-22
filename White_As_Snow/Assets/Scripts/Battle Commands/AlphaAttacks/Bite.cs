using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite : BaseAttack {

	public Bite()
    {
        moveName = "Bite";
        moveDescription = "Lunges at a single enemy with a chance to do double damage.";

        moveValue = 30f;
    }
}
