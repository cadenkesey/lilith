// Adapted from:
// Unity Moving Platform Tutorial
// Dekoba
// 1/3/2021
// https://youtu.be/9KdY4mafG_E

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private Vector3[] _destinationPoints = default;
    [SerializeField]
    private float _movementSpeed = 2f;
    [SerializeField]
    private float _timeToWait = 1f;
    [SerializeField]
    private bool _automatic = default;

    private int _locationIndex = 0;
    private Vector3 _nextLocation = default;
    private float _timeSinceStopping = 0f;
    private bool _active = false;
    private int _occupants = 0;
    private float _tolerance = 0f;

    public bool Automatic
    {
        get
        {
            return _automatic;
        }
    }

    private void Start()
    {
        if (_destinationPoints.Length > 0)
        {
            _nextLocation = _destinationPoints[0];
        }
        _tolerance = _movementSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (transform.position != _nextLocation)
        {
            if (!_active)
            {
                MovePlatform();
            }
            else
            {
                if (_occupants > 0)
                {
                    MovePlatform();
                }
            }
        }
        else
        {
            if (_automatic || _active)
            {
                UpdateTarget();
            }
        }
    }

    /// <summary>
    /// Move platform towards target location
    /// </summary>
    private void MovePlatform()
    {
        Vector3 heading = _nextLocation - transform.position;
        Vector3 update = (heading / heading.magnitude) * _movementSpeed * Time.deltaTime;

        if (transform.position + update == _nextLocation)
        {
            ReachTarget();
        }
        else
        {
            if (!float.IsNaN(update.x) && !float.IsNaN(update.y) && !float.IsNaN(update.z))
            {
                transform.position += update;
            }
            if (heading.magnitude < _tolerance)
            {
                ReachTarget();
            }
        }
    }

    /// <summary>
    /// Move platform to reach target location exactly
    /// </summary>
    private void ReachTarget()
    {
        transform.position = _nextLocation;
        _timeSinceStopping = Time.time;

        _active = false;
    }

    /// <summary>
    /// Wait for delay time then update target location
    /// </summary>
    private void UpdateTarget()
    {
        if (Time.time - _timeSinceStopping > _timeToWait)
        {
            NextPlatform();
        }
    }

    /// <summary>
    /// Update target location
    /// </summary>
    private void NextPlatform()
    {
        _locationIndex++;
        if (_locationIndex >= _destinationPoints.Length)
        {
            _locationIndex = 0;
        }
        _nextLocation = _destinationPoints[_locationIndex];
    }

    /// <summary>
    /// Called when a player activates the platform
    /// </summary>
    public void PlayerTrigger()
    {
        if (transform.position == _nextLocation)
        {
            _timeSinceStopping = Time.time;
            _active = true;
        }
    }

    public void SetOccupants(int increase)
    {
        _occupants = _occupants + increase;
    }

    public void SwitchTrigger()
    {
        NextPlatform();
    }
}
