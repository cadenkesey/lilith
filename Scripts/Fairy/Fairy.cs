using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : MonoBehaviour
{
    [SerializeField]
    private GameObject _player = null;
    [SerializeField]
    private GameObject _groundCheck = null;
    [SerializeField]
    private float _movementSpeed = 3.0f;
    [SerializeField]
    private Waypoint[] _patrolPoints = null;
    [SerializeField]
    private GameObject _projectile = default;
    [SerializeField]
    private GameObject _projectileSpawn = default;
    [SerializeField]
    private GameObject _projectileCharge = default;

    private float _turnSpeed = 3.0f;
    private float _fieldOfViewHorizontal = 10;
    private float _fieldOfViewVertical = 60;
    private int _projectileDamage = 5;
    private bool _firedLastTurn = false;

    public ChargeAnimationController ChargeAnimationControl
    {
        get
        {
            return _projectileCharge.GetComponent<ChargeAnimationController>(); ;
        }
    }

    public GameObject Player
    {
        get
        {
            return _player;
        }
    }

    public GameObject GroundCheck
    {
        get
        {
            return _groundCheck;
        }
    }

    public Waypoint[] PatrolPoints
    {
        get
        {
            return _patrolPoints;
        }
    }

    public float MovementSpeed
    {
        get
        {
            return _movementSpeed;
        }
    }

    public bool FiredLastTurn
    {
        get
        {
            return _firedLastTurn;
        }
        set
        {
            _firedLastTurn = value;
        }
    }

    /// <summary>
    /// Check if the player is close enough to be detected
    /// </summary>
    /// <returns>True if player is detected</returns>
    public bool PlayerNearby()
    {
        if (PlayerInView(15.0f, 90, 90) || PlayerInView(1.0f, 180, 90))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Check if the player is visible to the enemy
    /// </summary>
    /// <returns>True if the player is within sight</returns>
    public bool PlayerVisible()
    {
        if (PlayerInView(15.0f, 360, 360))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Check if the player is within a certain field of view and range
    /// </summary>
    /// <param name="viewDistance">The maximum distance the enemy can see</param>
    /// <param name="viewHorz">The enemy's field of view horizontally</param>
    /// <param name="viewVert">The enemy's field of view vertically</param>
    /// <returns>True if the player is visible</returns>
    public bool PlayerInView(float viewDistance, float viewHorz, float viewVert)
    {
        // Check if an object is visible in the direction of the player from the enemy
        RaycastHit playerHit;
        if (Physics.Raycast(transform.position, _player.transform.position - transform.position, out playerHit, viewDistance))
        {
            Transform target = playerHit.transform;

            // Check if the detected object is the player
            if (target.CompareTag("Player"))
            {
                (float angleHorz, float angleVert) = EnemyOperations.AngleToTarget(transform, _player.transform);

                // Check if the player is in front of the enemy within a line of sight
                if (angleHorz < viewHorz && angleVert < viewVert)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Check if the player is within range to be fired at and then fire
    /// </summary>
    /// <returns>True if the enemy was able to fire at the player</returns>
    public bool Fire()
    {
        (float angleHorz, float angleVert) = EnemyOperations.AngleToTarget(transform, _player.transform);

        if (angleHorz < _fieldOfViewHorizontal && angleVert < _fieldOfViewVertical)
        {
            CreateProjectile();
            _firedLastTurn = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Create a projectile and aim it towards the player
    /// </summary>
    private void CreateProjectile()
    {
        GameObject proj = Instantiate(_projectile, _projectileSpawn.transform.position, Quaternion.Euler(_projectileSpawn.transform.forward)) as GameObject;
        proj.GetComponent<ProjectileAnimationController>().Player = _player;
        proj.GetComponent<ProjectileAnimationController>().PlayFlyAnim();
        proj.GetComponent<FairyProjectile>().FireProjectile(_projectileSpawn, _player, _projectileDamage);
    }
}
