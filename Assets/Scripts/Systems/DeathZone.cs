using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public string gameOverScene = "GameOver";

    [Header("SFX")]
    public AudioClip sfxFall;
    [Range(0f,1f)] public float volFall = 0.8f;

    void OnTriggerEnter2D(Collider2D other){
        if (!other.CompareTag("Player")) return;

        // remember which level to retry
        PlayerPrefs.SetInt("RetryIndex", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();

        Sfx2D.Play(sfxFall, volFall);  // fall sound
        StartCoroutine(GoGameOverSoon());
    }

    IEnumerator GoGameOverSoon(){
        yield return new WaitForSeconds(0.06f);
        SceneManager.LoadScene(gameOverScene);
    }
}
