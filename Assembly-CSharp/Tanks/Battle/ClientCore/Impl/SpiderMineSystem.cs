using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class SpiderMineSystem : ECSSystem {
        [OnEventFire]
        public void Instantiate(NodeAddedEvent e, [Combine] SpiderInstantiatedNode spider, SingleNode<MapInstanceComponent> map) {
            GameObject gameObject = spider.effectInstance.GameObject;
            spider.Entity.AddComponent(new RigidbodyComponent(gameObject.GetComponent<Rigidbody>()));
            NewEvent(new InitMinePlacingTransformEvent(spider.effectInstance.GameObject.transform.position)).AttachAll(spider, map).Schedule();
        }

        [OnEventFire]
        public void InitSelf(NodeAddedEvent e, SpiderSelfActiveNode spider) {
            spider.Entity.AddComponent<UnitTargetingComponent>();
            GameObject gameObject = spider.rigidbody.Rigidbody.transform.GetChild(0).GetComponentInChildren<Rigidbody>(true).gameObject;
            MinePhysicsTriggerBehaviour minePhysicsTriggerBehaviour = gameObject.AddComponent<MinePhysicsTriggerBehaviour>();
            minePhysicsTriggerBehaviour.TriggerEntity = spider.Entity;
        }

        [OnEventFire]
        public void AddTeamEvaluator(NodeAddedEvent evt, [Combine] SpiderSelfActiveNode spider, [JoinByBattle] SingleNode<TeamBattleComponent> battle) {
            spider.Entity.AddComponent<TeamTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, [Combine] SpiderSelfActiveNode spider, [JoinByBattle] SingleNode<CTFComponent> battle) {
            spider.Entity.AddComponent<CTFTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void Activate(NodeAddedEvent e, SpiderActiveNode activeSpider) {
            activeSpider.spiderAnimator.StartActivation();
        }

        [OnEventFire]
        public void AcceptNewTarget(NodeAddedEvent e, SpiderActiveWithTargetNode activeSpider) {
            Entity target = activeSpider.unitTarget.Target;

            if (target.HasComponent<RigidbodyComponent>()) {
                Rigidbody rigidbody = target.GetComponent<RigidbodyComponent>().Rigidbody;
                SpiderMineConfigComponent spiderMineConfig = activeSpider.spiderMineConfig;
                SpiderAnimatorComponent spiderAnimator = activeSpider.spiderAnimator;
                spiderAnimator.Speed = spiderMineConfig.Speed;
                spiderAnimator.Acceleration = spiderMineConfig.Acceleration;
                spiderAnimator.SetTarget(rigidbody);
                spiderAnimator.StartRuning();
            }
        }

        [OnEventFire]
        public void RemoveTarget(NodeRemoveEvent e, SpiderActiveWithTargetNode activeSpider) {
            SpiderAnimatorComponent spiderAnimator = activeSpider.spiderAnimator;
            spiderAnimator.SetTarget(null);
            spiderAnimator.StartIdle();
        }

        [OnEventFire]
        public void UpdateTarget(UpdateEvent e, SpiderActiveWithTargetNode activeSpider, [JoinByTank] SingleNode<SelfTankComponent> tank) {
            Entity target = activeSpider.unitTarget.Target;

            if (!target.Alive || !target.HasComponent<TankActiveStateComponent>()) {
                activeSpider.Entity.RemoveComponent<UnitTargetComponent>();
            }
        }

        [OnEventFire]
        public void Explosion(TriggerEnterEvent e, RemoteTankNode targetTank, SpiderActiveNode spider, [JoinByTank] SingleNode<SelfTankComponent> tank) {
            MineUtil.ExecuteSplashExplosion(spider.Entity, tank.Entity, spider.rigidbody.Rigidbody.transform.position);
        }

        [OnEventComplete]
        public void StopUnit(SelfSplashHitEvent e, SpiderActiveNode spider) {
            spider.Entity.RemoveComponent<UnitReadyComponent>();
        }

        public class SpiderEffectNode : Node {
            public MineConfigComponent mineConfig;
            public SpiderMineEffectComponent spiderMineEffect;

            public UnitMoveComponent unitMove;
        }

        public class SpiderInstantiatedNode : SpiderEffectNode {
            public EffectInstanceComponent effectInstance;
        }

        public class SpiderActiveNode : SpiderInstantiatedNode {
            public EffectActiveComponent effectActive;

            public RigidbodyComponent rigidbody;

            public SpiderAnimatorComponent spiderAnimator;
            public UnitReadyComponent unitReady;
        }

        public class SpiderSelfActiveNode : SpiderActiveNode {
            public SelfComponent self;
        }

        public class SpiderActiveWithTargetNode : SpiderActiveNode {
            public SpiderMineConfigComponent spiderMineConfig;
            public UnitTargetComponent unitTarget;
        }

        public class RemoteTankNode : Node {
            public EnemyComponent enemy;
            public RemoteTankComponent remoteTank;

            public RigidbodyComponent rigidbody;
        }
    }
}