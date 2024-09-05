using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    [HideInInspector] public PlayerControls controls;
    [HideInInspector] public Vector2 MoveInput;
    [HideInInspector] public bool JumpInput;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        controls = new PlayerControls();

        controls.PlayerMovement.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();

        controls.PlayerMovement.Jump.performed += ctx => JumpInput = ctx.ReadValue<float>() > 0;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
