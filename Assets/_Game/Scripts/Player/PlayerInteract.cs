using System;
using Alchemist.Input;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [Header("Interact Settings")]
        [SerializeField] private Transform playerCamera;
        [SerializeField] private float interactDistance;
        [SerializeField] private LayerMask interactLayer;

        private InputCapture _inputCapture;

        private void Awake()
        {
            _inputCapture = GetComponent<InputCapture>();
        }

        private void Update()
        {
            var ray = new Ray(playerCamera.position, playerCamera.forward);
            if(Physics.SphereCast(ray, 0.4f, out var hit, interactLayer))
                if (_inputCapture.InteractKeyDown)
                {
                    var interactable = hit.collider.GetComponentInParent<IInteractable>();
                    if (interactable == null) return;
                    
                    interactable.Interact(gameObject);
                }
        }
    }
}