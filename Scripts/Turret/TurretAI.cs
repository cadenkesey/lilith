using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    [SerializeField]
    private GameObject _range = default;
    [SerializeField]
    private GameObject _projectile = default;
    [SerializeField]
    private GameObject _projectileSpawn = default;

    private GameObject _target = null;
    private TurretAnimationController _animator = default;
    private RangeChecker _rangeCheck = default;

    // Turning
    private float _turnSpeed = 1.0f;

    // Shooting
    private float _fireRate = 1f;
    private float _viewAreaHorz = 10;
    private float _viewAreaVert = 60;
    private int _damage = 5;
    private float _fireTimer = 0.0f;
    
    private void Start()
    {
        _animator = GetComponent<TurretAnimationController>();
        _rangeCheck = _range.GetComponent<RangeChecker>();
    }
    
    private void Update()
    {
        GetTarget();

        _fireTimer += Time.deltaTime;

        if (_target)
        {
            TurnTowardTarget();

            if (_fireTimer >= _fireRate)
            {
                Fire();
            }
        }
    }

    /// <summary>
    /// Find closest target to attack
    /// </summary>
    private void GetTarget()
    {
        int numberOfTargets = _rangeCheck.GetNumberOfTargets();

        if (numberOfTargets > 0)
        {
            GameObject newTarget;

            if (numberOfTargets > 1)
            {
                newTarget = TurretOperations.NearestTarget(transform, _rangeCheck.GetValidTargets(), _rangeCheck.GetTags());
            }
            else
            {
                newTarget = TurretOperations.SingleTargetObstructed(transform, _rangeCheck.GetTarget(), _rangeCheck.GetTags());
            }

            if (newTarget != null && !GameObject.ReferenceEquals(_target, newTarget))
            {
                _target = newTarget;
                _animator.Activate();
                _fireTimer = 0.0f;
            }
            else if (newTarget == null && !GameObject.ReferenceEquals(_target, null))
            {
                _target = null;
            }
        }
        else
        {
            if (!GameObject.ReferenceEquals(_target, null))
            {
                _target = null;
                _animator.Deactivate();
            }
        }
    }

    /// <summary>
    /// Turn the turret a single step towards the target
    /// </summary>
    private void TurnTowardTarget()
    {
        Vector3 turnDirection = TurretOperations.RotateStepTowardsTarget(transform, _target.transform, _turnSpeed);
        transform.rotation = Quaternion.LookRotation(turnDirection);
    }

    /// <summary>
    /// Check if the target is within the angle to be fired at and then fire at them
    /// </summary>
    private void Fire()
    {
        (float angleHorz, float angleVert) = EnemyOperations.AngleToTarget(transform, _target.transform);

        if (angleHorz < _viewAreaHorz && angleVert < _viewAreaVert)
        {
            SpawnProjectile();
            _animator.Fire();
            _fireTimer = 0.0f;
        }
    }

    /// <summary>
    /// Create projectile
    /// </summary>
    private void SpawnProjectile()
    {
        GameObject proj = Instantiate(_projectile, _projectileSpawn.transform.position, Quaternion.Euler(_projectileSpawn.transform.forward)) as GameObject;
        proj.GetComponent<TurretProjectile>().FireProjectile(_projectileSpawn, _target, _damage);
    }
}