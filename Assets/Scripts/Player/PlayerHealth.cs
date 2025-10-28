using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;
    public int currentHearts;
    public float invincibleTime = 0.6f;
    public float knockbackForce = 8f;

    Rigidbody2D rb;
    bool invincible;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        currentHearts = maxHearts;
    }

    public void TakeDamage(int amount, Vector2 hitFromDir){
        if (invincible) return;

        currentHearts = Mathf.Max(0, currentHearts - amount);

        // knockback (small hop + away)
        rb.velocity = new Vector2(hitFromDir.normalized.x * knockbackForce, knockbackForce);

        if (currentHearts <= 0){
            // simple Game Over: load scene
            SceneManager.LoadScene("GameOver");
            return;
        }

        // brief invincibility frames
        invincible = true;
        Invoke(nameof(EndInvincibility), invincibleTime);
    }

    void EndInvincibility(){ invincible = false; }
}