using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip pickupSfx;
    [Range(0f,1f)] public float volPickup = 0.5f;

    void OnTriggerEnter2D(Collider2D other){
        if (!other.CompareTag("Player")) return;

        // count seed
        GameManager.I.SeedsCollectedThisLevel++;

        // play independent 2D one-shot that survives this object's destroy
        Play2D(pickupSfx, volPickup);

        // remove the seed from the scene
        Destroy(gameObject);
    }

    void Play2D(AudioClip clip, float vol){
        if (!clip) return;
        var go = new GameObject("SeedSFX_" + clip.name);
        var a = go.AddComponent<AudioSource>();
        a.playOnAwake = false;
        a.spatialBlend = 0f; // 2D
        a.volume = vol;
        a.clip = clip;
        a.Play();
        Destroy(go, clip.length);
    }
}
