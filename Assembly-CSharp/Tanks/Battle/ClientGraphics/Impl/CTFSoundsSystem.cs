using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CTFSoundsSystem : ECSSystem {
        const string EFFECT_NAME = "Effects";

        [OnEventFire]
        public void FillFlagSounds(NodeAddedEvent e, SelfBattleUserNode battleUser, [Context] [JoinByBattle] CTFNode battle, [Context] [JoinByMap] MapNode map,
            [JoinAll] SingleNode<SoundListenerComponent> listener) {
            CTFAssetProxyBehaviour component = ((GameObject)battle.resourceData.Data).GetComponent<CTFAssetProxyBehaviour>();
            CTFSoundsComponent cTFSoundsComponent = new();
            GameObject gameObject = new("Effects");
            gameObject.transform.parent = listener.component.transform;
            cTFSoundsComponent.EffectRoot = gameObject;
            cTFSoundsComponent.FlagLost = CreateSound(component.flagLostSound, gameObject);
            cTFSoundsComponent.FlagReturn = CreateSound(component.flagReturnSound, gameObject);
            cTFSoundsComponent.FlagStole = CreateSound(component.flagStoleSound, gameObject);
            cTFSoundsComponent.FlagWin = CreateSound(component.flagWinSound, gameObject);
            listener.Entity.AddComponent(cTFSoundsComponent);
        }

        AudioSource CreateSound(GameObject prefab, GameObject effectRoot) {
            GameObject gameObject = Object.Instantiate(prefab);
            gameObject.transform.parent = effectRoot.transform;
            return gameObject.GetComponent<AudioSource>();
        }

        [OnEventFire]
        public void DestroySounds(NodeRemoveEvent e, MapNode map, [JoinAll] SingleNode<CTFSoundsComponent> listener) {
            ScheduleEvent<PrepareDestroyCTFSoundsEvent>(listener);
            listener.Entity.RemoveComponent<CTFSoundsComponent>();
        }

        [OnEventFire]
        public void PlayDropSound(FlagDropEvent e, SingleNode<FlagComponent> flag, [JoinAll] SingleNode<CTFSoundsComponent> ctfSoundsNode) {
            ctfSoundsNode.component.FlagLost.Play();
        }

        [OnEventFire]
        public void PlayReturnSound(FlagReturnEvent e, SingleNode<FlagComponent> flag, [JoinAll] SingleNode<CTFSoundsComponent> ctfSoundsNode) {
            ctfSoundsNode.component.FlagReturn.Play();
        }

        [OnEventFire]
        public void PlayReturnSound(FlagPickupEvent e, SingleNode<FlagComponent> flag, [JoinAll] SingleNode<CTFSoundsComponent> ctfSoundsNode) {
            ctfSoundsNode.component.FlagStole.Play();
        }

        [OnEventFire]
        public void PlayReturnSound(FlagDeliveryEvent e, SingleNode<FlagComponent> flag, [JoinAll] SingleNode<CTFSoundsComponent> ctfSoundsNode) {
            ctfSoundsNode.component.FlagWin.Play();
        }

        public class CTFNode : Node {
            public BattleGroupComponent battleGroup;
            public CTFComponent ctf;

            public MapGroupComponent mapGroup;

            public ResourceDataComponent resourceData;
        }

        public class MapNode : Node {
            public MapGroupComponent mapGroup;

            public MapInstanceComponent mapInstance;
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;
        }
    }
}