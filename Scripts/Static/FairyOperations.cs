using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FairyOperations
{
    /// <summary>
    /// Returns rotation to rotate enemy a single step towards the target
    /// </summary>
    /// <param name="enemy">The enemy transform</param>
    /// <param name="target">The player transform</param>
    /// <param name="turnSpeed">The speed at which the enemy should turn</param>
    /// <returns>The new rotation vector</returns>
    public static Vector3 RotateStepTowardsTarget(Transform enemy, Transform target, float turnSpeed)
    {
        Vector3 targetDirection = target.position - enemy.position;

        float singleStep = turnSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(enemy.forward, targetDirection, singleStep, 0.0f);
        newDirection.y = 0;

        return newDirection;
    }

    /// <summary>
    /// Returns rotation to rotate enemy towards the target
    /// </summary>
    /// <param name="enemy">The enemy transform</param>
    /// <param name="target">The player transform</param>
    /// <returns></returns>
    public static Quaternion TurnTowardPlayer(Transform enemy, Transform target)
    {
        Vector3 targetDirection = target.position - enemy.position;

        Vector3 newDirection = Vector3.RotateTowards(enemy.forward, targetDirection, 7.0f, 0.0f);
        newDirection.y = 0;

        return Quaternion.LookRotation(newDirection);
    }

    /// <summary>
    /// Check if the next projected position of the enemy is too close to the player
    /// </summary>
    /// <param name="nextMovement">The projected position of the enemy</param>
    /// <param name="fairyPosition">The current position of the enemy</param>
    /// <param name="targetPosition">The position of the player</param>
    /// <param name="distance">The distance to the player</param>
    /// <returns>True is too close</returns>
    public static bool CheckNextPosition(Vector3 nextMovement, Vector3 fairyPosition, Vector3 targetPosition, float distance)
    {
        Vector3 projectedNextPosition = nextMovement + fairyPosition;

        Vector3 towardsNextPosition2D = new Vector3(projectedNextPosition.x - fairyPosition.x, 0, projectedNextPosition.z - fairyPosition.z);
        Vector3 towardsPlayer2D = new Vector3(targetPosition.x - fairyPosition.x, 0, targetPosition.z - fairyPosition.z);
        float angleAwayFromPlayer = Vector3.Angle(towardsPlayer2D.normalized, towardsNextPosition2D.normalized);

        Vector3 distanceToPlayer = targetPosition - projectedNextPosition;

        bool angleTooClose = angleAwayFromPlayer < 45.0f || angleAwayFromPlayer > 165.0f;

        // Check if enemy is moving too close to the player
        if (distanceToPlayer.magnitude < distance && angleTooClose)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Calculate the next step towards the target
    /// </summary>
    /// <param name="currentCorner">The current corner of the pathway to head towards</param>
    /// <param name="groundCheckPosition">The location of the bottom of the enemy</param>
    /// <param name="movementSpeed">The speed at which the enemy should move</param>
    /// <param name="result">The enemy's next position to move to</param>
    /// <returns>True if the enemy will reach the target this step</returns>
    public static bool GetNextPosition(Vector3 currentCorner, Vector3 groundCheckPosition, float movementSpeed, out Vector3 result)
    {
        result = groundCheckPosition;

        Vector3 heading = currentCorner - groundCheckPosition;
        Vector3 update = (heading / heading.magnitude) * movementSpeed * Time.deltaTime;

        float tolerance = movementSpeed * Time.deltaTime;

        if ((groundCheckPosition + update == currentCorner) || (heading.magnitude < tolerance))
        {
            result = currentCorner - groundCheckPosition;
            return true;
        }
        else
        {
            if (!float.IsNaN(update.x) && !float.IsNaN(update.y) && !float.IsNaN(update.z))
            {
                result = update;
            }
            return false;
        }
    }

    /// <summary>
    /// Find a new random point on the navmesh for the enemy to move to
    /// </summary>
    /// <param name="center">The center of the circle to search</param>
    /// <param name="range">The radius of the circle</param>
    /// <param name="result">The point to move to</param>
    /// <returns>True if a new point was able to be found</returns>
    public static bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 randomCirclePoint = Random.insideUnitCircle * range;
            Vector3 randomPoint = new Vector3(center.x + randomCirclePoint.x, center.y, center.z + randomCirclePoint.y);

            //Vector3 randomPoint = center + (Random.insideUnitSphere * range);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    /// <summary>
    /// Remove any duplicate corners from the list
    /// Make sure the first corner is not the enemy's current position
    /// </summary>
    /// <param name="pathwayCorners">The list of corners along the pathway</param>
    /// <param name="fairyStart">The starting position of the enemy</param>
    /// <returns>Returns an adjusted list of corners</returns>
    public static List<Vector3> CheckArrayForDuplicates(Vector3[] pathwayCorners, Vector3 fairyStart)
    {
        List<Vector3> returnList = new List<Vector3>();

        if (pathwayCorners != null && pathwayCorners.Length > 0)
        {
            Vector3 lastCorner = fairyStart;

            for (int i = 0; i < pathwayCorners.Length; i++)
            {
                if (pathwayCorners[i] != lastCorner)
                {
                    returnList.Add(pathwayCorners[i]);
                    lastCorner = pathwayCorners[i];
                }
            }
        }

        return returnList;
    }

    /// <summary>
    /// Determine if the fairy should fire at the player
    /// </summary>
    /// <param name="fairyPosition">The transform of the enemy</param>
    /// <param name="targetPosition">The transform of the player</param>
    /// <returns>True if the enemy should fire</returns>
    public static bool chanceToFire(Transform fairyPosition, Transform targetPosition)
    {
        float distanceToPlayer = (targetPosition.position - fairyPosition.position).magnitude;

        if (distanceToPlayer > 20)
        {
            distanceToPlayer = 0;
        }
        else
        {
            distanceToPlayer = 20 - distanceToPlayer;
        }

        float distanceChance = (distanceToPlayer / 20);

        float randomChance = Random.Range(0.0f, 1.0f);

        float chanceToAttack = distanceChance * 0.7f + randomChance * 0.3f;

        if (chanceToAttack > 0.8)
        {
            return true;
        }

        return false;
    }
}
