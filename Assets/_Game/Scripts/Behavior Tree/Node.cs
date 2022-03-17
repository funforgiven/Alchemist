namespace Alchemist.AI
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }
    
    /// <summary>
    /// Base class for node system. Every node in the system must derive
    /// from this class or its subclass.
    /// </summary>
    public class Node
    {
        // The state of the Node. Generally gets modified in Tick() method after
        // calculations.
        public NodeState State { get; protected set; }
        
        // Whether the Initialize() method needs to be called or not.
        // This is useful for resetting things like index.
        public bool HasInitialized { get; protected set; }
        
        // The NodeController that owns this node. Grants access to 
        // the attached GameObject.
        protected readonly NodeController NodeController;

        // Delegates for events //
        public delegate void OnNodeTickDelegate();
        
        // Events //
        public event OnNodeTickDelegate NodeTick;

        /// <summary>
        /// Base constructor for Node.
        /// </summary>
        /// <param name="nodeController">Owner node controller</param>
        protected Node(NodeController nodeController)
        {
            State = NodeState.Running;
            NodeController = nodeController;
        }

        /// <summary>
        /// Gets called when an object requires to get initialized.
        /// </summary>
        public virtual void Initialize() { }
        
        /// <summary>
        /// Gets called in update. All logic should be handled inside this
        /// method.
        /// </summary>
        public virtual void Tick() { }
        
        // Event Raisers //
        protected void OnNodeTick()
        {
            NodeTick?.Invoke();
        }
    }
}

