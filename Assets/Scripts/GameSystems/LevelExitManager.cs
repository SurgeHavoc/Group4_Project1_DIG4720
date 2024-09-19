using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitManager : MonoBehaviour
{
    private static LevelExitManager Instance;

    public GameObject LevelExit;

    private Collider2D LevelExitCollider;

    [SerializeField]
    public List<string> EnemyTypesInLevel;

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

    // LevelExit starts inactive at first.
    private void Start()
    {
        LevelExit.SetActive(false);
        LevelExitCollider = LevelExit.GetComponent<Collider2D>();
    }

    public void EnemyDefeated(string EnemyType)
    {
        ProgressManager.Instance.EnemyDefeated(EnemyType);
        HUDUIManager.Instance.UpdateEnemyCounter(EnemyType, ProgressManager.Instance.DefeatedEnemyCounts[EnemyType]);
        if(ProgressManager.Instance.HasDefeatedRequiredEnemies(EnemyTypesInLevel))
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
        ProgressManager ProgressManager = FindObjectOfType<ProgressManager>();
        ProgressManager.ResetProgress();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
