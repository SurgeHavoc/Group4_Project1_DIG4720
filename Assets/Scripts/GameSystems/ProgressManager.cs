using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    public HashSet<string> DefeatedEnemyTypes = new HashSet<string>();
    public int RequiredUniqueEnemies = 1;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnemyDefeated(string EnemyType)
    {
        DefeatedEnemyTypes.Add(EnemyType);
        Debug.Log("Unique enemies defeated: " + DefeatedEnemyTypes.Count);
    }

    public void ResetProgress()
    {
        DefeatedEnemyTypes.Clear();

        ResetEnemies();
    }

    public void ResetEnemies()
    {
        EnemyStatic[] enemies = FindObjectsOfType<EnemyStatic>();
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
            enemy.GetComponent<Collider2D>().enabled = true;
            enemy.GetComponent<IEnemyBehavior>().IsDefeated = false;

            HeadDetect HeadDetect = enemy.GetComponentInChildren<HeadDetect>();
            if (HeadDetect != null)
            {
                HeadDetect.GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    public bool HasDefeatedRequiredEnemies()
    {
        return DefeatedEnemyTypes.Count >= RequiredUniqueEnemies;
    }
}
