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

    float timer = 0f;
    bool playerIn = false;

    void Start(){
        UpdatePrompt(); // show initial state (likely "Need X more")
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

        // Not enough seeds -> no entry
        int need = GameManager.I.MinSeedsToWin - GameManager.I.SeedsCollectedThisLevel;
        if (need > 0){
            // still update the prompt while standing here
            UpdatePrompt();
            return;
        }

        // Enough seeds
        if (requireButton){
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)){
                EnterGarden();
            }
        } else {
            timer += Time.deltaTime;
            if (timer >= standTimeToEnter){
                EnterGarden();
            }
        }
    }

    void EnterGarden(){
        // Optional: small SFX or fade here
        SceneManager.LoadScene(gardenSceneName);
    }

    void UpdatePrompt(){
        if (!promptText) return;

        int need = GameManager.I.MinSeedsToWin - GameManager.I.SeedsCollectedThisLevel;
        if (!playerIn){
            // Show static END text when not inside
            promptText.text = "END";
            return;
        }

        if (need > 0){
            promptText.text = $"Need {need} more seeds";
        } else {
            promptText.text = requireButton ? "Press ↑ to enter" : "Entering…";
        }
    }
}
