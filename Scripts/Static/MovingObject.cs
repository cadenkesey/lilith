using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovingObject
{
    /// <summary>
    /// Move object towards target location
    /// </summary>
    /// <param name="objectPos">The object's current position</param>
    /// <param name="targetPos">The target location to move to</param>
    /// <param name="speed">The speed at which the object should move</param>
    /// <param name="tolerance">The distance at which the object should reach the target location</param>
    /// <returns></returns>
    public static Vector3 Move(Vector3 objectPos, Vector3 targetPos, float speed, float tolerance)
    {
        Vector3 objectPosNew = objectPos;

        Vector3 heading = targetPos - objectPos;
        Vector3 update = (heading / heading.magnitude) * speed * Time.deltaTime;

        if (objectPos + update == targetPos)
        {
            objectPosNew = targetPos;
        }
        else
        {
            if (!float.IsNaN(update.x) && !float.IsNaN(update.y) && !float.IsNaN(update.z))
            {
                objectPosNew = objectPos + update;
            }
            if (heading.magnitude < tolerance)
            {
                objectPosNew = targetPos;
            }
        }

        return objectPosNew;
    }

    /// <summary>
    /// Determine the next location to move to
    /// </summary>
    /// <param name="points">List of all destinations</param>
    /// <param name="currentPoint">The current location</param>
    /// <returns>The next location's index and position</returns>
    public static (int, Vector3) NextLocation(Vector3[] points, int currentPoint)
    {
        int nextPoint = currentPoint + 1;
        if (nextPoint >= points.Length)
        {
            nextPoint = 0;
        }

        Vector3 nextLocation = points[nextPoint];

        return (nextPoint, nextLocation);
    }
}
