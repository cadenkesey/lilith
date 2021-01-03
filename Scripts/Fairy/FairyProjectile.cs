// Adapted From:
// Unity Turret Tutorial
// Chris Gough
// 1/3/2021
// https://youtu.be/-_k8Ob7ElUo

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FairyProjectile : MonoBehaviour
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

            this.transform.rotation = FairyOperations.TurnTowardPlayer(this.transform, target.transform);

            // Destroy the projectile after it has existed for a certain amount of time
            Destroy(gameObject, 5.0f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        bool checkTurret = collision.gameObject.GetComponent<Fairy>() == null;

        if (collision.gameObject.GetComponent<FairyProjectile>() == null && checkTurret)
        {
            GameObject hit = Instantiate(_hitEffect, transform.position, Quaternion.Euler(transform.forward)) as GameObject;

            if (collision.gameObject.GetComponent<Death>() != null)
            {
                collision.gameObject.GetComponent<Death>().Die();
            }

            Destroy(gameObject);
        }
    }
}
