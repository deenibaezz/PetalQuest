using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FootstepPlayer : MonoBehaviour
{
    [Header("Ground Check (same as PlayerController)")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundRadius = 0.15f;

    [Header("Clips (use short 0.1–0.25s taps if possible)")]
    public AudioClip[] footstepClips;

    [Header("Timing")]
    public float baseInterval = 0.35f;   // time between steps at slow walk
    public float speedScale = 0.18f;     // faster run -> shorter interval
    public float minInterval = 0.12f;
    public float minSpeedForSteps = 0.2f;

    [Header("Mix / Feel")]
    [Range(0f,1f)] public float volume = 0.25f;      // quieter than other SFX
    public Vector2 pitchRange = new Vector2(0.96f, 1.06f);

    Rigidbody2D rb;
    AudioSource stepSrc;   // dedicated source for footsteps only
    float timer;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();

        // make/find a dedicated footstep source so we don't fight with jump/coins
        stepSrc = gameObject.AddComponent<AudioSource>();
        stepSrc.playOnAwake = false;
        stepSrc.loop = false;           // single step only
        stepSrc.spatialBlend = 0f;      // 2D
        stepSrc.priority = 200;         // low priority so other SFX win
        stepSrc.dopplerLevel = 0f;
        stepSrc.volume = 1f;            // we set per-step volume below
    }

    void Update(){
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);

        // also require very small vertical motion so steps stop during jump/fall
        bool verticalStill = Mathf.Abs(rb.velocity.y) < 0.05f;
        float speedX = Mathf.Abs(rb.velocity.x);
        bool shouldStep = grounded && verticalStill && speedX >= minSpeedForSteps;

        if (!shouldStep){
            if (stepSrc.isPlaying) stepSrc.Stop(); // cut long clips instantly
            timer = 0.02f;
            return;
        }

        // cadence speeds up with run speed
        timer -= Time.deltaTime;
        float interval = baseInterval / (1f + speedX * speedScale);
        interval = Mathf.Max(minInterval, interval);

        // don’t start a new step if one is still playing (avoids overlap)
        if (timer <= 0f && !stepSrc.isPlaying){
            PlayStep();
            timer = interval;
        }
    }

    void PlayStep(){
        if (footstepClips == null || footstepClips.Length == 0) return;
        var clip = footstepClips[Random.Range(0, footstepClips.Length)];
        if (!clip) return;

        stepSrc.pitch = Random.Range(pitchRange.x, pitchRange.y);
        stepSrc.clip  = clip;
        stepSrc.volume = volume;
        stepSrc.Play(); // use Play so Stop() can actually cut it off
    }

    void OnDrawGizmosSelected(){
        if (!groundCheck) return;
        Gizmos.color = new Color(0.4f, 1f, 0.4f, 0.5f);
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
