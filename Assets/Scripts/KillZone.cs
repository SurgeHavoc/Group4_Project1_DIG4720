using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerHealth PlayerHealth = collision.GetComponent<PlayerHealth>();
            PlayerHealth.Die();
        }
    }
}
