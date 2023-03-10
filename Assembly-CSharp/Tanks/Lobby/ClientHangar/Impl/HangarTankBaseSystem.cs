using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarTankBaseSystem : ECSSystem {
        public class HangarNode : Node {
            public HangarContainerPositionComponent hangarContainerPosition;
            public HangarTankPositionComponent hangarTankPosition;
        }

        public class HangarCameraNode : Node {
            public CameraComponent camera;

            public CameraRootTransformComponent cameraRootTransform;

            public HangarComponent hangar;
        }

        public class HangarPreviewItemNode : Node {
            public HangarItemPreviewComponent hangarItemPreview;
        }

        public class WeaponItemPreviewNode : HangarPreviewItemNode {
            public ParentGroupComponent parentGroup;
            public WeaponItemComponent weaponItem;
        }

        public class TankItemPreviewNode : HangarPreviewItemNode {
            public ParentGroupComponent parentGroup;
            public TankItemComponent tankItem;
        }

        public class SkinItemPreviewLoadedNode : HangarPreviewItemNode {
            public ResourceDataComponent resourceData;

            public SkinItemComponent skinItem;
        }

        public class WeaponSkinItemPreviewLoadedNode : SkinItemPreviewLoadedNode {
            public WeaponSkinItemComponent weaponSkinItem;
        }

        public class HullSkinItemPreviewLoadedNode : SkinItemPreviewLoadedNode {
            public HullSkinItemComponent hullSkinItem;
        }
    }
}