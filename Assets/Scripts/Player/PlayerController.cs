using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundRadius = 0.15f;
    public float coyoteTime = 0.12f, jumpBuffer = 0.12f;
    public float climbSpeed = 4f;
    float defaultGravity;
    bool onLadder;

    public SpriteRenderer sr;     // for flip
    Animator anim;                // NEW

    Rigidbody2D rb;
    float coyoteCounter, jumpBufferCounter;
    bool grounded;
    bool facingRight = true;

    void Awake(){
        defaultGravity = GetComponent<Rigidbody2D>().gravityScale;
        rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();     // NEW
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