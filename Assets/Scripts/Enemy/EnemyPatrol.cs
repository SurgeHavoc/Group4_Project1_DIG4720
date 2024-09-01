using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // Set waypoints for enemy to patrol.
    public Transform[] waypoints;
    public float EnemyPatrolSpeed = 2f;
    // Pause at point.
    public float EnemyPatrolWaitTime = 1f;

    // Current waypoint for patrol enemy to move to.
    private int CurrentWaypointIndex = 0;
    // Enemy should not start paused.
    private bool waiting = false;

    private SpriteRenderer SpriteRenderer;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(!waiting)
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        Transform TargetWaypoint = waypoints[CurrentWaypointIndex];
        Vector2 direction = TargetWaypoint.position - transform.position;

        if(direction.x > 0)
        {
            SpriteRenderer.flipX = false;
        }
        else if(direction.x < 0)
        {
            SpriteRenderer.flipX = true;
        }

        // Enemy moves toward the current waypoint.
        transform.Translate(EnemyPatrolSpeed * Time.deltaTime * direction.normalized);

        // Check if the enemy has reached the current waypoint.
        if (Vector2.Distance(transform.position, TargetWaypoint.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        waiting = true;
        yield return new WaitForSeconds(EnemyPatrolWaitTime);
        CurrentWaypointIndex = (CurrentWaypointIndex + 1) % waypoints.Length;
        waiting = false;
    }

    private void OnDrawGizmos()
    {
        if(waypoints != null && waypoints.Length > 1)
        {
            for(int i = 0; i < waypoints.Length; i ++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(waypoints[i].position, 0.1f);

                if(i < waypoints.Length - 1)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                }
            }
        }
    }
}
