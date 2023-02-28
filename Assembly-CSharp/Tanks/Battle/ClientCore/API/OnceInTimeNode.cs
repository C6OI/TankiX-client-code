namespace Tanks.Battle.ClientCore.API {
    public class OnceInTimeNode : CompositeNode {
        public float Time;

        float timeToFinish;

        public override void Start() {
            timeToFinish = UnityEngine.Time.timeSinceLevelLoad + Time;
        }

        public override TreeNodeState Running() {
            if (UnityEngine.Time.timeSinceLevelLoad > timeToFinish) {
                for (int i = 0; i < children.Count; i++) {
                    children[i].Update();
                }

                return TreeNodeState.SUCCESS;
            }

            return TreeNodeState.RUNNING;
        }

        public override void End() { }
    }
}