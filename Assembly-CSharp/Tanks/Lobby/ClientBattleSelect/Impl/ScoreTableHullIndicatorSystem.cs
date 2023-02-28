using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTableHullIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void SetHulls(NodeAddedEvent e, [Combine] HullIndicatorNode hullIndicator, [Context] [JoinByUser] HullNode hull) {
            SetHull(hullIndicator, hull);
        }

        void SetHull(HullIndicatorNode hullIndicator, HullNode hull) {
            hullIndicator.scoreTableHullIndicator.SetHullIcon(hull.marketItemGroup.Key);
        }

        public class HullIndicatorNode : Node {
            public ScoreTableHullIndicatorComponent scoreTableHullIndicator;

            public UserGroupComponent userGroup;
        }

        public class HullNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public TankComponent tank;
            public UserGroupComponent userGroup;
        }
    }
}