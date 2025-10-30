using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlowerSpot : MonoBehaviour
{
    public GameObject flowerPrefab;
    public GameObject promptUI;   // assign your TMP text object here
    bool grown = false;
    bool playerIn = false;

    [Header("SFX")]
    public AudioClip sfxLadderGrow;
    [Range(0f,1f)] public float volLadder = 0.6f;

    void Start(){
        if (promptUI) promptUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        playerIn = true;
        UpdatePrompt();
    }

    void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        playerIn = false;
        UpdatePrompt();
    }

    void Update() {
        UpdatePrompt(); // show/hide prompt dynamically

        if (!playerIn || grown) return;

        bool pressed = Input.GetKeyDown(KeyCode.Space);
        if (GameManager.I.HasWateringCan && pressed) {
            Instantiate(flowerPrefab, transform.position, Quaternion.identity);
            Sfx2D.Play(sfxLadderGrow, volLadder);   // <— play watering/ladder sound
            grown = true;
            UpdatePrompt(); // hide prompt after blooming
            Debug.Log("Flowers bloomed!");
        }
    }

    void UpdatePrompt(){
        if (!promptUI) return;
        bool show = playerIn && !grown && GameManager.I.HasWateringCan;
        if (promptUI.activeSelf != show) promptUI.SetActive(show);
    }
}
