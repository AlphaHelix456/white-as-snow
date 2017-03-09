using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneChanger : MonoBehaviour {

    // Use this for initialization
    private GameData gameData;
	void Start () {
        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        if(gameData == null)
        {
            print("No gameData found");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D c) {
        if(c.gameObject.tag == "wolf") {
            gameData.setGameProgress(gameData.getGameProgress() + 1);
            SceneManager.LoadScene("JakeSmith");
        }
    }
}
