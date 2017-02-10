using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyCombat
{
    new public string name;

    public enum Encounter
    {
        NORMAL,
        WOLF,
        FINAL
    }

    public Encounter EncounterType;

    public float baseHP;
    public float currentHP;

    public float baseATK;
    public float currentATK;
    public float baseDEF;
    public float currentDEF;

    public float baseSPD;
    public float currentSPD;

    public float baseCRIT;
    public float currentCRIT;

}