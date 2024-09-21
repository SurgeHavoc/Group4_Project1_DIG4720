using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Button RestartButton;

    private PlayerInput PlayerInput;
    private InputAction GameRestartAction;

    private string LastScene;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        GameRestartAction = PlayerInput.actions["GameStart"];

        LastScene = SceneTracker.LastScene;
    }

    private void OnEnable()
    {
        GameRestartAction.Enable();
    }

    private void OnDisable()
    {
        GameRestartAction.Disable();
    }

    private void Update()
    {
        if(GameRestartAction.WasPressedThisFrame())
        {
            OnGameRestartPressed();
        }
    }

    private void OnGameRestartPressed()
    {
        if(RestartButton != null)
        {
            RestartButton.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("No Restart Button assigned. Reloading the last scene directly.");
            ReloadLastScene();
        }
    }

    public void ReloadLastScene()
    {
        ProgressManager.Instance.ResetProgress();

        SceneManager.LoadScene(LastScene);
    }
}
