using UnityEngine;

namespace Alchemist.Input
{
    public class InputCapture : MonoBehaviour
    {
        public float Horizontal { get; protected set; }
        public float Vertical { get; protected set; }
        public float MouseX { get; protected set; }
        public float MouseY { get; protected set; }

        public bool enableInput = true;
        
        public bool LeftMouseKeyDown { get; protected set; }
        public bool InteractKeyDown { get; protected set; }
        public bool JumpKeyDown { get; protected set; }
        public bool InventoryToggleKeyDown { get; protected set; }
    }
}