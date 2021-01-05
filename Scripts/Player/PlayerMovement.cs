// Adapted from:
// First Person Movement in Unity
// Brackeys
// 1/4/2021
// https://youtu.be/_QajrabyTJc

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement parameters
    [SerializeField]
    private float _movementSpeed = 6;
    [SerializeField]
    private float _gravity = -9.81f;
    [SerializeField]
    private float _jumpHeight = 3f;

    // Checks for touching ground
    [SerializeField]
    private Transform _groundCheck = default;
    [SerializeField]
    private float _groundDistance = 0.4f;
    [SerializeField]
    private LayerMask _groundMask = default;

    private Inputs _controls;
    private Vector2 _movementInput;
    private float _jumpInput;

    private CharacterController _charController;
    private Vector3 _verticalVelocity;
    private bool _isGrounded;

    private void Awake()
    {
        _controls = new Inputs();
        _controls.Player.Movement.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _controls.Player.Jump.performed += ctx => _jumpInput = ctx.ReadValue<float>();

        _charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMove();
    }

    /// <summary>
    /// Calculate to where the player should move
    /// </summary>
    private void PlayerMove()
    {
        // WASD input
        float horizInput = _movementInput.x * _movementSpeed * Time.deltaTime;
        float vertInput = _movementInput.y * _movementSpeed * Time.deltaTime;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;
        Vector3 wasdMovement = forwardMovement + rightMovement;

        // Ground check
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (_isGrounded && _verticalVelocity.y < 0)
        {
            _verticalVelocity.y = -2f;
        }

        // Jump
        if ((_jumpInput == 1) && _isGrounded)
        {
            _verticalVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        // Hitting ceiling
        if (((_charController.collisionFlags & CollisionFlags.Above) != 0) && _verticalVelocity.y > 0)
        {
            _verticalVelocity.y = 0;
        }

        // Gravity
        _verticalVelocity.y += _gravity * Time.deltaTime;

        // Apply all movement
        _charController.Move(wasdMovement + (_verticalVelocity * Time.deltaTime));
    }

    public void setMovementSpeed(float newSpeed)
    {
        _movementSpeed = newSpeed;
    }

    public float getMovementSpeed()
    {
        return _movementSpeed;
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
