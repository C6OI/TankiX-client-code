using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class ItemPreviewBaseSystem : ECSSystem {
        protected void PreviewItem(Entity item) {
            ScheduleEvent<PrewievEvent>(item);
        }

        public class UserItemNode : Node {
            public GarageItemComponent garageItem;
            public UserGroupComponent userGroup;

            public UserItemComponent userItem;
        }

        public class MountedUserItemNode : UserItemNode {
            public MountedItemComponent mountedItem;
        }

        [Not(typeof(GraffitiItemComponent))]
        public class NotGraffitiNode : Node {
            public GarageItemComponent garageItem;
        }

        public class PreviewNode : Node {
            public GarageItemComponent garageItem;

            public HangarItemPreviewComponent hangarItemPreview;
        }

        public class GraffitiPreviewNode : PreviewNode {
            public GraffitiItemComponent graffitiItem;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class WeaponNotPreviewNode : Node {
            public WeaponItemComponent weaponItem;
        }

        public class HulLPreviewNode : Node {
            public TankItemComponent tankItem;
        }

        public class PrewievEvent : Event { }
    }
}