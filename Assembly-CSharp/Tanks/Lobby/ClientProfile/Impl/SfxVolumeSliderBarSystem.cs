using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class SfxVolumeSliderBarSystem : ECSSystem {
        [OnEventFire]
        public void InitVolumeNotifier(NodeAddedEvent e, SfxVolumeSliderBarNode slider, [Context] [JoinByScreen] SoundSettingsScreenNode screen,
            SingleNode<SoundListenerResourcesComponent> listener) {
            GameObject gameObject = slider.sfxVolumeSliderBar.gameObject;
            VolumeChangedNotifierComponent volumeChangedNotifierComponent = gameObject.AddComponent<VolumeChangedNotifierComponent>();
            volumeChangedNotifierComponent.Slider = gameObject.GetComponent<Slider>();
            volumeChangedNotifierComponent.AudioSource = Object.Instantiate(listener.component.Resources.SfxSourcePreview);
            slider.Entity.AddComponent(volumeChangedNotifierComponent);
        }

        [OnEventFire]
        public void FinalizeVolumeNotifier(NodeRemoveEvent e, SfxVolumeSliderBarNotifierNode slider) {
            Object.DestroyObject(slider.volumeChangedNotifier.AudioSource.gameObject, slider.volumeChangedNotifier.AudioSource.clip.length);
            Object.Destroy(slider.volumeChangedNotifier);
        }

        public class SfxVolumeSliderBarNode : Node {
            public ScreenGroupComponent screenGroup;
            public SFXVolumeSliderBarComponent sfxVolumeSliderBar;
        }

        public class SfxVolumeSliderBarNotifierNode : SfxVolumeSliderBarNode {
            public VolumeChangedNotifierComponent volumeChangedNotifier;
        }

        public class SoundSettingsScreenNode : Node {
            public ScreenGroupComponent screenGroup;
            public SoundSettingsScreenComponent soundSettingsScreen;
        }
    }
}