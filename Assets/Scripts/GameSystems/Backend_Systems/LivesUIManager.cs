using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUIManager : MonoBehaviour
{
    public GameObject Life1;
    public GameObject Life2;
    public GameObject Life3;

    private PlayerHealth PlayerHealth;

    private void Start()
    {
        PlayerHealth = FindObjectOfType<PlayerHealth>();
        UpdateLivesUI();
    }

    private void Update()
    {
        UpdateLivesUI();
    }

    public void UpdateLivesUI()
    {
        int CurrentLives = PlayerHealth.MaxHealth - PlayerHealth.DeathCount;

        Life1.SetActive(CurrentLives >= 1);
        Life2.SetActive(CurrentLives >= 2);
        Life3.SetActive(CurrentLives >= 3);

        if(PlayerHealth.DeathCount >= 3)
        {
            Life1.SetActive(false);
            Life2.SetActive(false);
            Life3.SetActive(false);
        }
    }
}
