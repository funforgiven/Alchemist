using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Transform playerBody;
        [SerializeField] private Transform playerCamera;
        [SerializeField] private float cameraSensitivity = 300f;
        [SerializeField] private bool lockCursorOnStart = true;
        [SerializeField] private Vector2 angleLimits = new Vector2(-80, 80);

        private PlayerInput _input;

        // This stores camera's X rotation in order to
        // eliminate quaternions from our workflow.
        private float _cameraXRotation;

        private void Start()
        {
            _input = GetComponent<PlayerInput>();
            
            if (lockCursorOnStart)
                Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            var mouseX = _input.MouseX * cameraSensitivity;
            var mouseY = _input.MouseY * cameraSensitivity;

            _cameraXRotation -= mouseY;
            _cameraXRotation = Mathf.Clamp(_cameraXRotation, angleLimits.x, angleLimits.y);
        
            playerCamera.localRotation = Quaternion.Euler(_cameraXRotation, 0, 0);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}