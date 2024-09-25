using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDetect : MonoBehaviour
{
    GameObject enemy;
    private Animator EnemyAnimator;
    public float BounceForce = 5f;

    private IEnemyBehavior EnemyBehavior;

    private AudioSource PlayerAudioSource;
    public AudioClip JumpSound;

    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.parent.gameObject;
        EnemyAnimator = enemy.GetComponent<Animator>();

        EnemyBehavior = enemy.GetComponent<IEnemyBehavior>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerAudioSource = player.GetComponent<AudioSource>();
    }

    public void TriggerDefeatByAttack()
    {
        if (EnemyAnimator != null)
        {
            EnemyAnimator.SetTrigger("DefeatTrigger");
        }

        if (EnemyBehavior != null)
        {
            EnemyBehavior.IsDefeated = true;
        }

        // Reach over to LevelExitManager to notify that a unique enemy type has been defeated.
        LevelExitManager LevelExitManager = FindObjectOfType<LevelExitManager>();
        if (LevelExitManager != null && EnemyBehavior != null)
        {
            LevelExitManager.EnemyDefeated(EnemyBehavior.EnemyType);
        }

        // Disable colliders.
        enemy.GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Destroy the enemy after a delay.
        StartCoroutine(DestroyEnemyAfterDelay(3f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // Call helper function to facilitate actions regarding enemy defeat.
            TriggerDefeatByAttack();

            // Player bounces after landing on top of enemy.
            Rigidbody2D PlayerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if(PlayerRb != null)
            {
                PlayerRb.velocity = new Vector2(PlayerRb.velocity.x, BounceForce);

                PlayerAudioSource.PlayOneShot(JumpSound);
            }
        }
    }

    private IEnumerator DestroyEnemyAfterDelay(float delay)
    {
        // Wait for the delay.
        yield return new WaitForSeconds(delay);

        // Destroy the enemy.
        //Destroy(enemy);

        enemy.SetActive(false);
    }
}
