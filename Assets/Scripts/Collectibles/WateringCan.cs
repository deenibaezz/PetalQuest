using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        GameManager.I.HasWateringCan = true;
        Destroy(gameObject);
        Debug.Log("Collected Watering Can!");
    }
}