using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    private Camera _playerCamera = default;

    [SerializeField]
    private GameObject _playerWeapon = default;

    [SerializeField]
    // How far can the player be from an object to use it
    private float _useRange = 1f;
    [SerializeField]
    // How far can the player be from an object to shoot it
    private float _shootRange = 1f;

    [SerializeField]
    // How far can the player be from an object to shoot it
    private GameObject _hitEffect = default;

    private Inputs _controls;
    private WeaponAnimationController _weaponAnimate = null;
    private float _adjustWeapon = .01f;
    private bool _shooting = false;
    private float _shootSpeed = .25f;

    private int _layerMask = (1 << 0) | (1 << 8);

    private void Awake()
    {
        _controls = new Inputs();
        _controls.Player.Use.started += ctx => useAction();
        _controls.Player.Shoot.started += ctx => shootAction();
        _controls.Player.Quit.started += ctx => quitAction();
    }

    private void Start()
    {
        _weaponAnimate = _playerWeapon.GetComponent<WeaponAnimationController>();
    }

    /// <summary>
    /// Called when player presses the use key
    /// </summary>
    private void useAction()
    {
        RaycastHit useHit;
        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out useHit, _useRange))
        {
            Transform target = useHit.transform;

            if (target.CompareTag("Door"))
            {
                DoorTrigger door = target.GetComponent<DoorTrigger>();
                if (door != null)
                {
                    door.Use();
                }
            }

            if (target.CompareTag("Message Switch"))
            {
                MessageSwitch mSwitch = target.GetComponent<MessageSwitch>();
                if (mSwitch != null)
                {
                    mSwitch.Use();
                }
            }

            if (target.CompareTag("Platform Switch"))
            {
                PlatformSwitch pSwitch = target.GetComponent<PlatformSwitch>();
                if (pSwitch != null)
                {
                    pSwitch.Use();
                }
            }

            if (target.CompareTag("Turret"))
            {
                TurretHealth turretHealth = target.GetComponent<TurretHealth>();
                if (turretHealth != null)
                {
                    turretHealth.Die();
                }
            }
        }
    }

    /// <summary>
    /// Called when player presses the shoot key
    /// </summary>
    private void shootAction()
    {
        if (!_shooting)
        {
            _shooting = true;

            if (_weaponAnimate != null)
            {
                Vector3 newPos = new Vector3(_playerWeapon.transform.localPosition.x, _playerWeapon.transform.localPosition.y + _adjustWeapon, _playerWeapon.transform.localPosition.z - _adjustWeapon);
                _playerWeapon.transform.localPosition = newPos;
                _weaponAnimate.PlayFireAnim();
            }

            RaycastHit shootHit;
            if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out shootHit, _shootRange, _layerMask, QueryTriggerInteraction.Ignore))
            {
                Transform target = shootHit.transform;

                if (target.CompareTag("Enemy"))
                {
                    FairyHealth enemyHealth = target.GetComponent<FairyHealth>();

                    GameObject hit = Instantiate(enemyHealth.HitEffect, shootHit.point, Quaternion.Euler(transform.forward)) as GameObject;

                    if (enemyHealth != null)
                    {
                        enemyHealth.Hit();
                    }
                }
                else
                {
                    Debug.Log(target);

                    GameObject hit = Instantiate(_hitEffect, shootHit.point, Quaternion.Euler(transform.forward)) as GameObject;
                }
            }

            StartCoroutine(StopShoot(_shootSpeed));
        }
    }

    /// <summary>
    /// Called when the player presses the quit key
    /// </summary>
    private void quitAction()
    {
        Application.Quit();
    }

    /// <summary>
    /// Stop the gun firing animation and allow the player to fire again
    /// </summary>
    /// <param name="playSpeed">The time to keep the gun animation playing</param>
    /// <returns></returns>
    private IEnumerator StopShoot(float playSpeed)
    {
        yield return new WaitForSeconds(playSpeed);

        if (_weaponAnimate != null)
        {
            Vector3 newPos = new Vector3(_playerWeapon.transform.localPosition.x, _playerWeapon.transform.localPosition.y - _adjustWeapon, _playerWeapon.transform.localPosition.z + _adjustWeapon);
            _playerWeapon.transform.localPosition = newPos;
            _weaponAnimate.PlayIdleAnim();
        }

        _shooting = false;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
