using UnityEngine;
using Alchemist.Input;

namespace Player
{
    public class PlayerInput : InputCapture
    {
        [Header("Input Settings")]
        [SerializeField] private KeyCode primaryUseKey;
        [SerializeField] private KeyCode interactKey;
        [SerializeField] private KeyCode jumpKey;
        [SerializeField] private KeyCode inventoryKey;
        
        private void Update()
        {
            Horizontal = enableInput ? Input.GetAxis("Horizontal") : 0;
            Vertical = enableInput ? Input.GetAxis("Vertical") : 0;
            MouseX = enableInput ? Input.GetAxis("Mouse X") : 0;
            MouseY = enableInput ? Input.GetAxis("Mouse Y") : 0;

            LeftMouseKeyDown = enableInput && Input.GetKeyDown(primaryUseKey);
            InteractKeyDown = enableInput && Input.GetKeyDown(interactKey);
            JumpKeyDown = enableInput && Input.GetKeyDown(jumpKey);
            
            // Don't check input enable for inventory.
            InventoryToggleKeyDown = Input.GetKeyDown(inventoryKey);
        }
    }
}