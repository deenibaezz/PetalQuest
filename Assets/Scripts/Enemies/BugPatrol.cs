using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BugPatrol : MonoBehaviour
{
    [Header("Patrol")]
    public float speed = 2f;
    public Transform leftEdge, rightEdge;

    [Header("Stomp Detection")]
    public float stompBounce = 12f;          // how high the player bounces on stomp
    public float topHitYAllowance = 0.15f;   // extra leniency for "from above"

    [Header("SFX")]
    public AudioClip sfxSquish;              // play when stomped
    [Range(0f,1f)] public float volSquish = 0.7f;
    public AudioClip sfxOuch;                // play when bug hurts player
    [Range(0f,1f)] public float volOuch = 0.6f;

    Animator anim;
    Rigidbody2D rb;
    int dir = 1; // 1 = right, -1 = left

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate(){
        // move (keep bug pinned to ground, so y-vel = 0)
        rb.velocity = new Vector2(dir * speed, 0f);

        // flip at patrol bounds
        if (dir > 0 && transform.position.x >= rightEdge.position.x) dir = -1;
        else if (dir < 0 && transform.position.x <= leftEdge.position.x) dir = 1;

        // visual flip (your sprite default faces left)
        var s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (dir > 0 ? -1 : 1);
        transform.localScale = s;

        // drive walk anim
        if (anim) anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void OnCollisionEnter2D(Collision2D c){
        if (!c.collider.CompareTag("Player")) return;

        var playerRb = c.collider.attachedRigidbody;
        if (playerRb == null) return;

        // --- determine if the hit is a stomp (from above) ---
        bool stomp = false;

        // Use contact normals first (robust)
        foreach (var contact in c.contacts){
            if (contact.normal.y < -0.4f){ stomp = true; break; }
        }
        // Fallback: compare centers (player noticeably higher)
        if (!stomp){
            if (c.collider.bounds.center.y > transform.position.y + topHitYAllowance)
                stomp = true;
        }

        if (stomp){
            // Bounce player up
            playerRb.velocity = new Vector2(playerRb.velocity.x, stompBounce);

            // Play squish via a temporary 2D AudioSource that survives this object's destroy
            Play2D(sfxSquish, volSquish);

            Debug.Log("SQUISH! (stomp branch reached)");

            // Optionally disable visuals/colliders first so it "dies" instantly
            DisableSelfVisualsAndCollision();

            // Destroy beetle immediately (sound continues from temp source)
            Destroy(gameObject);
        }
        else{
            // Side hit → damage player
            var ph = c.collider.GetComponent<PlayerHealth>();
            if (ph != null){
                Vector2 away = (c.collider.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
                Play2D(sfxOuch, volOuch);
                ph.TakeDamage(1, away);
            }
        }
    }

    // Spawns a one-shot 2D AudioSource that auto-destroys after clip length
    void Play2D(AudioClip clip, float vol){
        if (!clip) return;
        var go = new GameObject("SFX2D_temp");
        var a = go.AddComponent<AudioSource>();
        a.playOnAwake = false;
        a.spatialBlend = 0f;     // 2D
        a.volume = 1f;
        a.clip = clip;
        a.Play();
        Destroy(go, clip.length);
    }

    void DisableSelfVisualsAndCollision(){
        // stop moving
        if (rb) { rb.velocity = Vector2.zero; rb.simulated = false; }
        // hide sprite(s)
        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.enabled = false;
        foreach (var childSr in GetComponentsInChildren<SpriteRenderer>()){
            childSr.enabled = false;
        }
        // disable colliders
        foreach (var col in GetComponentsInChildren<Collider2D>()){
            col.enabled = false;
        }
        // optionally stop animator
        if (anim) anim.enabled = false;
    }
}