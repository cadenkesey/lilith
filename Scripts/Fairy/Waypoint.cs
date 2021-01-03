// Adapted from:
// Finite State Machines in Unity
// Table Flip Games
// 12/27/2020
// https://youtu.be/DO-6ikrN9jg

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Vector3 getLocation()
    {
        return this.transform.position;
    }
}