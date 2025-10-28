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
        // Show/hide prompt dynamically
        UpdatePrompt();

        if (!playerIn || grown) return;
        if (GameManager.I.HasWateringCan && Input.GetKeyDown(KeyCode.Space)) {
            Instantiate(flowerPrefab, transform.position, Quaternion.identity);
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