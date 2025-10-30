using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sfx2D
{
    // Plays a 2D one-shot that survives scene loads; auto-destroys.
    public static void Play(AudioClip clip, float volume = 1f, float pitch = 1f){
        if (!clip) return;
        var go = new GameObject("SFX2D_" + clip.name);
        Object.DontDestroyOnLoad(go);
        var a = go.AddComponent<AudioSource>();
        a.playOnAwake = false;
        a.spatialBlend = 0f; // 2D
        a.volume = Mathf.Clamp01(volume);
        a.pitch = Mathf.Clamp(pitch, 0.1f, 3f);
        a.clip = clip;
        a.Play();
        Object.Destroy(go, clip.length / Mathf.Max(0.01f, a.pitch));
    }
}