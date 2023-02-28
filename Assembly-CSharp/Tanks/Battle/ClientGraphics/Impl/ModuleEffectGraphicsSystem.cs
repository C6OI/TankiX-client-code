using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ModuleEffectGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void InitEffectRenderer(NodeAddedEvent e, SingleNode<EffectInstanceComponent> effect) {
            Renderer renderer = effect.component.GameObject.GetComponentsInChildren<Renderer>()[0];
            effect.Entity.AddComponent(new EffectRendererGraphicsComponent(renderer));
        }

        [OnEventFire]
        public void InitEnemyDMMaterial(NodeAddedEvent e, EffectGraphicsNode effect, [JoinByUser] SingleNode<BattleUserComponent> battleUser, [JoinByBattle] SingleNode<DMComponent> dm) {
            ApplyTeamColor(effect, battleUser.Entity.GetComponent<ColorInBattleComponent>().TeamColor, true);
        }

        [OnEventFire]
        public void InitTeamMaterial(NodeAddedEvent e, [Combine] EffectTeamGraphicsNode effect, [JoinByTank] [Context] [Combine] RemoteTankNode remoteTank,
            [JoinByTeam] SingleNode<ColorInBattleComponent> teamUIColor) {
            ApplyTeamColor(effect, teamUIColor.component.TeamColor, true);
        }

        [OnEventFire]
        public void InitTeamMaterial(NodeAddedEvent e, [Combine] EffectTeamGraphicsNode effect, [JoinByTank] [Context] [Combine] SelfTankNode selfTank,
            [JoinByTeam] SingleNode<ColorInBattleComponent> teamUIColor) {
            ApplyTeamColor(effect, teamUIColor.component.TeamColor, false);
        }

        [OnEventFire]
        public void Disable(RemoveEffectEvent e, EffectRemovingNode effect) {
            InstantiateEffectEffect(effect.effectInstance, effect.removeEffectGraphics.EffectPrefab, effect.removeEffectGraphics.EffectLifeTime, effect.removeEffectGraphics.Origin);
        }

        void ApplyTeamColor(EffectGraphicsNode effect, TeamColor color, bool useBlueMaterial) {
            Renderer renderer = effect.effectRendererGraphics.Renderer;
            EffectTeamGraphicsComponent effectTeamGraphics = effect.effectTeamGraphics;

            switch (color) {
                case TeamColor.BLUE:
                    renderer.material = !useBlueMaterial ? new Material(effectTeamGraphics.SelfMaterial) : new Material(effectTeamGraphics.BlueTeamMaterial);
                    break;

                case TeamColor.RED:
                    renderer.material = new Material(effectTeamGraphics.RedTeamMaterial);
                    break;
            }

            effect.Entity.AddComponent<EffectPaintedComponent>();
        }

        public static void InstantiateEffectEffect(EffectInstanceComponent effectInstanceComponent, GameObject prefab, float lifeTime, Vector3 yOrigin) {
            GameObject gameObject = effectInstanceComponent.GameObject;
            GameObject obj = Object.Instantiate(prefab, gameObject.transform.position + yOrigin, Quaternion.identity);
            Object.DestroyObject(obj, lifeTime);
        }

        public class EffectRemovingNode : Node {
            public EffectInstanceComponent effectInstance;
            public RemoveEffectGraphicsComponent removeEffectGraphics;
        }

        public class EffectGraphicsNode : Node {
            public EffectRendererGraphicsComponent effectRendererGraphics;

            public EffectTeamGraphicsComponent effectTeamGraphics;

            public TankGroupComponent tankGroup;
        }

        public class EffectTeamGraphicsNode : EffectGraphicsNode {
            public TeamGroupComponent teamGroup;
        }

        public class TankNode : Node {
            public TankComponent tank;

            public TankGroupComponent tankGroup;

            public TeamGroupComponent teamGroup;
        }

        public class RemoteTankNode : TankNode {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : TankNode {
            public SelfTankComponent selfTank;
        }
    }
}