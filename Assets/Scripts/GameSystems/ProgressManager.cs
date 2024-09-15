using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    public Dictionary<string, int> DefeatedEnemyCounts = new();
    public int EnemiesPerType = 3;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnemyDefeated(string EnemyType)
    {
        if(DefeatedEnemyCounts.ContainsKey(EnemyType))
        {
            DefeatedEnemyCounts[EnemyType]++;
        }
        else
        {
            DefeatedEnemyCounts[EnemyType] = 1;
        }
        Debug.Log(EnemyType + " enemies defeated: " + DefeatedEnemyCounts[EnemyType]);
    }

    public void ResetProgress()
    {
        DefeatedEnemyCounts.Clear();

        Debug.Log("Resetting defeated enemy counts.");

        Debug.Log("Watermelon enemies defeated: " + (DefeatedEnemyCounts.ContainsKey("Watermelon") ? DefeatedEnemyCounts["Watermelon"] : 0));
        Debug.Log("Grape enemies defeated: " + (DefeatedEnemyCounts.ContainsKey("Grape") ? DefeatedEnemyCounts["Grape"] : 0));
        Debug.Log("Banana enemies defeated: " + (DefeatedEnemyCounts.ContainsKey("Banana") ? DefeatedEnemyCounts["Banana"] : 0));

        ResetEnemies();
        HUDUIManager.Instance.ResetAllCounters();
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
        foreach(var count in DefeatedEnemyCounts.Values)
        {
            if(count < EnemiesPerType)
            {
                return false;
            }
        }
        return true;
    }
}
