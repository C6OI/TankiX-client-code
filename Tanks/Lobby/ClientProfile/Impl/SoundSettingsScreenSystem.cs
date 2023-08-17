using Lobby.ClientControls.API;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class SoundSettingsScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitSFXVolumeSliderBar(NodeAddedEvent e, SFXVolumeSliderBarNode sfxVolumeSliderBarNode) =>
            sfxVolumeSliderBarNode.sliderBar.Value = SoundSettingsUtils.GetSavedVolume(SoundType.SFX);

        [OnEventFire]
        public void InitMusicVolumeSliderBar(NodeAddedEvent e, MusicVolumeSliderBarNode musicVolumeSliderBarNode) =>
            musicVolumeSliderBarNode.sliderBar.Value = SoundSettingsUtils.GetSavedVolume(SoundType.Music);

        [OnEventFire]
        public void InitUIVolumeSliderBar(NodeAddedEvent e, UIVolumeSliderBarNode uiVolumeSliderBarNode) =>
            uiVolumeSliderBarNode.sliderBar.Value = SoundSettingsUtils.GetSavedVolume(SoundType.UI);

        public class SFXVolumeSliderBarNode : Node {
            public SFXVolumeSliderBarComponent sfxVolumeSliderBar;

            public SliderBarComponent sliderBar;
        }

        public class MusicVolumeSliderBarNode : Node {
            public MusicVolumeSliderBarComponent musicVolumeSliderBar;

            public SliderBarComponent sliderBar;
        }

        public class UIVolumeSliderBarNode : Node {
            public SliderBarComponent sliderBar;
            public UIVolumeSliderBarComponent uiVolumeSliderBar;
        }
    }
}