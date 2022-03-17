namespace Alchemist.AI
{
    public class Root : Decorator
    {
        public Root(NodeController nodeController, Node child) : base(nodeController, child)
        {
        }

        public override void Tick()
        {
            OnNodeTick();
            
            if(!Child.HasInitialized)
                Child.Initialize();

            Child.Tick();
        }
    }
}