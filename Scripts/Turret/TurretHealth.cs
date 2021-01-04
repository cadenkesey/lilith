using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    /*[SerializeField]
    private float health = 100f;
    [SerializeField]
    private float defense = 20f;*/
    [SerializeField]
    private GameObject _deadObject = default;

    /// <summary>
    /// Destroy the turret and replace it with a new object
    /// </summary>
    public void Die()
    {
        Instantiate(_deadObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}