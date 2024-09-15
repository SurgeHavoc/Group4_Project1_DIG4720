using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatic : MonoBehaviour, IEnemyBehavior
{
    [SerializeField]
    private string enemyType;

    // Interface properties.
    public string EnemyType
    {
        // Set the enemy type value in the inspector.
        get { return enemyType; }
    }

    public bool IsDefeated { get; set; } = false;

    private Animator EnemyAnimator;
    private SpriteRenderer SpriteRenderer;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        EnemyAnimator = GetComponent<Animator>();
    }
}
