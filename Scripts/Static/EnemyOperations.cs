using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOperations
{
    /// <summary>
    /// Get the angles (in degrees) from the enemy to its target on the x-z plane (horizontal) and the y-axis (vertical)
    /// </summary>
    /// <param name="enemy">The transform of the enemy</param>
    /// <param name="target">The transform of the player</param>
    /// <returns>Returns a horizontal angle float and a vertical angle float</returns>
    public static (float, float) AngleToTarget(Transform enemy, Transform target)
    {
        Vector3 enemyPos = enemy.position;
        Vector3 targetPos = target.position;

        Vector3 enemyAtTargetHeight = new Vector3(enemyPos.x, targetPos.y, enemyPos.z);

        float horzDist = Vector3.Distance(targetPos, enemyAtTargetHeight);
        float vertDist = enemyPos.y - targetPos.y;
        float angleVert = Mathf.Abs(Mathf.Atan(vertDist / horzDist)) * Mathf.Rad2Deg;

        float angleHorz = Mathf.Abs(Vector3.Angle(targetPos - enemyAtTargetHeight, enemy.forward));

        return (angleHorz, angleVert);
    }
}
