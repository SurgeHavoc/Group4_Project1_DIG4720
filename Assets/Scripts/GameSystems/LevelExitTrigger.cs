using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitTriggerDetect : MonoBehaviour
{
    private LevelExitManager LevelExitManager;

    private void Start()
    {
        LevelExitManager = FindObjectOfType<LevelExitManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && LevelExitManager != null)
        {
            LevelExitManager.PlayerReachedLevelExit();
        }
    }
}
