namespace Tanks.Battle.ClientCore.API {
    public class TimerNode : DecoratorNode {
        public float Time;

        float timeToFinish;

        public override void Start() {
            timeToFinish = UnityEngine.Time.timeSinceLevelLoad + Time;
        }

        public override TreeNodeState Running() {
            if (UnityEngine.Time.timeSinceLevelLoad > timeToFinish) {
                return TreeNodeState.SUCCESS;
            }

            Child.Update();
            return TreeNodeState.RUNNING;
        }

        public override void End() { }
    }
}