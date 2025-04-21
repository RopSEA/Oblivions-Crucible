using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidFollower : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public float followSpeed = 5f; // Adjust for smooth movement
    public float baseOffset = 10f; // Base distance from player
    public float transitionSpeed = 3f; // Speed of offset transition

    private Vector3 currentOffset; // Stores the current offset

    void Start()
    {
        currentOffset = Vector3.zero; // Start with no offset
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Determine target offset based on player's position
            float targetOffsetX = 0f;
            float targetOffsetY = 0f;

            if (player.position.x >= 17)
                targetOffsetX = baseOffset;
            else if (player.position.x <= -17)
                targetOffsetX = -baseOffset;

            if (player.position.y >= 10)
                targetOffsetY = baseOffset;
            else if (player.position.y <= -10)
                targetOffsetY = -baseOffset;

            Vector3 targetOffset = new Vector3(targetOffsetX, targetOffsetY, 0);

            // Smoothly transition to new offset using Lerp
            currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * transitionSpeed);

            // Calculate target position with smoothed offset
            Vector3 targetPosition = player.position + currentOffset;

            // Smoothly move the void toward the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }
}
