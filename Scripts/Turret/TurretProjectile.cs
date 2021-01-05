// Adapted From:
// Unity Turret Tutorial
// Chris Gough
// 1/3/2021
// https://youtu.be/-_k8Ob7ElUo

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _hitEffect = default;

    private Vector3 _targetDirection;
    private bool _hasFired;

    private void Update()
    {
        if (_hasFired)
        {
            transform.position += _targetDirection * (_speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Send the projectile moving towards its target
    /// </summary>
    /// <param name="launcher">The object where the projectile is created</param>
    /// <param name="target">The target for the projectile to hit</param>
    /// <param name="damage">The amount of damage the projectile will do</param>
    public void FireProjectile(GameObject launcher, GameObject target, int damage)
    {
        if (launcher & target)
        {
            _targetDirection = (target.transform.position - launcher.transform.position).normalized;
            _hasFired = true;

            Destroy(gameObject, 5.0f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        bool checkTurret = collision.gameObject.GetComponent<TurretAI>() == null && collision.gameObject.GetComponent<RangeChecker>() == null;

        if (collision.gameObject.GetComponent<TurretProjectile>() == null && checkTurret)
        {
            GameObject hit = Instantiate(_hitEffect, transform.position, Quaternion.Euler(transform.forward)) as GameObject;
            Destroy(gameObject);
        }
    }
}
