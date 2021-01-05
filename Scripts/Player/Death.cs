using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField]
    private float _crushDistance = 0.4f;
    [SerializeField]
    private LayerMask _crushMask = default;

    [SerializeField]
    private GameObject _hit = default;

    private Crouch _playerCrouch;
    private Transform _playerPosition;

    private void Awake()
    {
        _playerCrouch = GetComponent<Crouch>();
        _playerPosition = GetComponent<Transform>();
        _hit.SetActive(false);
    }

    private void Update()
    {
        bool touchingFloors = Physics.CheckSphere(_playerPosition.position, _crushDistance, _crushMask);

        // If player has floors within their crush distance
        if (touchingFloors)
        {
            // Check all of the objects to see if any are platforms
            Collider[] hitColliders = Physics.OverlapSphere(_playerPosition.position, _crushDistance, _crushMask);
            int i = 0;
            for (i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.CompareTag("Platform"))
                {
                    // If player is colliding with platforms and forced to crouch, then die
                    if (_playerCrouch.getForcedCrouch())
                    {
                        Die();
                    }
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Play the hit effect on the screen
    /// </summary>
    public void Die()
    {
        //Debug.Log("YOU HAVE DIED");
        _hit.SetActive(true);

        StartCoroutine(StopFlash(0.1f));
    }

    /// <summary>
    /// Turn of the hit effect
    /// </summary>
    /// <param name="playSpeed">The time to keep the effect active</param>
    /// <returns></returns>
    private IEnumerator StopFlash(float playSpeed)
    {
        yield return new WaitForSeconds(playSpeed);

        _hit.SetActive(false);
    }
}
