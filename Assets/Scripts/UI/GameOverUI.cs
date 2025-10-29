using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverUI : MonoBehaviour
{
    public string mainMenuScene = "MainMenu";

    public void Retry(){
        Time.timeScale = 1f;
        // default to Level_Day’s index (change 1 if needed)
        int idx = PlayerPrefs.GetInt("RetryIndex", 1);
        Debug.Log("[GameOverUI] Retry -> build index " + idx);
        SceneManager.LoadScene(idx, LoadSceneMode.Single);
    }

    public void MainMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
    }
}