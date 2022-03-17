namespace Alchemist.AI
{
    /// <summary>
    /// A node with only 1 child is called a Decorator.
    /// </summary>
    public class Decorator : Node
    {
        /// <summary>
        /// Child node of decorator
        /// </summary>
        protected Node Child;
        
        /// <summary>
        /// Base constructor for Decorator
        /// </summary>
        /// <param name="nodeController">Owner node controller</param>
        /// <param name="child">Child of this decorator node</param>
        public Decorator(NodeController nodeController, Node child) : base(nodeController)
        {
            Child = child;
        }

        /// <summary>
        /// Sets child of this decorator node.
        /// </summary>
        /// <param name="child">Child to set</param>
        public void SetChild(Node child) => Child = child;
    }
}
