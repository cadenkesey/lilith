using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class BillboardDirection
{
    // 8 outputs - Forward, Forward_Right, Right, Backward_Right, Backward, Backward_Left, Left, Forward_Left
    // 5 outputs - Forward, Forward_Right, Backward_Right, Backward_Left, Forward_Left
    // 4 outputs - Forward, Right, Backward, Left

    /// <summary>
    /// Return the direction the enemy is facing relative to the player
    /// </summary>
    /// <param name="enemy">The transform of the enemy</param>
    /// <param name="player">The transform of the player</param>
    /// <param name="cutoff">The angle at which the enemy should
    /// no longer be considered facing towards the player</param>
    /// <returns>The direction out of eight</returns>
    public static int GetDirection8(Transform enemy, Transform player, float cutoff)
    {
        // Get vector pointing from player to character
        Vector3 playerDir = enemy.position - player.position;
        playerDir.y = 0;
        playerDir = Vector3.Normalize(playerDir);

        // Get character's forward direction
        Vector3 characterDir = enemy.forward;
        characterDir.y = 0;
        characterDir = Vector3.Normalize(characterDir);

        // Dot product: -1 is facing towards player, 1 is facing away, 0 is left or right
        float dotProduct = Vector3.Dot(playerDir, characterDir);

        // Forward
        if (dotProduct < -cutoff)// && dotProduct >= -1.0f)
        {
            return 0;
        }
        // Backward
        else if (dotProduct > cutoff)// && dotProduct <= 1.0f)
        {
            return 4;
        }
        else
        {
            // Get vector orthogonal to vector pointing towards player
            Vector3 playerDirRight = new Vector3(playerDir.z, 0, -playerDir.x);

            // Dot Product: 1 is right, -1 is left, 0 is forward or backward relative to the player
            float dotProduct2 = Vector3.Dot(playerDirRight, characterDir);

            // Forward 45
            if (dotProduct <= -0.25f)
            {
                // Forward Right
                if (dotProduct2 >= 0)
                {
                    return 1;
                }
                // Forward Left
                else
                {
                    return 7;
                }
            }
            // Backward 45
            else if (dotProduct >= 0.25f)
            {
                // Backward Right
                if (dotProduct2 >= 0)
                {
                    return 3;
                }
                // Backward Left
                else
                {
                    return 5;
                }
            }
            // Directly left or right
            else
            {
                // Right
                if (dotProduct2 >= 0)
                {
                    return 2;
                }
                // Left
                else
                {
                    return 6;
                }
            }
        }
    }

    /// <summary>
    /// Return the direction the enemy is facing relative to the player
    /// </summary>
    /// <param name="enemy">The transform of the enemy</param>
    /// <param name="player">The transform of the player</param>
    /// <param name="cutoff">The angle at which the enemy should
    /// no longer be considered facing towards the player</param>
    /// <returns>The direction out of five</returns>
    public static int GetDirection5(Transform enemy, Transform player, float cutoff)
    {
        // Get vector pointing from player to character
        Vector3 playerDir = enemy.position - player.position;
        playerDir.y = 0;
        playerDir = Vector3.Normalize(playerDir);

        // Get character's forward direction
        Vector3 characterDir = enemy.forward;
        characterDir.y = 0;
        characterDir = Vector3.Normalize(characterDir);

        // Dot product: -1 is facing towards player, 1 is facing away, 0 is left or right
        float dotProduct = Vector3.Dot(playerDir, characterDir);

        // Forward
        if (dotProduct < -cutoff)// && dotProduct >= -1.0f)
        {
            return 0;
        }
        else
        {
            // Get vector orthogonal to vector pointing towards player
            Vector3 playerDirRight = new Vector3(playerDir.z, 0, -playerDir.x);

            // Dot Product: 1 is right, -1 is left, 0 is forward or backward relative to the player
            float dotProduct2 = Vector3.Dot(playerDirRight, characterDir);

            // Forward 45
            if (dotProduct <= 0)
            {
                // Forward Right
                if (dotProduct2 >= 0)
                {
                    return 1;
                }
                // Forward Left
                else
                {
                    return 4;
                }
            }
            // Backward 45
            else
            {
                // Backward Right
                if (dotProduct2 >= 0)
                {
                    return 2;
                }
                // Backward Left
                else
                {
                    return 3;
                }
            }
        }
    }

    /// <summary>
    /// Return the direction the enemy is facing relative to the player
    /// </summary>
    /// <param name="enemy">The transform of the enemy</param>
    /// <param name="player">The transform of the player</param>
    /// <param name="cutoff">The angle at which the enemy should
    /// no longer be considered facing towards the player</param>
    /// <returns>The direction out of four</returns>
    public static int GetDirection4(Transform enemy, Transform player, float cutoff)
    {
        // Get vector pointing from player to character
        Vector3 playerDir = enemy.position - player.position;
        playerDir.y = 0;
        playerDir = Vector3.Normalize(playerDir);

        // Get character's forward direction
        Vector3 characterDir = enemy.forward;
        characterDir.y = 0;
        characterDir = Vector3.Normalize(characterDir);

        // Dot product: -1 is facing towards player, 1 is facing away, 0 is left or right
        float dotProduct = Vector3.Dot(playerDir, characterDir);

        // Forward
        if (dotProduct <= -0.5f)// && dotProduct >= -1.0f)
        {
            return 0;
        }
        // Backward
        else if (dotProduct >= 0.5f)// && dotProduct <= 1.0f)
        {
            return 2;
        }
        else
        {
            // Get vector orthogonal to vector pointing towards player
            Vector3 playerDirRight = new Vector3(playerDir.z, 0, -playerDir.x);

            // Dot Product: 1 is right, -1 is left, 0 is forward or backward relative to the player
            float dotProduct2 = Vector3.Dot(playerDirRight, characterDir);

            // Right
            if (dotProduct2 >= 0)
            {
                return 1;
            }
            // Left
            else
            {
                return 3;
            }
        }
    }
}
