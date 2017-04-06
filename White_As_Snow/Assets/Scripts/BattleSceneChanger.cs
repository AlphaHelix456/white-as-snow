using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneChanger : MonoBehaviour {

    // Use this for initialization
    private GameData gameData;
    public GameObject whiteFlash;
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
            StartCoroutine(whiteTransition());
            gameData.setGameProgress(gameData.getGameProgress() + 1);
        }
    }
    IEnumerator whiteTransition()
    {
        byte alpha = 0;
        whiteFlash.SetActive(true);

        while (alpha < 255)
        {
            yield return new WaitForSeconds(0.005f);
            alpha += 3;
            whiteFlash.GetComponent<Image>().color = new Color32(255, 255, 255, alpha);
        }
        SceneManager.LoadScene("JakeSmith");
    }
}
