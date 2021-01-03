using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyHealth : MonoBehaviour
{
    /*[SerializeField]
    private float health = 100f;
    [SerializeField]
    private float defense = 20f;*/
    /*[SerializeField]
    private GameObject deadObject = default;*/

    private float _health = 10f;

    [SerializeField]
    private GameObject _hitEffect = default;

    public GameObject HitEffect
    {
        get
        {
            return _hitEffect;
        }
    }

    /// <summary>
    /// Take a point away from the enemy's health
    /// Set the enemy to die if health is gone
    /// </summary>
    public void Hit()
    {
        _health -= 1;
        
        if (_health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Destroy the enemy
    /// </summary>
    public void Die()
    {
        //Instantiate(deadObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
