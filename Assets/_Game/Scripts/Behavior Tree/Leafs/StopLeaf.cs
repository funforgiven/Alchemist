using UnityEngine.AI;

namespace Alchemist.AI
{
    public class StopLeaf : Node
    {
        private NavMeshAgent _agent;
        
        public StopLeaf(NodeController nodeController) : base(nodeController)
        {
            _agent = NodeController.GetComponent<NavMeshAgent>();
        }

        public override void Initialize()
        {
            State = NodeState.Running;
            HasInitialized = true;
        }

        public override void Tick()
        {
            OnNodeTick();
            
            _agent.isStopped = true;
            State =  _agent.isStopped ? NodeState.Success : NodeState.Failure;
        }
    }
}