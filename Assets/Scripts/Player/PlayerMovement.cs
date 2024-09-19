using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerMovement : MonoBehaviour
{
    private Vector2 MoveInput;// References Player input for movement.
    public float MoveSpeed = 6f;
    public float JumpForce = 10f;

    public float WallSlideSpeed = 3f;

    private float WallJumpingDirection;
    private float WallJumpingTime = 0.2f;
    private float WallJumpingCounter;
    private float WallJumpingDuration = 0.2f;
    private float WallJumpCooldown = 0.6f; // A cooldown for jumping.
    private float WallJumpCooldownTimer = 0f; // A timer to track the cooldown.
    private Vector2 WallJumpingPower = new(6f, 8f);

    private bool IsWallSliding;
    private bool IsWallJumping;
    private bool IsFacingRight = true;
    private bool IsJumping = false;
    private bool IsWalking;
    private bool CanCancelWallJump = true;

    public float WallJumpCancelBuffer = 2.5f;

    [SerializeField] private Rigidbody2D rb; // References Player.
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private Transform WallCheck;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private LayerMask WallLayer;

    private PlayerControls controls;
    private Animator animator;
    private SpriteRenderer SpriteRenderer;

    private void Awake()
    {
        controls = new PlayerControls();

        // Bind the movement and jump input.
        controls.PlayerMovement.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.PlayerMovement.Move.canceled += ctx => MoveInput = Vector2.zero;
        controls.PlayerMovement.Jump.performed += ctx => IsJumping = true;
        controls.PlayerMovement.Jump.canceled += ctx => IsJumping = false;

        // Bind dev controls for instantly quitting game and fast forwarding to next level.
        controls.PlayerMovement.QuitGame.performed += ctx => QuitGame();
        controls.PlayerMovement.LoadNextLevel.performed += ctx => LoadNextLevel();

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleWallJumpAndWallSlide();

        UpdateAnimations();
    }

    private void HandleWallJumpAndWallSlide()
    {
        if (WallJumpCooldownTimer > 0f)
        {
            WallJumpCooldownTimer -= Time.deltaTime;
        }

        if (IsJumping && IsGrounded() && WallJumpCooldownTimer <= 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            IsJumping = false;
            WallJumpCooldownTimer = WallJumpCooldown;
        }

        WallSlide();
        WallJump();

        if (!IsWallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if(!IsWallJumping)
        {
            rb.velocity = new Vector2(MoveInput.x * MoveSpeed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.1f, GroundLayer);
    }

    private bool IsTouchingWall()
    {
        return Physics2D.OverlapCircle(WallCheck.position, 0.1f, WallLayer);
    }

    private void WallSlide()
    {
        if (IsTouchingWall() && !IsGrounded())
        {
            IsWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -WallSlideSpeed, float.MaxValue));
        }
        else
        {
            IsWallSliding = false;
        }
    }

    private void WallJump()
    {
        // Can wall jump when wall sliding and when within the allowed wall jump limit.
        if(IsWallSliding)
        {
            IsWallJumping = false;
            WallJumpingDirection = -transform.localScale.x;
            WallJumpingCounter = WallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
            CanCancelWallJump = false;
            Invoke(nameof(EnableWallJumpCancel), WallJumpCancelBuffer); // Enable wall jump canceling after the delay.
        }
        else
        {
            WallJumpingCounter -= Time.deltaTime;
        }

        if(IsJumping && WallJumpingCounter > 0f && WallJumpCooldownTimer <= 0f)
        {
            IsWallJumping = true;
            rb.velocity = new Vector2(WallJumpingDirection * WallJumpingPower.x, WallJumpingPower.y);
            WallJumpingCounter = 0f;

            // Flip the player's direction after making a wall jump.
            if(transform.localScale.x != WallJumpingDirection)
            {
                IsFacingRight = !IsFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            WallJumpCooldownTimer = WallJumpCooldown;

            InvokeRepeating(nameof(ExtendWallJumping), WallJumpingDuration, 1f);
        }

        // After a delay, cancel if horizontal input is detected during the jump.
        if(IsWallJumping && CanCancelWallJump && MoveInput.x != 0f)
        {
            CancelWallJump();
        }
    }

    // Allows the player to extend the duration of the wall jump, allowing for longer wall jumps.
    private void ExtendWallJumping()
    {
        if (IsJumping && WallJumpingDuration < 1f)
        {
            WallJumpingDuration += 0.1f;
        }
        else
        {
            StopWallJumping();
        }
    }

    private void StopWallJumping()
    {
        IsWallJumping = false;

        // Smooth transition out of wall jumping by maintaining downward momentum.
        if(rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.9f);
        }

        WallJumpingDuration = 0.2f;
        CancelInvoke(nameof(ExtendWallJumping));
    }

    private void CancelWallJump()
    {
        IsWallJumping = false;
        StopWallJumping();
    }

    private void EnableWallJumpCancel()
    {
        CanCancelWallJump = true;
    }

    void Flip()
    {
        if(IsFacingRight && MoveInput.x < 0f || !IsFacingRight && MoveInput.x > 0f)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Die()
    {
        Debug.Log("Player died!");

        if(ProgressManager.Instance != null)
        {
            ProgressManager.Instance.ResetProgress();
        }
        SceneTracker.LastScene = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene("Game_over");
    }

    private void UpdateAnimations()
    {
        if(MoveInput.x != 0)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    /* Dev controls
     * "E" to load next level.
     * 
     * "Esc" to escape game.
     */

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
