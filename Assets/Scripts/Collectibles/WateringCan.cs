using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WateringCan : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip sfxPickupBucket;
    [Range(0f,1f)] public float volPickupBucket = 0.6f;

    Collider2D col;
    SpriteRenderer sr;

    void Awake(){
        col = GetComponent<Collider2D>();
        sr  = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if (!other.CompareTag("Player")) return;

        GameManager.I.HasWateringCan = true;

        // play sound that survives scene loads
        Sfx2D.Play(sfxPickupBucket, volPickupBucket);

        // hide & remove
        if (col) col.enabled = false;
        if (sr)  sr.enabled = false;
        StartCoroutine(DestroySoon());
    }

    IEnumerator DestroySoon(){
        yield return null; // next frame is enough
        Destroy(gameObject);
    }
}