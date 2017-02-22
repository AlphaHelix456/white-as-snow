using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack : MonoBehaviour {
    public string moveName;
    public string moveDescription;

    public float moveValue; //attack or heal value
    public bool allyTargeted = false;
}
