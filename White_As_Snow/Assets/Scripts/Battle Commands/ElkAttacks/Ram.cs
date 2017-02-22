using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : BaseAttack {

    public Ram()
    {
        moveName = "Ram";
        moveDescription = "Charge a single enemy, dealing damage."; //second functionality = reset ATB gauge? probably save for later enemy

        moveValue = 50f;
    }
}
