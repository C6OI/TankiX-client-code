using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class StreamHitLoadCheckerSystem : ECSSystem {
        [OnEventFire]
        public void TryMarkTargetLoaded(NodeAddedEvent e, TankNode tank,
            [JoinByBattle] ICollection<StreamHitNode> streamHits) {
            foreach (StreamHitNode streamHit in streamHits) {
                AddIfMatches(tank, streamHit);
            }
        }

        [OnEventFire]
        public void TryMarkTargetLoaded(NodeAddedEvent e, StreamHitNode streamHit,
            [JoinByBattle] ICollection<TankNode> tanks) {
            foreach (TankNode tank in tanks) {
                AddIfMatches(tank, streamHit);
            }
        }

        void AddIfMatches(TankNode tank, StreamHitNode streamHit) {
            StreamHitComponent streamHit2 = streamHit.streamHit;

            if (streamHit2.TankHit != null && streamHit2.TankHit.Entity == tank.Entity) {
                streamHit.Entity.AddComponent<StreamHitTargetLoadedComponent>();
            }
        }

        [OnEventComplete]
        public void Remove(NodeRemoveEvent e, LoadedHitForNRNode nr, [JoinSelf] LoadedHitNode streamHit) =>
            streamHit.Entity.RemoveComponent<StreamHitTargetLoadedComponent>();

        public class TankNode : Node {
            public AssembledTankComponent assembledTank;

            public BattleGroupComponent battleGroup;
            public TankComponent tank;
        }

        public class StreamHitNode : Node {
            public BattleGroupComponent battleGroup;
            public StreamHitComponent streamHit;
        }

        public class LoadedHitNode : Node {
            public BattleGroupComponent battleGroup;
            public StreamHitComponent streamHit;

            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
        }

        public class LoadedHitForNRNode : Node {
            public BattleGroupComponent battleGroup;
            public StreamHitComponent streamHit;
        }
    }
}