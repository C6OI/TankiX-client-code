using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class UserEquipmentSystem : ECSSystem {
        [OnEventFire]
        public void sendUserEquipmentOnEnterLobby(NodeAddedEvent e, UserInMatchMakingLobby user, [Context] [JoinByUser] Weapon weapon, [Context] [JoinByUser] Hull hull,
            [JoinByUser] UserInMatchMakingLobby user2Lobby, [JoinByBattleLobby] SingleNode<BattleLobbyComponent> lobby) {
            SetEquipmentEvent setEquipmentEvent = new();
            setEquipmentEvent.WeaponId = weapon.marketItemGroup.Key;
            setEquipmentEvent.HullId = hull.marketItemGroup.Key;
            ScheduleEvent(setEquipmentEvent, lobby);
            user.Entity.RemoveComponentIfPresent<InitUserEquipmentComponent>();
            user.Entity.AddComponent<InitUserEquipmentComponent>();
        }

        [OnEventFire]
        public void sendUserEquipmentOnEnterLobby(NodeAddedEvent e, UserInMatchMakingLobbyPrototype user, [Context] [JoinByUser] Weapon weapon, [Context] [JoinByUser] Hull hull,
            [JoinByUser] UserInMatchMakingLobby user2Lobby, [JoinByBattleLobby] SingleNode<BattleLobbyComponent> lobby) {
            SetEquipmentEvent setEquipmentEvent = new();

            if (user.userUseItemsPrototype.Preset.HasComponent<PresetEquipmentComponent>()) {
                PresetEquipmentComponent component = user.userUseItemsPrototype.Preset.GetComponent<PresetEquipmentComponent>();
                setEquipmentEvent.WeaponId = Flow.Current.EntityRegistry.GetEntity(component.WeaponId).GetComponent<MarketItemGroupComponent>().Key;
                setEquipmentEvent.HullId = Flow.Current.EntityRegistry.GetEntity(component.HullId).GetComponent<MarketItemGroupComponent>().Key;
                ScheduleEvent(setEquipmentEvent, lobby);
            }
        }

        public class UserInMatchMakingLobby : Node {
            public BattleLobbyGroupComponent battleLobbyGroup;

            public SelfUserComponent selfUser;
            public UserComponent user;
        }

        public class UserInMatchMakingLobbyPrototype : UserInMatchMakingLobby {
            public InitUserEquipmentComponent initUserEquipment;

            public UserUseItemsPrototypeComponent userUseItemsPrototype;
        }

        public class Weapon : Node {
            public MarketItemGroupComponent marketItemGroup;

            public MountedItemComponent mountedItem;
            public WeaponItemComponent weaponItem;
        }

        public class Hull : Node {
            public MarketItemGroupComponent marketItemGroup;

            public MountedItemComponent mountedItem;
            public TankItemComponent tankItem;
        }

        public class InitUserEquipmentComponent : Component { }
    }
}