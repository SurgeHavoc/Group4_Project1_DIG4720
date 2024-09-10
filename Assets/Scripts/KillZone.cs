using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(collision.CompareTag("Player"))
            {
                PlayerMovement PlayerMovement = collision.GetComponent<PlayerMovement>();
                if(PlayerMovement != null)
                {
                    PlayerMovement.Die();
                }
            }
        }
    }
}
