using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float SmoothSpeed = 0.125f;
    public Vector2 offset;

    private float MinY; // Minimum Y value that the camera can go down to.

    private void Start()
    {
        MinY = transform.position.y - 25; // Offset of 25 allows player to think through about what they had just done.
    }
    private void FixedUpdate()
    {
        // Calculate the Vector3 of the desired position based off of the player's position and offset.
        Vector3 DesiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        // Clamp the Y position of the camera to keep it from falling.
        DesiredPosition.y = Mathf.Max(DesiredPosition.y, MinY);

        // Allows the camera to smoothly move towards the desired position.
        Vector3 SmoothedPosition = Vector3.Lerp(transform.position, DesiredPosition, SmoothSpeed);
        transform.position = SmoothedPosition;
    }
}
