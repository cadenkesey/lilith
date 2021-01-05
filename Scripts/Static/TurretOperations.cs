using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretOperations
{
    // Returns rotation to rotate enemy a single step towards the target
    public static Vector3 RotateStepTowardsTarget(Transform enemy, Transform target, float turnSpeed)
    {
        Vector3 targetDirection = target.position - enemy.position;

        float singleStep = turnSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(enemy.forward, targetDirection, singleStep, 0.0f);
        newDirection.y = 0;

        return newDirection;
    }

    // Return the nearest target that is in the enemy's view
    public static GameObject NearestTarget(Transform enemy, List<GameObject> validTargets, List<string> tags)
    {
        GameObject currentTarget = null;
        float closestDist = 0.0f;

        for (int i = 0; i < validTargets.Count; i++)
        {
            GameObject replacementTarget = validTargets[i];

            float dist = Vector3.Distance(enemy.position, replacementTarget.transform.position);

            bool shouldReplace = (currentTarget == null) || (dist < closestDist);

            if (shouldReplace && !TargetObstructed(enemy, replacementTarget, tags))
            {
                currentTarget = replacementTarget;
                closestDist = dist;
            }
        }

        return currentTarget;
    }

    // Check if there is an object between the enemy and the target
    public static bool TargetObstructed(Transform enemy, GameObject target, List<string> tags)
    {
        bool obstructed = true;

        RaycastHit targetHit;
        bool hitObject = Physics.Raycast(enemy.position, target.transform.position - enemy.position, out targetHit);

        if (hitObject)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (targetHit.collider.CompareTag(tags[i]))
                {
                    obstructed = false;
                    break;
                }
            }
        }

        return obstructed;
    }

    // Returns null if a single target is obstructed
    public static GameObject SingleTargetObstructed(Transform enemy, GameObject target, List<string> tags)
    {
        if (TargetObstructed(enemy, target, tags))
        {
            return null;
        }
        else
        {
            return target;
        }
    }
}
