using System;

namespace Alchemist.AI
{
    /// <summary>
    /// Executes child node and returns succeed whether the child has failed or not.
    /// </summary>
    public class Succeeder : Decorator
    {
        /// <summary>
        /// Base constructor of Succeeder
        /// </summary>
        /// <param name="nodeController">Owner node controller</param>
        /// <param name="child">Child of this Succeeder node</param>
        public Succeeder(NodeController nodeController, Node child) : base(nodeController, child)
        {
        }

        public override void Initialize()
        {
            State = NodeState.Running;
            HasInitialized = true;
        }

        public override void Tick()
        {
            OnNodeTick();
            
            if(!Child.HasInitialized)
                Child.Initialize();

            Child.Tick();
            var childState = Child.State;
            switch (childState)
            {
                case NodeState.Running:
                    State = NodeState.Running;    
                    break;
                case NodeState.Success:
                case NodeState.Failure:
                    State = NodeState.Success;
                    HasInitialized = false;
                    break;
                default:
                    throw new Exception("There is no such NodeState!");
            }
        }
    }
}

