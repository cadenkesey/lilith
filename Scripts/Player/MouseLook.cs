// Adapted from:
// First Person Controller
// Acacia Developer
// 1/4/2021
// https://youtu.be/n-KX8AeGK7E

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivity = 1f;

    private Inputs _controls;
    private Vector2 _mouseInput;
    private Transform _playerBody;
    private float _xRotation = 0f;

    private void Awake()
    {
        _controls = new Inputs();
        _controls.Player.Look.performed += ctx => _mouseInput = ctx.ReadValue<Vector2>();

        _playerBody = this.transform.parent;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = _mouseInput.x * _mouseSensitivity;
        float mouseY = _mouseInput.y * _mouseSensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);
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
