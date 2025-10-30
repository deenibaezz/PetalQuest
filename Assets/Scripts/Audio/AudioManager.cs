using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SfxEntry {
    public string key;          // e.g., "bug_squish"
    public AudioClip clip;
    [Range(0f,1f)] public float volume = 0.8f;
    [Range(0.5f,2f)] public float pitch = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager I { get; private set; }

    [Header("SFX Library")]
    public List<SfxEntry> sfx = new List<SfxEntry>();

    AudioSource oneShot;
    Dictionary<string, SfxEntry> map;
    bool muted;

    void Awake(){
        if (I != null && I != this){ Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        oneShot = gameObject.AddComponent<AudioSource>();
        oneShot.playOnAwake = false;

        map = new Dictionary<string, SfxEntry>();
        foreach (var e in sfx){
            if (!string.IsNullOrEmpty(e.key) && e.clip) map[e.key] = e;
        }

        muted = PlayerPrefs.GetInt("Muted", 0) == 1;
        ApplyMute();
    }

    public void Play(string key, float volMul = 1f, float pitchMul = 1f){
        if (muted) return;
        if (!map.TryGetValue(key, out var e) || e.clip == null) return;

        oneShot.pitch = e.pitch * pitchMul;
        oneShot.PlayOneShot(e.clip, e.volume * volMul);
    }

    public void SetMuted(bool m){
        muted = m;
        PlayerPrefs.SetInt("Muted", muted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyMute();
    }

    public bool IsMuted() => muted;

    void ApplyMute(){
        // simplest global mute
        AudioListener.volume = muted ? 0f : 1f;
    }
}