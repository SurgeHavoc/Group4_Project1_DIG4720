using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatic : MonoBehaviour, IEnemyBehavior
{
    // Interface properties.
    public string EnemyType { get; private set; }
    public bool IsDefeated { get; set; } = false;

    private Animator EnemyAnimator;
    private SpriteRenderer SpriteRenderer;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        EnemyAnimator = GetComponent<Animator>();
    }
}
