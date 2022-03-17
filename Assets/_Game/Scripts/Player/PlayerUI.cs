using Alchemist.Input;
using UnityEngine;

namespace Player
{
    public class PlayerUI : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private Canvas inventoryUI;

        private InputCapture _inputCapture;
        private bool _isShowingInventory = false;

        private void Awake()
        {
            _inputCapture = GetComponent<InputCapture>();
        }

        private void Update()
        {
            if(_inputCapture.InventoryToggleKeyDown)
                ToggleInventory();
        }

        /// <summary>
        /// Shows inventory UI.
        /// </summary>
        private void ToggleInventory()
        {
            _isShowingInventory = !_isShowingInventory;
            
            inventoryUI.enabled = _isShowingInventory;
            _inputCapture.enableInput = !_isShowingInventory;

            Cursor.lockState = _isShowingInventory ? CursorLockMode.Confined : CursorLockMode.Locked;
        }
    }
}