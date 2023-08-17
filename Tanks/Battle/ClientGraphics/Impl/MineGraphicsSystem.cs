using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MineGraphicsSystem : ECSSystem {
        static readonly float MINE_ACTIVATION_TIME = 1f;

        static readonly Vector4 MINE_ACTIVATION_COLOR = new(1f, 1f, 1f, 1f);

        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void InitRenderer(NodeAddedEvent e, SingleNode<MineInstanceComponent> mine) {
            Renderer renderer = GraphicsBuilderUtils.GetRenderer(mine.component.GameObject);
            mine.Entity.AddComponent(new MineRendererGraphicsComponent(renderer));
            renderer.material.SetColor("_Color", MINE_ACTIVATION_COLOR);
        }

        [OnEventFire]
        public void InitEnemyDMMaterial(NodeAddedEvent e, EnemyMineNode mine, [JoinByBattle] SingleNode<DMComponent> dm) {
            Renderer renderer = mine.mineRendererGraphics.Renderer;
            renderer.material = new Material(mine.mineTeamGraphics.EnemyMaterial);
            renderer.material.SetColor("_Color", MINE_ACTIVATION_COLOR);
        }

        [OnEventFire]
        public void InitTeamMaterial(NodeAddedEvent e, TeamMineNode mine,
            [JoinByTeam] SingleNode<TeamColorComponent> teamColor) {
            Renderer renderer = mine.mineRendererGraphics.Renderer;
            MineTeamGraphicsComponent mineTeamGraphics = mine.mineTeamGraphics;

            switch (teamColor.component.TeamColor) {
                case TeamColor.BLUE:
                    renderer.material = new Material(mineTeamGraphics.BlueTeamMaterial);
                    break;

                case TeamColor.RED:
                    renderer.material = new Material(mineTeamGraphics.RedTeamMaterial);
                    break;
            }

            renderer.material.SetColor("_Color", MINE_ACTIVATION_COLOR);
        }

        [OnEventFire]
        public void Explosion(MineExplosionEvent e, MineExplosionNode mine) {
            GameObject gameObject = mine.mineInstance.GameObject;
            GameObject effectPrefab = mine.mineExplosionGraphics.EffectPrefab;
            Object obj = Object.Instantiate(effectPrefab, gameObject.transform.position + Vector3.up, Quaternion.identity);
            Object.Destroy(obj, 2f);
        }

        [OnEventFire]
        public void Disable(MineDisableEvent e, MineDisableNode mine) {
            GameObject gameObject = mine.mineInstance.GameObject;
            GameObject effectPrefab = mine.mineDisableGraphics.EffectPrefab;

            GameObject obj =
                Object.Instantiate(effectPrefab, gameObject.transform.position + Vector3.up, Quaternion.identity);

            Object.Destroy(obj, 2f);
        }

        [OnEventFire]
        public void Activation(MineActivationEvent e, MineActiveNode mine) =>
            mine.Entity.AddComponent(new MineActivationGraphicsComponent(UnityTime.time));

        [OnEventFire]
        public void ActivationEffect(TimeUpdateEvent e, MineActivationNode mine) {
            MineConfigComponent mineConfig = mine.mineConfig;
            float num = UnityTime.time - mine.mineActivationGraphics.ActivationStartTime;
            float num2 = num / (MINE_ACTIVATION_TIME * 0.5f);

            if (num2 > 1f) {
                num2 = Math.Max(0f, 2f - num2);
            }

            Material material = mine.mineRendererGraphics.Renderer.material;
            material.SetColor("_Color", MINE_ACTIVATION_COLOR);
            material.SetFloat("_ColorLerp", num2);

            if (num > MINE_ACTIVATION_TIME) {
                mine.Entity.RemoveComponent<MineActivationGraphicsComponent>();
            }
        }

        [OnEventFire]
        public void AlfaBlendByDistance(TimeUpdateEvent e, MineBlendNode mine, [JoinByBattle] SelfTankNode selfTank) {
            if (!mine.Entity.HasComponent<MineActivationGraphicsComponent>()) {
                MineConfigComponent mineConfig = mine.mineConfig;
                Vector3 position = mine.mineInstance.GameObject.transform.position;
                Vector3 position2 = selfTank.hullInstance.HullInstance.transform.position;
                float magnitude = (position2 - position).magnitude;
                float num = 1f;

                if (magnitude > mineConfig.BeginHideDistance) {
                    num = 1f - Math.Min(1f, (magnitude - mineConfig.BeginHideDistance) / mineConfig.HideRange);
                }

                Renderer renderer = mine.mineRendererGraphics.Renderer;

                if (num <= 0f) {
                    renderer.enabled = false;
                    return;
                }

                renderer.enabled = true;
                Vector4 mINE_ACTIVATION_COLOR = MINE_ACTIVATION_COLOR;
                mINE_ACTIVATION_COLOR.w = num;
                renderer.material.SetColor("_Color", mINE_ACTIVATION_COLOR);
            }
        }

        public class EnemyMineNode : Node {
            public EnemyComponent enemy;
            public MineComponent mine;

            public MineRendererGraphicsComponent mineRendererGraphics;

            public MineTeamGraphicsComponent mineTeamGraphics;
        }

        public class TeamMineNode : Node {
            public MineComponent mine;

            public MineRendererGraphicsComponent mineRendererGraphics;

            public MineTeamGraphicsComponent mineTeamGraphics;

            public TeamGroupComponent teamGroup;
        }

        public class MineDisableNode : Node {
            public MineComponent mine;

            public MineDisableGraphicsComponent mineDisableGraphics;

            public MineInstanceComponent mineInstance;
        }

        public class MineExplosionNode : Node {
            public MineComponent mine;

            public MineExplosionGraphicsComponent mineExplosionGraphics;

            public MineInstanceComponent mineInstance;
        }

        public class MineActiveNode : Node {
            public MineComponent mine;

            public MineActiveComponent mineActive;

            public MineInstanceComponent mineInstance;
        }

        public class MineActivationNode : Node {
            public MineComponent mine;

            public MineActivationGraphicsComponent mineActivationGraphics;

            public MineConfigComponent mineConfig;

            public MineRendererGraphicsComponent mineRendererGraphics;
        }

        public class SelfTankNode : Node {
            public HullInstanceComponent hullInstance;

            public SelfTankComponent selfTank;
            public TankComponent tank;
        }

        public class MineBlendNode : Node {
            public EnemyComponent enemy;
            public MineComponent mine;

            public MineConfigComponent mineConfig;

            public MineInstanceComponent mineInstance;

            public MineRendererGraphicsComponent mineRendererGraphics;
        }
    }
}