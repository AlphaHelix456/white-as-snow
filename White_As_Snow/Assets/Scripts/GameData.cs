﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameData : MonoBehaviour {

    // Use this for initialization
    private WolfCombat[] wolfStats;
    private const int ALPHA = 0;
    private const int CAUTION = 1;
    private const int HYPER = 2;

    public int gameProgress;
    private const int OVERWORLD_0 = 0;
    private const int BATTLE_0 = 1;
    private const int OVERWORLD_1 = 2;
    private const int BATTLE_1 = 3;

    private Inventory inventory;
	void Start () {
        if(GameObject.FindGameObjectsWithTag("GameData").Length == 1)
        {
            DontDestroyOnLoad(this);
        } else
        {
            DestroyObject(this);
        }
        wolfStats[0] = new WolfCombat();
        wolfStats[ALPHA].name = "Alpha";
        wolfStats[ALPHA].baseHP = 300;
        wolfStats[ALPHA].currentHP = 300;
        wolfStats[ALPHA].baseHunger = 100;
        wolfStats[ALPHA].currentHunger = 100;
        wolfStats[ALPHA].baseATK = 50;
        wolfStats[ALPHA].currentATK = 50;
        wolfStats[ALPHA].baseDEF = 100;
        wolfStats[ALPHA].currentDEF = 100;
        wolfStats[ALPHA].baseSPD = 100;
        wolfStats[ALPHA].currentSPD = 100;
        wolfStats[ALPHA].baseCRIT = 0;
        wolfStats[ALPHA].currentCRIT = 0;

        wolfStats[1] = new WolfCombat();
        wolfStats[CAUTION].name = "Caution";
        wolfStats[CAUTION].baseHP = 300;
        wolfStats[CAUTION].currentHP = 300;
        wolfStats[CAUTION].baseHunger = 100;
        wolfStats[CAUTION].currentHunger = 100;
        wolfStats[CAUTION].baseATK = 50;
        wolfStats[CAUTION].currentATK = 50;
        wolfStats[CAUTION].baseDEF = 100;
        wolfStats[CAUTION].currentDEF = 100;
        wolfStats[CAUTION].baseSPD = 100;
        wolfStats[CAUTION].currentSPD = 100;
        wolfStats[CAUTION].baseCRIT = 0;
        wolfStats[CAUTION].currentCRIT = 0;

        wolfStats[2] = new WolfCombat();
        wolfStats[HYPER].name = "Hyper";
        wolfStats[HYPER].baseHP = 300;
        wolfStats[HYPER].currentHP = 300;
        wolfStats[HYPER].baseHunger = 100;
        wolfStats[HYPER].currentHunger = 100;
        wolfStats[HYPER].baseATK = 50;
        wolfStats[HYPER].currentATK = 50;
        wolfStats[HYPER].baseDEF = 100;
        wolfStats[HYPER].currentDEF = 100;
        wolfStats[HYPER].baseSPD = 100;
        wolfStats[HYPER].currentSPD = 100;
        wolfStats[HYPER].baseCRIT = 0;
        wolfStats[HYPER].currentCRIT = 0;

        gameProgress = OVERWORLD_0;
        inventory = new Inventory(new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 });
    }
	
	// Update is called once per frame
	void Update () {
     
    }
    public void setWolfStats(int wolfIndex, WolfCombat newStats)
    {
        wolfStats[wolfIndex] = newStats;
    }
    public WolfCombat getWolfStats(int wolfIndex)
    {
        return wolfStats[wolfIndex];
    }
    public Inventory getInventory()
    {
        return inventory;
    }
    public void setInventory(Inventory i)
    {
        inventory = i;
    }
    public int getGameProgress()
    {
        return gameProgress;
    }
    public void setGameProgress(int x)
    {
        gameProgress = x;
    }
}
