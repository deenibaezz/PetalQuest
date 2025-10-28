using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BugPatrol : MonoBehaviour {
    public float speed = 2f;
    public Transform leftEdge, rightEdge;
    public LayerMask groundMask;     // optional if you later use raycasts
    Animator anim;
    Rigidbody2D rb;
    int dir = 1; // 1 = right, -1 = left

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate(){
        // move
        rb.velocity = new Vector2(dir * speed, 0f);

        // flip at patrol bounds
        if (dir > 0 && transform.position.x >= rightEdge.position.x) dir = -1;
        else if (dir < 0 && transform.position.x <= leftEdge.position.x) dir = 1;

        // visual flip
        var s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (dir > 0 ? -1 : 1); // if default faces left
        transform.localScale = s;

        // drive walk animation (optional)
        if (anim) anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void OnCollisionEnter2D(Collision2D c){
        if (c.collider.CompareTag("Player")){
            // Gentle day-scene consequence: small knockback
            var playerRb = c.collider.attachedRigidbody;
            if (playerRb){
                Vector2 away = (c.collider.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
                playerRb.velocity = new Vector2(away.x * 6f, 6f); // push + hop
            }
            Debug.Log("Ouch! Beetle bumped you.");
        }
    }
}