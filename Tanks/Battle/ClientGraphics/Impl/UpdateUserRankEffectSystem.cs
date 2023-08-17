using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateUserRankEffectSystem : ECSSystem {
        [OnEventFire]
        public void PlayUpdateUserRankEffect(UpdateRankEvent e, UserRankNode user, [JoinByUser] NotReadyTankNode tank) =>
            tank.Entity.AddComponent<UpdateUserRankEffectReadyComponent>();

        [OnEventFire]
        public void ScheduleUpdateUserRankEffect(NodeAddedEvent evt, DeadTankNode tank, [JoinByUser] UserRankNode user) =>
            ScheduleUpdateUserRankEffect(tank, user);

        [OnEventFire]
        public void ScheduleUpdateUserRankEffect(NodeAddedEvent evt, SemiActiveTankNode tank,
            [JoinByUser] UserRankNode user) => ScheduleUpdateUserRankEffect(tank, user);

        [OnEventFire]
        public void ScheduleUpdateUserRankEffect(NodeAddedEvent evt, ActiveTankNode tank, [JoinByUser] UserRankNode user) =>
            ScheduleUpdateUserRankEffect(tank, user);

        void ScheduleUpdateUserRankEffect(ReadyTankNode tank, UserRankNode user) =>
            NewEvent<UpdateUserRankEffectEvent>().AttachAll(tank, user).Schedule();

        [OnEventFire]
        public void PlayUpdateRankEffect(UpdateUserRankEffectEvent evt, ReadyTankNode tank, UserRankNode user,
            [JoinByUser] BattleUserNode battleUser) {
            GameObject effectPrefab = tank.updateUserRankEffect.EffectPrefab;
            GameObject gameObject = Object.Instantiate(effectPrefab);
            Transform transform = gameObject.transform;
            Transform transform2 = tank.tankVisualRoot.transform;
            Transform instanceRootTransform = new GameObject("RankEffectRoot").transform;
            instanceRootTransform.parent = transform2;
            instanceRootTransform.localPosition = Vector3.zero;
            instanceRootTransform.localRotation = Quaternion.identity;
            instanceRootTransform.localScale = Vector3.one;

            UpdateUserRankTransformBehaviour updateUserRankTransformBehaviour =
                instanceRootTransform.gameObject.AddComponent<UpdateUserRankTransformBehaviour>();

            updateUserRankTransformBehaviour.Init();
            transform.parent = instanceRootTransform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            UpdateRankEffectParticleMovement[] componentsInChildren =
                gameObject.GetComponentsInChildren<UpdateRankEffectParticleMovement>(true);

            componentsInChildren.ForEach(delegate(UpdateRankEffectParticleMovement p) {
                p.parent = instanceRootTransform;
            });

            UpdateRankEffectSettings componentInChildren =
                instanceRootTransform.GetComponentInChildren<UpdateRankEffectSettings>(true);

            componentInChildren.icon.SetRank(user.userRank.Rank + 1);
            gameObject.SetActive(true);
            Object.DestroyObject(instanceRootTransform.gameObject, componentInChildren.DestroyTimeDelay);

            NewEvent<UpdateRankEffectFinishedEvent>().Attach(battleUser)
                .ScheduleDelayed(tank.updateUserRankEffect.FinishEventTime);

            if (!tank.Entity.HasComponent<UpdateUserRankEffectInstantiatedComponent>()) {
                tank.Entity.AddComponent<UpdateUserRankEffectInstantiatedComponent>();
            }

            tank.Entity.RemoveComponent<UpdateUserRankEffectReadyComponent>();
        }

        [OnEventFire]
        public void ReleaseEffectsOnDeath(NodeRemoveEvent e, TankWithEffectsNode tank,
            [JoinAll] SingleNode<MapInstanceComponent> map) => tank.tankVisualRoot
            .GetComponentsInChildren<UpdateUserRankTransformBehaviour>(true).ForEach(
                delegate(UpdateUserRankTransformBehaviour c) {
                    c.transform.SetParent(map.component.SceneRoot.transform, true);
                });

        public class UserRankNode : Node {
            public UserGroupComponent userGroup;
            public UserRankComponent userRank;
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankComponent tank;

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

        public class BattleUserNode : Node {
            public BattleUserComponent battleUser;

            public UserGroupComponent userGroup;
        }
    }
}