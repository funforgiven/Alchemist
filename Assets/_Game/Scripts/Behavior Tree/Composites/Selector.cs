using System;

namespace Alchemist.AI
{
    /// <summary>
    /// A selector will return success if any of its children returns success and
    /// will not process any other children.
    /// </summary>
    public class Selector : Composite
    {
        /// <summary>
        /// Base constructor for Selector.
        /// </summary>
        /// <param name="nodeController">Owner node controller</param>
        /// <param name="children">Children of this selector node</param>
        public Selector(NodeController nodeController, params Node[] children) : base(nodeController, children)
        {
        }

        public override void Initialize()
        {
            CurrentChildIndex = 0;
            State = NodeState.Running;
            HasInitialized = true;
        }

        public override void Tick()
        {
            OnNodeTick();
            
            var currentChild = Children[CurrentChildIndex];
            
            if(!currentChild.HasInitialized)
                currentChild.Initialize();
            
            currentChild.Tick();
            var childState = currentChild.State;

            switch (childState)
            {
                case NodeState.Running:
                    State = NodeState.Running;
                    break;
                case NodeState.Success:
                    State = NodeState.Success;
                    HasInitialized = false;
                    break;
                case NodeState.Failure:
                    State = NodeState.Running;
                    if (CurrentChildIndex == Children.Count - 1)
                    {
                        State = NodeState.Failure;
                        HasInitialized = false;
                    }
                    else
                        CurrentChildIndex++;
                    break;
                default:
                    throw new Exception("There is no such NodeState!");
            }
        }
    }
}
