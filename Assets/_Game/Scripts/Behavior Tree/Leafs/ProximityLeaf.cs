using UnityEngine;

namespace Alchemist.AI
{
    public class ProximityLeaf : Node
    {
        private readonly string _triggerDistanceInformation;
        private readonly string _targetGameObjectInformation;

        public ProximityLeaf(NodeController nodeController, string triggerDistanceInformation, string targetGameObjectInformation) : base(nodeController)
        {
            _triggerDistanceInformation = triggerDistanceInformation;
            _targetGameObjectInformation = targetGameObjectInformation;
        }

        public override void Initialize()
        {
            State = NodeState.Running;
            HasInitialized = true;
        }
        
        public override void Tick()
        {
            OnNodeTick();
            
            var targetPosition = NodeController.Blackboard.Get<GameObject>(_targetGameObjectInformation).transform.position;
            State = Vector3.Distance(NodeController.transform.position, targetPosition) <=
                    NodeController.Blackboard.Get<float>(_triggerDistanceInformation) ? NodeState.Success : NodeState.Failure;
        }
    }
}