using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 1;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage.");

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth PlayerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if(PlayerHealth != null)
            {
                PlayerHealth.TakeDamage(transform.position);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth PlayerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (PlayerHealth != null)
            {
                // Enemy's position is passed through the method to help calculate knockback.
                PlayerHealth.TakeDamage(transform.position);
            }
        }
    }
}
