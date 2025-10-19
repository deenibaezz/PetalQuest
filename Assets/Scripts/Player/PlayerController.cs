using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;       // optional helper below feet
    public LayerMask groundMask;        // set to your Ground layer in Inspector
    public float groundRadius = 0.15f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;                      // optional if you have animations
    bool grounded;
    bool facingRight = true;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); // ok if null
        // Rigidbody defaults
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
    }

    void Update() {
        // Horizontal input (arrows). Swap to A/D if you want later.
        float x = 0f;
        if (Input.GetKey(KeyCode.RightArrow)) x += 1f;
        if (Input.GetKey(KeyCode.LeftArrow))  x -= 1f;

        // Apply horizontal velocity
        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

        // Flip sprite to face movement
        if (x > 0.01f) facingRight = true;
        else if (x < -0.01f) facingRight = false;
        sr.flipX = !facingRight;

        // Ground check (optional but useful for jump gating)
        if (groundCheck) {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);
        } else {
            grounded = true; // if you haven't set it up yet, allow jumps
        }

        // Jump (Up Arrow)
        if (grounded && Input.GetKeyDown(KeyCode.UpArrow)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Drive animations if you set them up
        if (anim) {
            anim.SetFloat("Speed", Mathf.Abs(x));
            anim.SetBool("IsGrounded", grounded);
        }
    }

    void OnDrawGizmosSelected() {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}