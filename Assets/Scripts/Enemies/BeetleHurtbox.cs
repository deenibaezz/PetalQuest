using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BeetleHurtbox : MonoBehaviour
{
    public float stompBounce = 12f;
    Rigidbody2D beetleRb;

    void Awake(){
        beetleRb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D c){
        if (!c.collider.CompareTag("Player")) return;

        // get references
        var player = c.collider.GetComponent<PlayerHealth>();
        var playerRb = c.collider.GetComponent<Rigidbody2D>();
        if (!player || !playerRb) return;

        // figure out contact: above or side?
        // Use the first contact normal and positions as a robust check
        ContactPoint2D contact = c.GetContact(0);
        Vector2 normal = contact.normal; // normal points from beetle -> player

        bool playerComingDown = playerRb.velocity.y <= -0.05f;
        bool playerCenterIsAbove = c.collider.bounds.center.y > transform.bounds().center.y + 0.05f;

        // If contact normal mostly points DOWN (player landing on beetle) OR player above & falling
        bool stomp = (normal.y < -0.5f) || (playerComingDown && playerCenterIsAbove);

        if (stomp){
            // bounce player up
            playerRb.velocity = new Vector2(playerRb.velocity.x, stompBounce);
            // destroy beetle
            AudioManager.I.Play("bug_squish");
            Destroy(gameObject);
        } else {
            // damage player & knock them away from beetle
            Vector2 away = (c.collider.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
            AudioManager.I.Play("player_ouch", 1f);
            player.TakeDamage(1, away);
        }
    }
}

// handy bounds extension so we can use transform.bounds()
static class BoundsExt {
    public static Bounds bounds(this Transform t){
        var r = t.GetComponent<Renderer>();
        if (r) return r.bounds;
        var c = t.GetComponent<Collider2D>();
        if (c) return c.bounds;
        return new Bounds(t.position, Vector3.zero);
    }
}