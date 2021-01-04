using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private Transform _parentDoor;

    private void Awake()
    {
        _parentDoor = this.transform.parent;
    }

    /// <summary>
    /// Door needs to open if closing on someone
    /// </summary>
    /// <param name="other">The object colliding with the door trigger</param>
    private void OnTriggerStay(Collider other)
    {
        _parentDoor.GetComponent<Door>().Bumped();
    }

    /// <summary>
    /// A player is opening or closing the door
    /// </summary>
    public void Use()
    {
        _parentDoor.GetComponent<Door>().DoorTrigger();
    }
}
