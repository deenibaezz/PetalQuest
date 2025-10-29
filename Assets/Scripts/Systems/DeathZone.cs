using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public string gameOverScene = "GameOver";

    void OnTriggerEnter2D(Collider2D other){
        if (!other.CompareTag("Player")) return;

        PlayerPrefs.SetInt("RetryIndex", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameOverScene);
    }
}