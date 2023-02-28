using System;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleSelectLoadSystem : ECSSystem {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        [OnEventComplete]
        public void AttachScreenToTargeBattle(ShowScreenEvent e, SingleNode<BattleGroupComponent> node, [JoinAll] UngroupedBattleSelectLoadScreenNode screen) {
            screen.Entity.AddComponent(new BattleGroupComponent(node.component.Key));
        }

        [OnEventFire]
        public void ShowBattleSelect(NodeAddedEvent e, BattleSelectLoadScreenNode screen, UserReadyForLobbyNode user, [JoinAll] SelfUserNode selfUser,
            [JoinByUser] Optional<MountedWeaponNode> weapon, [JoinAll] SelfUserNode selfUser2, [JoinByUser] Optional<MountedHullNode> hull) {
            if (GetEffectiveLevel(weapon, hull) < BattleSelectSystem.TRAIN_BATTLE_MAXIMAL_RANK) {
                ScheduleEvent<ShowScreenNoAnimationEvent<MainScreenComponent>>(user);
            } else {
                ScheduleEvent(new ShowBattleEvent(screen.battleGroup.Key), EngineService.EntityStub);
            }
        }

        int GetEffectiveLevel(Optional<MountedWeaponNode> weapon, Optional<MountedHullNode> hull) {
            if (weapon.IsPresent() && hull.IsPresent()) {
                return Math.Max(weapon.Get().upgradeLevelItem.Level, hull.Get().upgradeLevelItem.Level);
            }

            return BattleSelectSystem.TRAIN_BATTLE_MAXIMAL_RANK;
        }

        public class UserReadyForLobbyNode : Node {
            public SelfUserComponent selfUser;

            public UserRankComponent userRank;

            public UserReadyForLobbyComponent userReadyForLobby;
        }

        public class BattleSelectLoadScreenNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleSelectLoadScreenComponent battleSelectLoadScreen;
        }

        public class MountedWeaponNode : Node {
            public MountedItemComponent mountedItem;

            public UpgradeLevelItemComponent upgradeLevelItem;
            public WeaponItemComponent weaponItem;
        }

        public class MountedHullNode : Node {
            public MountedItemComponent mountedItem;
            public TankItemComponent tankItem;

            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
        }

        [Not(typeof(BattleGroupComponent))]
        public class UngroupedBattleSelectLoadScreenNode : Node {
            public BattleSelectLoadScreenComponent battleSelectLoadScreen;
        }
    }
}