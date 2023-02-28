using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class CustomBattlesScreenSystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void InitModes(NodeAddedEvent e, CustomBattlesScreenNode screen, [JoinAll] ICollection<ArcadeModeNode> modes, [JoinAll] SelfUserRankNode selfUserRank) {
            if (modes.Count == 0) {
                return;
            }

            GameObject modePrefab = screen.customBattlesScreen.GameModeItemPrefab;

            List<ArcadeModeNode> source = (from m in modes
                                           where m.matchMakingModeActivation.Active
                                           orderby m.orderItem.Index
                                           select m).ToList();

            int userRank = selfUserRank.userRank.Rank;
            source = source.OrderByDescending(i => i.orderItem.Index).ToList();

            source.ForEach(delegate(ArcadeModeNode mode) {
                if (CanShowByRestrictions(mode.matchMakingModeRestrictions, userRank)) {
                    CreateActiveModeInstance(mode.Entity, modePrefab, screen.customBattlesScreen.GameModesContainer);
                }
            });

            List<ArcadeModeNode> list = (from m in modes
                                         where !m.matchMakingModeActivation.Active
                                         orderby m.orderItem.Index
                                         select m).ToList();

            list.ForEach(delegate(ArcadeModeNode mode) {
                if (CanShowByRestrictions(mode.matchMakingModeRestrictions, userRank)) {
                    CreateInactiveModeInstance(mode.Entity, modePrefab, screen.customBattlesScreen.GameModesContainer);
                }
            });

            screen.customBattlesScreen.ScrollToTheLeft();
        }

        bool CanShowByRestrictions(MatchMakingModeRestrictionsComponent restrictions, int userRank) =>
            userRank <= restrictions.MaximalShowRank && userRank >= restrictions.MinimalShowRank;

        void CreateActiveModeInstance(Entity mode, GameObject prefab, GameObject container) {
            CreateModeInstance(mode, prefab, container).transform.SetAsFirstSibling();
        }

        void CreateInactiveModeInstance(Entity mode, GameObject prefab, GameObject container) {
            CreateModeInstance(mode, prefab, container).transform.SetAsLastSibling();
        }

        GameObject CreateModeInstance(Entity mode, GameObject prefab, GameObject container) {
            GameObject gameObject = Object.Instantiate(prefab);
            gameObject.transform.SetParent(container.transform, false);
            EntityBehaviour component = gameObject.GetComponent<EntityBehaviour>();
            component.BuildEntity(mode);
            return gameObject;
        }

        [OnEventFire]
        public void RemoveModes(NodeRemoveEvent e, CustomBattlesScreenNode screen, [JoinAll] [Combine] ArcadeModeGUINode gameMode) {
            Object.Destroy(gameMode.gameModeSelectButton.gameObject);
        }

        public class CustomBattlesScreenNode : Node {
            public CustomBattlesScreenComponent customBattlesScreen;
        }

        [Not(typeof(MatchMakingDefaultModeComponent))]
        public class ArcadeModeNode : Node {
            public MatchMakingArcadeModeComponent matchMakingArcadeMode;

            public MatchMakingModeComponent matchMakingMode;

            public MatchMakingModeActivationComponent matchMakingModeActivation;

            public MatchMakingModeRestrictionsComponent matchMakingModeRestrictions;

            public OrderItemComponent orderItem;
        }

        public class ArcadeModeGUINode : ArcadeModeNode {
            public GameModeSelectButtonComponent gameModeSelectButton;
        }

        public class MountedHullNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public MountedItemComponent mountedItem;

            public TankItemComponent tankItem;
        }

        public class SelfUserRankNode : Node {
            public SelfUserComponent selfUser;

            public UserRankComponent userRank;
        }
    }
}