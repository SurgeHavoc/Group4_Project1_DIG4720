using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector2 LastCheckpointPosition;
    private int PlayerHealthAtCheckpoint;

    private void Start()
    {
        // Upon start, set the initial checkpoint to the player's starting position.
        LastCheckpointPosition = FindObjectOfType<PlayerMovement>().transform.position;
    }

    // A method to save the checkpoint.
    public void SaveCheckpoint(Vector2 position, int health)
    {
        LastCheckpointPosition = position;
        PlayerHealthAtCheckpoint = health;
        Debug.Log("Checkpoint saved at position: " + LastCheckpointPosition);
    }

    public void RespawnAtCheckpoint(GameObject player)
    {
        player.transform.position = LastCheckpointPosition;
        PlayerHealth PlayerHealth = player.GetComponent<PlayerHealth>();
        if(PlayerHealth != null)
        {
            PlayerHealth.SetHealth(PlayerHealthAtCheckpoint);
        }
        Debug.Log("Player respawned at checkpoint.");
    }
}
