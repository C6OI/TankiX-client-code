namespace Tanks.Battle.ClientCore.API {
    public class ParallelNode : CompositeNode {
        public override TreeNodeState Running() {
            for (int i = 0; i < children.Count; i++) {
                children[i].Update();
            }

            return TreeNodeState.SUCCESS;
        }
    }
}