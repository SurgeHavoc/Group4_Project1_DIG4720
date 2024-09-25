using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenuUI; // Assign the Pause Menu panel to this.
    public Button DefaultSelectedButton;

    private PlayerControls controls;

    public AudioSource AudioSource;
    public AudioClip SelectSound;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.PlayerMovement.Pause.performed += _ => HandlePauseInput();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        controls.PlayerMovement.Pause.performed -= _ => HandlePauseInput();
        controls.Disable();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void HandlePauseInput()
    {
        if(IsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        AudioSource.PlayOneShot(SelectSound);
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;

        FindObjectOfType<PlayerMovement>().InhibitInput = 0.5f;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;

        EventSystem.current.SetSelectedGameObject(DefaultSelectedButton.gameObject);
    }

    public void LoadMainMenu()
    {
        AudioSource.PlayOneShot(SelectSound);
        Time.timeScale = 1f;
        IsPaused = false;
        PauseMenuUI.SetActive(false);
        SceneManager.LoadScene("Main_Menu");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure that the paused state is disabled by default on scene load.
        IsPaused = false;
        Time.timeScale = 1f;
        PauseMenuUI.SetActive(false);
    }
}
