using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyCombat : BaseClass
{
    public enum Encounter
    {
        NORMAL,
        WOLF,
        FINAL
    }

    public Encounter EncounterType;

}