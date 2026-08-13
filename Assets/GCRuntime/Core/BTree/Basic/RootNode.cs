namespace GCRuntime.BTree
{
    public class RootNode : BTNode
    {
        public BTNode Child;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override BTState OnUpdate()
        {
            return Child.Update();
        }

        public override BTNode Clone()
        {
            RootNode rootNode = Instantiate(this);
            rootNode.Child = Child.Clone();
            return rootNode;
        }
    }
}
