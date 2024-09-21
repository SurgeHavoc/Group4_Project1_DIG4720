using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneLoad : MonoBehaviour
{
    [SerializeField]
    // Name of the scene to destroy gameObject when the scene is loaded.
    public string DestroyOnThisSceneName = "Main_Menu";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        // Register a callback to be triggered once a scene has loaded.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unregister the callback here.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == DestroyOnThisSceneName)
        {
            Destroy(gameObject);
        }
    }
}
