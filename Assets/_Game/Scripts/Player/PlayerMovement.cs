using System.Collections;
using System.Collections.Generic;
using Alchemist.Input;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float speed = 8f;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private Transform playerBody;
    
        [Header("Ground Check Settings")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask;
        
        private CharacterController _characterController;
        private Vector3 _velocity;
        private bool _isGrounded;

        private InputCapture _inputCapture;

        // Start is called before the first frame update
        void Start()
        {
            _inputCapture = GetComponent<InputCapture>();
            _characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            CalculateGrounded();
            ProcessMovement();
            ProcessGravity();
        
            if(_inputCapture.enableInput && _inputCapture.JumpKeyDown && _isGrounded)
                Jump();
        }
    
        /// <summary>
        /// Calculates if the character has hit the ground or not.
        /// </summary>
        private void CalculateGrounded()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (_isGrounded && _velocity.y < 0)
                _velocity.y = -2;
        }
    
        /// <summary>
        /// Moves the character according to the Inputs.
        /// </summary>
        private void ProcessMovement()
        {
            var move = playerBody.right * _inputCapture.Horizontal +
                       playerBody.forward * _inputCapture.Vertical;

            _characterController.Move(Vector3.ClampMagnitude(move, 1) * (speed * Time.deltaTime));
        }
    
        /// <summary>
        /// Applies gravity to the character.
        /// </summary>
        private void ProcessGravity()
        {
            _velocity.y += gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    
        /// <summary>
        /// Make the character jump.
        /// </summary>
        private void Jump()
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}