using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float AttackRange = 1f;
    public int AttackDamage = 1;
    public Transform AttackPoint;
    public LayerMask EnemyLayers;
    public float AttackCooldown = 1.0f;
    private float LastAttackTime = 0f;

    private PlayerControls controls;
    private Animator animator;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.PlayerMovement.Attack.performed += ctx => Attack();

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

    private void Attack()
    {
        if(Time.time >= LastAttackTime + AttackCooldown)
        {
            animator.SetTrigger("AttackTrigger");

            Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

            foreach (Collider2D enemy in HitEnemies)
            {
                Debug.Log("Hit " + enemy.name);

                HeadDetect HeadDetect = enemy.GetComponentInChildren<HeadDetect>();

                HeadDetect.TriggerDefeatByAttack();
                enemy.GetComponent<EnemyController>().TakeDamage(AttackDamage);

                // Notify ProgressManager about the enemy's defeat.
                string EnemyType = enemy.tag;
                ProgressManager.Instance.EnemyDefeated(EnemyType);
            }

            LastAttackTime = Time.time;
        }
    }
}
