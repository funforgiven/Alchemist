using UnityEngine;
using UnityEngine.AI;

namespace Alchemist.AI
{
    public class MoveLeaf : Node
    {
        private readonly NavMeshAgent _agent;

        private readonly string _speedInformation;
        private readonly string _destinationInformation;
        private readonly float _offset;
        
        public MoveLeaf(NodeController nodeController, string speedInformation, string destinationInformation, float offset = 0f) : base(nodeController)
        {
            _agent = NodeController.GetComponent<NavMeshAgent>();
            _offset = offset;

            _speedInformation = speedInformation;
            _destinationInformation = destinationInformation;
            
            _agent.speed = NodeController.Blackboard.Get<float>(_speedInformation);
        }

        public override void Initialize()
        {
            State = NodeState.Running;
            HasInitialized = true;
        }

        public override void Tick()
        {
            OnNodeTick();
            
            _agent.isStopped = false;
            _agent.stoppingDistance = _offset;
            _agent.speed = NodeController.Blackboard.Get<float>(_speedInformation);

            var targetPosition = NodeController.Blackboard.Get<Vector3>(_destinationInformation);

            State =  _agent.SetDestination(targetPosition) ? NodeState.Success : NodeState.Failure;
        }
    }
}