using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIInputManager : MonoBehaviour
{
    public static UIInputManager Instance;

    public Button PlayButton;

    private PlayerInput PlayerInput;
    private InputAction GameStartAction;

    public AudioClip RetroSelectSound;
    public AudioSource AudioSource;

    private void Awake()
    {
        // A singleton pattern setup.
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        PlayerInput = GetComponent<PlayerInput>();
        AudioSource = GetComponent<AudioSource>();

        GameStartAction = PlayerInput.actions["GameStart"];
    }

    private void OnEnable()
    {
        GameStartAction.Enable();
    }
    private void OnDisable()
    {
        GameStartAction.Disable();
    }

    private void Update()
    {
        // A check for mouse left click or gamepad Start for the "GameStart" action.
        if(GameStartAction.WasPressedThisFrame())
        {
            OnGameStartPressed();
        }
    }

    private void OnGameStartPressed()
    {
        if(PlayButton != null)
        {
            AudioSource.PlayOneShot(RetroSelectSound);

            PlayButton.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("The Play Button is not assigned in the Inspector.");
        }
    }
}
