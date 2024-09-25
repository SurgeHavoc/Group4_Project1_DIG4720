using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 3;
    private int CurrentHealth;
    public int DeathCount = 0;
    public int MaxDeaths = 4;
    public float InvincibilityDuration = 1.5f; // Temporary invincibility upon taking damage.
    private bool IsInvincible = false;

    private Animator animator;

    public float KnockbackForce = 20f;
    private Rigidbody2D rb;

    public float FlashInterval = 0.2f;
    private SpriteRenderer SpriteRenderer;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        //animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        Debug.Log("Player health at start: " + CurrentHealth);
        Debug.Log("Death count at start: " + DeathCount);
    }

    public void TakeDamage(Vector2 EnemyPosition)
    {
        if(!IsInvincible)
        {
            CurrentHealth--;
            Debug.Log("Player health: " + CurrentHealth);


            ApplyKnockback(EnemyPosition);

            StartCoroutine(BecomeTemporarilyInvincible());

            if(CurrentHealth <= 0)
            {
                Die();
            }
            else
            {
                //animator.SetTrigger("Hurt");
            }
        }
    }

    private void ApplyKnockback(Vector2 EnemyPosition)
    {
        // Set the knockback direction to be away from the enemy.
        Vector2 KnockbackDirection = (transform.position - (Vector3)EnemyPosition).normalized;


        float HorizontalKnockbackForce = KnockbackForce;
        float VerticalKnockbackForce = KnockbackForce * 0.5f;

        // Apply the knockback force to the player's Rigidbody2D.
        //rb.AddForce(KnockbackDirection * KnockbackForce, ForceMode2D.Impulse);
        rb.velocity = new Vector2(KnockbackDirection.x * HorizontalKnockbackForce, VerticalKnockbackForce);
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        IsInvincible = true;
        float ElapsedTime = 0f;

        // Flashing effect with transparency occurs during invincibility.
        while(ElapsedTime < InvincibilityDuration)
        {
            // Alternates between semi-transparent and fully visible.
            Color color = SpriteRenderer.color;
            color.a = (color.a == 1f) ? 0.5f : 1f;
            SpriteRenderer.color = color;

            yield return new WaitForSeconds(FlashInterval);

            ElapsedTime += FlashInterval;
        }
        Color resetColor = SpriteRenderer.color;
        resetColor.a = 1f;
        SpriteRenderer.color = resetColor;

        IsInvincible = false;

        // Alternate flashing effect with invincibility.
        /*
         * Alternate Version
         * ---------------------
         * 
         * IsInvincible = true;
         * float ElapsedTime = 0f;
         * 
         * while(ElapsedTime < InvincibilityDuration)
         * {
         *      SpriteRenderer.enabled = !SpriteRenderer.enabled;
         *      yield return new WaitForSeconds(FlashInterval);
         *      ElapsedTime += FlashInterval;
         * }
         * SpriteRenderer.enabled = true;
         * 
         * IsInvincible = false;
         */
    }

    public void SetHealth(int health)
    {
        CurrentHealth = health;
        Debug.Log("Player health restored: " + CurrentHealth);
    }

    public int GetHealth()
    {
        return CurrentHealth;
    }

    public void Die()
    {
        DeathCount++;
        if(DeathCount >= MaxDeaths)
        {
            // Include SceneTracker so that the "Game_over" scene can load the player back into the scene they were just at.
            SceneTracker.LastScene = SceneManager.GetActiveScene().name;

            // Game over after the player dies four times.
            Debug.Log("Game Over!");
            SceneManager.LoadScene("Game_over");
        }
        else
        {
            // Player respawns at checkpoint if player has died fewer than four times.
            CheckpointManager CheckpointManager = FindObjectOfType<CheckpointManager>();
            if(CheckpointManager != null)
            {
                CheckpointManager.RespawnAtCheckpoint(gameObject);
            }

            CurrentHealth = MaxHealth;
            Debug.Log("Player respawned. Death count: " + DeathCount + " Current health: " + CurrentHealth);

            LivesUIManager LivesUIManager = FindObjectOfType<LivesUIManager>();

            if(LivesUIManager != null)
            {
                LivesUIManager.UpdateLivesUI();
            }

            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }
}
