using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateUserRankEffectSystem : ECSSystem {
        [OnEventFire]
        public void PlayUpdateUserRankEffect(UpdateRankEvent e, UserRankNode user, [JoinByUser] NotReadyTankNode tank) {
            tank.Entity.AddComponent<UpdateUserRankEffectReadyComponent>();
        }

        [OnEventFire]
        public void ScheduleUpdateUserRankEffect(NodeAddedEvent evt, DeadTankNode tank, [JoinByUser] UserRankNode user) {
            ScheduleUpdateUserRankEffect(tank, user);
        }

        [OnEventFire]
        public void ScheduleUpdateUserRankEffect(NodeAddedEvent evt, SemiActiveTankNode tank, [JoinByUser] UserRankNode user) {
            ScheduleUpdateUserRankEffect(tank, user);
        }

        [OnEventFire]
        public void ScheduleUpdateUserRankEffect(NodeAddedEvent evt, ActiveTankNode tank, [JoinByUser] UserRankNode user) {
            ScheduleUpdateUserRankEffect(tank, user);
        }

        void ScheduleUpdateUserRankEffect(ReadyTankNode tank, UserRankNode user) {
            NewEvent<UpdateUserRankEffectEvent>().AttachAll(tank, user).Schedule();
        }

        [OnEventFire]
        public void PlayUpdateRankEffect(UpdateUserRankEffectEvent evt, ReadyTankNode tank, UserRankNode user, [JoinByUser] BattleUserNode battleUser) {
            GameObject effectPrefab = tank.updateUserRankEffect.EffectPrefab;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = effectPrefab;
            getInstanceFromPoolEvent.AutoRecycleTime = effectPrefab.GetComponent<UpdateRankEffectSettings>().DestroyTimeDelay;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, tank);
            Transform instance = getInstanceFromPoolEvent2.Instance;
            GameObject gameObject = instance.gameObject;
            Transform transform = tank.tankVisualRoot.transform;
            Transform transform2 = new GameObject("RankEffectRoot").transform;
            transform2.parent = transform;
            transform2.localPosition = Vector3.zero;
            transform2.localRotation = Quaternion.identity;
            transform2.localScale = Vector3.one;
            UpdateUserRankTransformBehaviour updateUserRankTransformBehaviour = transform2.gameObject.AddComponent<UpdateUserRankTransformBehaviour>();
            updateUserRankTransformBehaviour.Init();
            instance.parent = transform2;
            instance.localPosition = Vector3.zero;
            instance.localRotation = Quaternion.identity;
            instance.localScale = Vector3.one;
            UpdateRankEffectParticleMovement[] componentsInChildren = gameObject.GetComponentsInChildren<UpdateRankEffectParticleMovement>(true);
            UpdateRankEffectParticleMovement[] array = componentsInChildren;

            foreach (UpdateRankEffectParticleMovement updateRankEffectParticleMovement in array) {
                updateRankEffectParticleMovement.parent = transform2;
            }

            UpdateRankEffectSettings componentInChildren = transform2.GetComponentInChildren<UpdateRankEffectSettings>(true);
            componentInChildren.icon.SetRank(user.userRank.Rank);
            gameObject.SetActive(true);
            NewEvent<UpdateRankEffectFinishedEvent>().Attach(battleUser).ScheduleDelayed(tank.updateUserRankEffect.FinishEventTime);

            if (!tank.Entity.HasComponent<UpdateUserRankEffectInstantiatedComponent>()) {
                tank.Entity.AddComponent<UpdateUserRankEffectInstantiatedComponent>();
            }

            tank.Entity.RemoveComponent<UpdateUserRankEffectReadyComponent>();
        }

        [OnEventFire]
        public void ReleaseEffectsOnDeath(NodeRemoveEvent e, TankIncarnationNode tankIncarnation, [JoinByTank] TankWithEffectsNode tank, [JoinAll] SingleNode<MapInstanceComponent> map) {
            tank.tankVisualRoot.GetComponentsInChildren<UpdateUserRankTransformBehaviour>(true).ForEach(delegate(UpdateUserRankTransformBehaviour c) {
                c.transform.SetParent(map.component.SceneRoot.transform, true);
            });
        }

        public class UserRankNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankComponent tank;

            public TankGroupComponent tankGroup;

            public TankVisualRootComponent tankVisualRoot;

            public UpdateUserRankEffectComponent updateUserRankEffect;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(UpdateUserRankEffectReadyComponent))]
        public class NotReadyTankNode : TankNode { }

        public class ReadyTankNode : TankNode {
            public UpdateUserRankEffectReadyComponent updateUserRankEffectReady;
        }

        public class DeadTankNode : ReadyTankNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class SemiActiveTankNode : ReadyTankNode {
            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class ActiveTankNode : ReadyTankNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class TankWithEffectsNode : TankNode {
            public UpdateUserRankEffectInstantiatedComponent updateUserRankEffectInstantiated;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }

        public class BattleUserNode : Node {
            public BattleUserComponent battleUser;

            public UserGroupComponent userGroup;
        }
    }
}