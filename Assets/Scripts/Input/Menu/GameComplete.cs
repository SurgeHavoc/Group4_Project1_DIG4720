using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameComplete : MonoBehaviour
{
    public Button MainMenuButton;

    private PlayerInput PlayerInput;
    private InputAction GameCompleteAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        GameCompleteAction = PlayerInput.actions["GameStart"];
    }

    private void OnEnable()
    {
        GameCompleteAction.Enable();
    }

    private void OnDisable()
    {
        GameCompleteAction.Disable();
    }

    private void Update()
    {
        if(GameCompleteAction.WasPressedThisFrame())
        {
            OnGameCompletePressed();
        }
    }

    private void OnGameCompletePressed()
    {
        if(MainMenuButton != null)
        {
            MainMenuButton.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("No main menu button assigned. Loading main menu.");
            LoadMainMenu();
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
