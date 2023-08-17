using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Battle.ClientHUD.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HUDBuilderSystem : ECSSystem {
        [OnEventFire]
        public void AddESMComponent(NodeAddedEvent e, HUDScreenNode hud) {
            BattleHUDESMComponent battleHUDESMComponent = new();
            EntityStateMachine esm = battleHUDESMComponent.Esm;
            esm.AddState<BattleHUDStates.ActionsState>();
            esm.AddState<BattleHUDStates.ChatState>();
            esm.AddState<BattleHUDStates.ShaftAimingState>();
            hud.Entity.AddComponent(battleHUDESMComponent);
        }

        [OnEventFire]
        public void HUDActivation(NodeAddedEvent e, HUDScreenNode hud, CameraNode camera) => CreateWorldSpaceCanvas(hud);

        static void CreateWorldSpaceCanvas(HUDScreenNode hud) {
            GameObject gameObject = Object.Instantiate(hud.hudWorldSpaceCanvasPrefab.hudWorldSpaceCanvasPrefab);
            Canvas component = gameObject.GetComponent<Canvas>();
            component.worldCamera = Camera.main;
        }

        [OnEventFire]
        public void RemoveWorldSpaceCanvas(NodeRemoveEvent e, HUDScreenNode hud,
            [JoinAll] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD) => Object.Destroy(worldSpaceHUD.component.gameObject);

        public class CameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraComponent camera;
        }

        public class HUDScreenNode : Node {
            public BattleScreenComponent battleScreen;

            public HUDWorldSpaceCanvasPrefabComponent hudWorldSpaceCanvasPrefab;
        }
    }
}