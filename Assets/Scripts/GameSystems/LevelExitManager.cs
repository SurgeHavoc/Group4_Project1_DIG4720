using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitManager : MonoBehaviour
{
    public GameObject LevelExit;

    private HashSet<string> DefeatedEnemyTypes = new HashSet<string>();
    private int RequiredUniqueEnemies = 1;

    private Collider2D levelExitCollider;
    // LevelExit starts inactive at first.
    private void Start()
    {
        LevelExit.SetActive(false);

        levelExitCollider = LevelExit.GetComponent<Collider2D>();
    }

    public void EnemyDefeated(string EnemyType)
    {
        DefeatedEnemyTypes.Add(EnemyType);

        if(DefeatedEnemyTypes.Count >= RequiredUniqueEnemies)
        {
            ShowLevelExit();
        }
    }

    private void ShowLevelExit()
    {
        LevelExit.SetActive(true);
    }

    public void PlayerReachedLevelExit()
    {
        Debug.Log("Level exit reached.");
        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        /*
        int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int NextSceneIndex = CurrentSceneIndex + 1;

        if(NextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(NextSceneIndex);
        }
        else
        {
            Debug.Log("Last scene in sequence reached.");
        }*/
    }
}
