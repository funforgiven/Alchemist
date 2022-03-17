using System.Collections.Generic;
using System.Linq;

namespace Alchemist.AI
{
    /// <summary>
    /// A node with n amount of children is called a Composite
    /// </summary>
    public class Composite : Node
    {
        protected readonly List<Node> Children;
        protected int CurrentChildIndex;
        
        /// <summary>
        /// Base constructor for Composite
        /// </summary>
        /// <param name="nodeController">Owner node controller</param>
        /// <param name="children">Children of this composite node</param>
        public Composite(NodeController nodeController, params Node[] children) : base(nodeController) 
        {
            Children = children.ToList();
            CurrentChildIndex = 0;
        }
    }
}

