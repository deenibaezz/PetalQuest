using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public AudioClip pickupSfx;
    AudioSource src;

    void Awake(){ src = FindObjectOfType<AudioSource>(); }

    void OnTriggerEnter2D(Collider2D other){
        if (!other.CompareTag("Player")) return;
        GameManager.I.SeedsCollectedThisLevel++;
        if (src && pickupSfx) src.PlayOneShot(pickupSfx);
        Destroy(gameObject);
    }
}