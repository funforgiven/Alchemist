using UnityEngine;

namespace Alchemist.AI
{
    public class DebugLeaf : Node
    {

        private string _debugMessage;
        private NodeState _forceState;
        
        public DebugLeaf(NodeController nodeController, string debugMessage, NodeState forceState = NodeState.Success) : base(nodeController)
        {
            _debugMessage = debugMessage;
            _forceState = forceState;
            State = forceState;
        }

        public override void Initialize()
        {
            State = _forceState;
            HasInitialized = true;
        }

        public override void Tick()
        {
            OnNodeTick();
            
            Debug.Log($"<b>Debug Leaf:</b> { _debugMessage }");
        }
    }
}