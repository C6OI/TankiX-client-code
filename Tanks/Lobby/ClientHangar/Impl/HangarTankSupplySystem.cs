using Lobby.ClientNavigation.API;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarTankSupplySystem : HangarTankBaseSystem {
        [OnEventFire]
        public void SwitchSoundListenerToBattleState(NodeAddedEvent evt, GarageSuppliesActiveScreenScreenNode screen,
            SingleNode<SoundListenerComponent> listener) =>
            ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleState>>(listener);

        [OnEventFire]
        public void SwitchSoundListenerToLobbyState(NodeRemoveEvent evt, GarageSuppliesActiveScreenScreenNode screen,
            [JoinAll] SingleNode<SoundListenerComponent> listener) =>
            ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerLobbyState>>(listener);

        [OnEventComplete]
        public void EnableSupplyAnimation(NodeAddedEvent e, SingleNode<SoundListenerComponent> listener, HangarNode hangar,
            SupplyItemPreviewNode supply, WeaponSkinItemPreviewLoadedNode weaponSkin,
            [JoinByParentGroup] [Context] WeaponItemPreviewNode weaponItem, HullSkinItemPreviewLoadedNode tankSkin,
            [JoinByParentGroup] [Context] TankItemPreviewNode tankItem) {
            GameObject instance = tankSkin.hangarItemPreview.Instance;
            GameObject instance2 = weaponSkin.hangarItemPreview.Instance;
            listener.component.transform.position = instance.transform.position;
            listener.component.transform.rotation = instance.transform.rotation;

            if (supply.supplyType.Type == SupplyType.ARMOR) {
                instance.GetComponentInChildren<DoubleArmorEffectComponent>().Play();
            }

            if (supply.supplyType.Type == SupplyType.DAMAGE) {
                instance2.GetComponentInChildren<DoubleDamageEffectComponent>().Play();
            }

            if (supply.supplyType.Type == SupplyType.SPEED) {
                instance.GetComponentInChildren<NitroEffectComponent>().Play();
            }
        }

        [OnEventFire]
        public void DisableSupplyAnimation(NodeRemoveEvent e, HangarNode hangar, SupplyItemPreviewNode supply,
            WeaponSkinItemPreviewLoadedNode weaponSkin, [JoinByParentGroup] [Context] WeaponItemPreviewNode weaponItem,
            HullSkinItemPreviewLoadedNode tankSkin, [Context] [JoinByParentGroup] TankItemPreviewNode tankItem) {
            GameObject instance = tankSkin.hangarItemPreview.Instance;
            GameObject instance2 = weaponSkin.hangarItemPreview.Instance;

            if (supply.supplyType.Type == SupplyType.ARMOR) {
                instance.GetComponentInChildren<DoubleArmorEffectComponent>().Stop();
            }

            if (supply.supplyType.Type == SupplyType.DAMAGE) {
                instance2.GetComponentInChildren<DoubleDamageEffectComponent>().Stop();
            }

            if (supply.supplyType.Type == SupplyType.SPEED) {
                instance.GetComponentInChildren<NitroEffectComponent>().Stop();
            }
        }

        [OnEventFire]
        public void ResetSupplyAnimationOnSuppliesScreenHidding(NodeAddedEvent e,
            GarageSuppliesScreenHidingNode suppliesScreenHiding, [JoinAll] SupplyItemPreviewNode supply) =>
            supply.Entity.RemoveComponent<HangarItemPreviewComponent>();

        public class SupplyItemPreviewNode : HangarPreviewItemNode {
            public SupplyTypeComponent supplyType;
        }

        public class GarageSuppliesScreenHidingNode : Node {
            public GarageSuppliesScreenComponent garageSuppliesScreen;
            public ScreenHidingComponent screenHiding;
        }

        public class GarageSuppliesActiveScreenScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public GarageSuppliesScreenComponent garageSuppliesScreen;
        }
    }
}