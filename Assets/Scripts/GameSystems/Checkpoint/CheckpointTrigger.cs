using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerHealth PlayerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if(PlayerHealth != null)
            {
                CheckpointManager CheckpointManager = FindObjectOfType<CheckpointManager>();
                CheckpointManager.SaveCheckpoint(collision.transform.position, PlayerHealth.GetHealth());
            }
        }
    }
}
