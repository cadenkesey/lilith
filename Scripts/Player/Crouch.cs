using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crouch : MonoBehaviour
{
    // Child objects of the player
    [SerializeField]
    private Transform _playerCylinder = default;
    [SerializeField]
    private float _cylinderCrouchHeight = 0.4f;
    [SerializeField]
    private Transform _playerCamera = default;
    [SerializeField]
    private float _cameraCrouchHeight = 0.1f;
    [SerializeField]
    private Transform _playerGroundCheck = default;
    [SerializeField]
    private float _groundCheckCrouchHeight = -0.06f;
    [SerializeField]
    private Transform _playerStandCheck = default;
    [SerializeField]
    private float _standCheckCrouchHeight = 0.65f;

    // Crouching player's height and movement speed
    [SerializeField]
    private float _crouchHeight = 0.5f;
    [SerializeField]
    private float _crouchSpeed = 2f;

    // How fast to adjust the camera to new height
    [SerializeField]
    private float _cameraAdjustSpeed = 0.005f;

    // Objects to check to stay crouched
    [SerializeField]
    private LayerMask _standMask = default;

    // Controls
    private Inputs _controls;
    private float _crouchInput;

    // Original heights
    private float _standingHeight;
    private float _cylinderHeight;
    private float _cameraHeight;
    private float _groundCheckHeight;
    private float _standCheckHeight;

    // Character controller and movement script
    private CharacterController _playerController;
    private PlayerMovement _playerMove;
    private float _standingSpeed;

    // Check if player has room to stand up
    private float _standDistance = 0.4f;

    // Force crouch if in a tight space
    private bool _forceCrouch = false;

    private void Awake()
    {
        _controls = new Inputs();
        _controls.Player.Crouch.performed += ctx => _crouchInput = ctx.ReadValue<float>();

        // Get character controller and movement script
        _playerController = GetComponent<CharacterController>();
        _playerMove = GetComponent<PlayerMovement>();
        _standingSpeed = _playerMove.getMovementSpeed();

        //Get original heights
        _standingHeight = _playerController.height;
        _cylinderHeight = _playerCylinder.localScale.y;
        _cameraHeight = _playerCamera.localPosition.y;
        _groundCheckHeight = _playerGroundCheck.localPosition.y;
        _standCheckHeight = _playerStandCheck.localPosition.y;
    }

    private void Update()
    {
        // Check if the player has room to stand up
        _forceCrouch = Physics.CheckSphere(_playerStandCheck.position, _standDistance, _standMask);

        //Crouching
        if (_crouchInput == 1 || _forceCrouch == true)
        {
            _playerController.height = _crouchHeight;
            _playerCylinder.localScale = new Vector3(_playerCylinder.localScale.x, _cylinderCrouchHeight, _playerCylinder.localScale.z);
            _playerGroundCheck.localPosition = new Vector3(_playerGroundCheck.localPosition.x, _groundCheckCrouchHeight, _playerGroundCheck.localPosition.z);
            _playerStandCheck.localPosition = new Vector3(_playerStandCheck.localPosition.x, _standCheckCrouchHeight, _playerStandCheck.localPosition.z);

            _playerMove.setMovementSpeed(_crouchSpeed);
        }
        // Standing
        else
        {
            _playerController.height = _standingHeight;
            _playerCylinder.localScale = new Vector3(_playerCylinder.localScale.x, _cylinderHeight, _playerCylinder.localScale.z);
            _playerGroundCheck.localPosition = new Vector3(_playerGroundCheck.localPosition.x, _groundCheckHeight, _playerGroundCheck.localPosition.z);
            _playerStandCheck.localPosition = new Vector3(_playerStandCheck.localPosition.x, _standCheckHeight, _playerStandCheck.localPosition.z);

            _playerMove.setMovementSpeed(_standingSpeed);
        }
    }

    private void LateUpdate()
    {
        if (_crouchInput == 1 || _forceCrouch == true)
        {
            if (_playerCamera.localPosition.y != _cameraCrouchHeight)
            {
                float update = _playerCamera.localPosition.y - _cameraAdjustSpeed;
                if (update < _cameraCrouchHeight)
                {
                    _playerCamera.localPosition = new Vector3(_playerCamera.localPosition.x, _cameraCrouchHeight, _playerCamera.localPosition.z);
                }
                else
                {
                    _playerCamera.localPosition = new Vector3(_playerCamera.localPosition.x, update, _playerCamera.localPosition.z);
                }
            }
        }
        else
        {
            if (_playerCamera.localPosition.y != _cameraHeight)
            {
                float update = _playerCamera.localPosition.y + _cameraAdjustSpeed;
                if (update > _cameraHeight)
                {
                    _playerCamera.localPosition = new Vector3(_playerCamera.localPosition.x, _cameraHeight, _playerCamera.localPosition.z);
                }
                else
                {
                    _playerCamera.localPosition = new Vector3(_playerCamera.localPosition.x, update, _playerCamera.localPosition.z);
                }
            }
        }
    }

    public bool getForcedCrouch()
    {
        return _forceCrouch;
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
