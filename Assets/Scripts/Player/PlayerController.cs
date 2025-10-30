using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour
{
    [Header("Move/Jump")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundRadius = 0.15f;
    public float coyoteTime = 0.12f, jumpBuffer = 0.12f;

    [Header("Climb")]
    public float climbSpeed = 4f;

    [Header("SFX")]
    public AudioClip sfxJump;
    [Range(0f,1f)] public float volJump = 0.5f;

    float defaultGravity;
    bool onLadder;

    public SpriteRenderer sr;     // for flip
    Animator anim;                // NEW

    Rigidbody2D rb;
    AudioSource audioSrc;
    float coyoteCounter, jumpBufferCounter;
    bool grounded;
    bool facingRight = true;

    void Awake(){
        defaultGravity = GetComponent<Rigidbody2D>().gravityScale;
        rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();     // NEW

        audioSrc = GetComponent<AudioSource>();
        audioSrc.playOnAwake = false;
        audioSrc.spatialBlend = 0f;          // 2D
        audioSrc.volume = 1f;   
    }

    void Update(){
        float x = Input.GetAxisRaw("Horizontal");

        // movement
        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

        // flip
        if (x > 0.01f) facingRight = true;
        else if (x < -0.01f) facingRight = false;
        if (sr) sr.flipX = !facingRight;

        if (onLadder) {
            float v = 0f;
            if (Input.GetKey(KeyCode.UpArrow)) v = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) v = -1f;
            
            rb.gravityScale = 0f; // no falling while on ladder
            rb.velocity = new Vector2(rb.velocity.x, v * climbSpeed);

            // optional: jump off the ladder
            if (Input.GetKeyDown(KeyCode.Space)) {
                onLadder = false;
                rb.gravityScale = defaultGravity;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (sfxJump) audioSrc.PlayOneShot(sfxJump, volJump);
            }
        } else {
            rb.gravityScale = defaultGravity;
        }

        // ground + jump
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);
        coyoteCounter = grounded ? coyoteTime : Mathf.Max(0, coyoteCounter - Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow)) jumpBufferCounter = jumpBuffer;
        else jumpBufferCounter = Mathf.Max(0, jumpBufferCounter - Time.deltaTime);

        if (coyoteCounter > 0 && jumpBufferCounter > 0){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            if (sfxJump) audioSrc.PlayOneShot(sfxJump, volJump);
            coyoteCounter = 0; jumpBufferCounter = 0;
        }

        // NEW: drive animations
        if (anim){
            anim.SetFloat("Speed", Mathf.Abs(x));  // 0 = idle, >0.1 = walk
            // later you can also set: anim.SetBool("IsGrounded", grounded);
        }
    }

    void OnDrawGizmosSelected(){
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
    void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Climbable")) onLadder = true;
    }
    void OnTriggerExit2D(Collider2D other) {
    if (other.CompareTag("Climbable")) onLadder = false;
    }
}