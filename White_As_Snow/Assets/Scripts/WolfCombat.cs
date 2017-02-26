using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WolfCombat
{
    public string name;

    public float baseHP;
    public float currentHP;

    public float baseHunger;
    public float currentHunger;

    public float baseATK;
    public float currentATK;
    public float baseDEF;
    public float currentDEF;

    public float baseSPD;
    public float currentSPD;

    public float baseCRIT;
    public float currentCRIT;

    public List<BaseAttack> availableAttacks = new List<BaseAttack>();
}
