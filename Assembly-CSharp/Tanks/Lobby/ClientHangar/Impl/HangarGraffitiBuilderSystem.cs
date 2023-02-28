using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientHangar.Impl.Builder;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarGraffitiBuilderSystem : ECSSystem {
        [OnEventComplete]
        public void OnGraffitiEquip(NodeAddedEvent e, GraffitiItemPreviewNode graffiti, ActiveGraffitiScreenNode activeGraffitiScreen, [JoinAll] HangarCameraNode hangarCameraNode) {
            DynamicDecalProjectorComponent component = (graffiti.resourceData.Data as GameObject).GetComponent<DynamicDecalProjectorComponent>();
            activeGraffitiScreen.graffitiPreview.SetPreview(component.Material.mainTexture);
            ScheduleEvent<HangarGraffitiBuildedEvent>(hangarCameraNode);
        }

        [OnEventFire]
        public void OnExitGraffitiScreen(NodeRemoveEvent e, ActiveGraffitiScreenNode activeGraffitiScreen, [JoinAll] HangarCameraNode hangarCameraNode) {
            ScheduleEvent<HangarGraffitiBuildedEvent>(hangarCameraNode);
        }

        public class ActiveGraffitiScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public GraffitiPreviewComponent graffitiPreview;
        }

        public class HangarCameraNode : Node {
            public CameraComponent camera;
            public HangarCameraComponent hangarCamera;
        }

        public class GraffitiItemPreviewNode : Node {
            public GraffitiItemComponent graffitiItem;
            public HangarItemPreviewComponent hangarItemPreview;

            public ResourceDataComponent resourceData;
        }
    }
}