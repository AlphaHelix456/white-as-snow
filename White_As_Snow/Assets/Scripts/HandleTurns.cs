using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurns {
    public string Attacker; //name of attacker
    public string Type;
    public GameObject AttackerGameObject; //GameObject of attacker
    public GameObject AttackersTarget; //GameObject of attacker's target

    //Later: what attack is being performed
    public BaseAttack chosenMove;
    //Is this attack friendly (ally-targeted) or hostlie (enemy-targeted)?
    public bool hostileAttack;
	
}
