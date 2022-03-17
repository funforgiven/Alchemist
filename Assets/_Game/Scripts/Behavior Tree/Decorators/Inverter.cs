using System;

namespace Alchemist.AI
{
    /// <summary>
    /// A child fails and it will return success to its parent,
    /// or a child succeeds and it will return failure to the parent.
    /// </summary>
    public class Inverter : Decorator
    {
        /// <summary>
        /// Base constructor for Inverter
        /// </summary>
        /// <param name="nodeController">Owner node controller</param>
        /// <param name="child">Child of this inverter node</param>
        public Inverter(NodeController nodeController, Node child) : base(nodeController, child)
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
                    State = NodeState.Failure;
                    HasInitialized = false;
                    break;
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

