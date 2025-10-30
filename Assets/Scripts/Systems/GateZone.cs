using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GateZone : MonoBehaviour
{
    [Header("Config")]
    public string gardenSceneName = "Garden_Day";
    public bool requireButton = true;        // if false: auto after delay
    public float standTimeToEnter = 0.75f;   // only used if requireButton == false

    [Header("UI (optional)")]
    public TMP_Text promptText;              // world-space TMP above the gate

    [Header("SFX")]
    public AudioClip sfxGateEnter;
    [Range(0f,1f)] public float volGate = 0.6f;

    float timer = 0f;
    bool playerIn = false;

    void Start(){
        UpdatePrompt();
    }

    void OnTriggerEnter2D(Collider2D other){
        if (!other.CompareTag("Player")) return;
        playerIn = true;
        timer = 0f;
        UpdatePrompt();
    }

    void OnTriggerExit2D(Collider2D other){
        if (!other.CompareTag("Player")) return;
        playerIn = false;
        timer = 0f;
        UpdatePrompt();
    }

    void Update(){
        if (!playerIn) return;

        int need = GameManager.I.MinSeedsToWin - GameManager.I.SeedsCollectedThisLevel;
        if (need > 0){ UpdatePrompt(); return; }

        if (requireButton){
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)){
                StartCoroutine(EnterGardenWithSfx());
            }
        } else {
            timer += Time.deltaTime;
            if (timer >= standTimeToEnter){
                StartCoroutine(EnterGardenWithSfx());
            }
        }
    }

    IEnumerator EnterGardenWithSfx(){
        Sfx2D.Play(sfxGateEnter, volGate);     // play gate-enter
        yield return new WaitForSeconds(0.06f); // let the attack be heard
        SceneManager.LoadScene(gardenSceneName);
    }

    void UpdatePrompt(){
        if (!promptText) return;

        int need = GameManager.I.MinSeedsToWin - GameManager.I.SeedsCollectedThisLevel;
        if (!playerIn){ promptText.text = "END"; return; }

        if (need > 0){
            promptText.text = $"Need {need} more seeds";
        } else {
            promptText.text = requireButton ? "Press ↑ to enter" : "Entering…";
        }
    }
}
