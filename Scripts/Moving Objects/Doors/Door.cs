// Adapted from:
// Unity Moving Platform Tutorial
// Dekoba
// 1/3/2021
// https://youtu.be/9KdY4mafG_E

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Vector3 _openPosition = default;
    [SerializeField]
    private float _openSpeed = 2f;
    [SerializeField]
    private float _waitToClose = 2f;

    // The door's closed location
    private Vector3 _closedPosition;
    // Whether door is open
    private bool _isOpen = false;
    // The location to move to
    private Vector3 _currentTarget = default;

    private float _tolerance = 0f;
    private bool _moving = false;

    private void Start()
    {
        _closedPosition = transform.position;
        _tolerance = _openSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_moving == true)
        {
            MoveDoor();
        }
    }

    /// <summary>
    /// Move door towards target location
    /// </summary>
    private void MoveDoor()
    {
        transform.position = MovingObject.Move(transform.position, _currentTarget, _openSpeed, _tolerance);

        // Door has reached target
        if (transform.position == _currentTarget)
        {
            _moving = false;

            // If door is in open position, set to close later
            if (_isOpen)
            {
                StartCoroutine(CloseDoor(_waitToClose));
            }
        }
    }

    /// <summary>
    /// Close the door automatically after some time
    /// </summary>
    /// <param name="waitTime">Time until door closes</param>
    /// <returns></returns>
    private IEnumerator CloseDoor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CloseDoor();

        _isOpen = false;
        _moving = true;
    }

    /// <summary>
    /// Change location to move towards
    /// </summary>
    private void SwitchPosition()
    {
        if (_isOpen)
        {
            _currentTarget = _closedPosition;
        }
        else
        {
            _currentTarget = _openPosition;
        }

        _isOpen = !_isOpen;
        _moving = true;
    }

    /// <summary>
    /// Close the door
    /// </summary>
    private void CloseDoor()
    {
        _currentTarget = _closedPosition;
        _moving = true;
        _isOpen = false;
    }

    /// <summary>
    /// Open the door
    /// </summary>
    private void OpenDoor()
    {
        _currentTarget = _openPosition;
        _moving = true;
        _isOpen = true;
    }

    /// <summary>
    /// Called if bumped into a player in order to open again
    /// </summary>
    public void Bumped()
    {
        StopAllCoroutines();
        OpenDoor();
    }

    /// <summary>
    /// Called if player is trying to open or close door
    /// </summary>
    public void DoorTrigger()
    {
        StopAllCoroutines();
        SwitchPosition();
    }
}
