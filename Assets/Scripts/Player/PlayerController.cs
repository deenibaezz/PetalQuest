using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundRadius = 0.15f;
    public float coyoteTime = 0.12f, jumpBuffer = 0.12f;

    Rigidbody2D rb;
    SpriteRenderer sr;          // NEW: flip left/right
    Animator anim;              // NEW: drive Idle/Walk
    float coyoteCounter, jumpBufferCounter;
    bool grounded;
    bool facingRight = true;    // NEW: remember last facing for idle

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();   // NEW
        anim = GetComponent<Animator>();       // NEW
    }

    void Update(){
        // horizontal movement
        float x = Input.GetAxisRaw("Horizontal"); // arrow keys or A/D
        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

        // flip logic (so idle faces last direction too)
        if (x > 0.01f) facingRight = true;
        else if (x < -0.01f) facingRight = false;
        if (sr) sr.flipX = !facingRight;       // NEW

        // ground check
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);
        coyoteCounter = grounded ? coyoteTime : Mathf.Max(0, coyoteCounter - Time.deltaTime);

        // jump buffer (when you press ↑)
        if (Input.GetKeyDown(KeyCode.UpArrow)) jumpBufferCounter = jumpBuffer;
        else jumpBufferCounter = Mathf.Max(0, jumpBufferCounter - Time.deltaTime);

        // jump logic
        if (coyoteCounter > 0 && jumpBufferCounter > 0){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            coyoteCounter = 0;
            jumpBufferCounter = 0;
        }

        // NEW: drive Animator param so Idle<->Walk works
        if (anim) anim.SetFloat("Speed", Mathf.Abs(x));
        // (Optional later) anim.SetBool("IsGrounded", grounded);
    }

    void OnDrawGizmosSelected(){
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}