using Assets.lobby.modules.ClientControls.Scripts.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine.Audio;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class SoundSettingsSystem : ECSSystem {
        [OnEventFire]
        public void InitSoundSettings(NodeAddedEvent e, SoundListenerNode listener) {
            listener.soundListenerResources.Resources.SfxMixer.SetFloat(SoundSettingsUtils.VOLUME_PARAM_KEY,
                !SoundSettingsUtils.GetSavedMuteFlag(SoundType.SFX) ? SoundSettingsUtils.GetSavedVolume(SoundType.SFX) : SoundSettingsUtils.MUTED_VOLUME_VALUE);

            listener.soundListenerResources.Resources.UIMixer.SetFloat(SoundSettingsUtils.VOLUME_PARAM_KEY,
                !SoundSettingsUtils.GetSavedMuteFlag(SoundType.UI) ? SoundSettingsUtils.GetSavedVolume(SoundType.UI) : SoundSettingsUtils.MUTED_VOLUME_VALUE);

            listener.soundListenerResources.Resources.MusicMixer.SetFloat(SoundSettingsUtils.MUSIC_VOLUME_PARAM_KEY,
                !SoundSettingsUtils.GetSavedMuteFlag(SoundType.Music) ? SoundSettingsUtils.GetSavedVolume(SoundType.Music) : SoundSettingsUtils.MUTED_VOLUME_VALUE);

            listener.soundListenerResources.Resources.MusicMixer.SetFloat(SoundSettingsUtils.LAZY_UI_VOLUME_PARAM_KEY,
                !SoundSettingsUtils.GetSavedMuteFlag(SoundType.UI) ? SoundSettingsUtils.GetSavedVolume(SoundType.UI) : SoundSettingsUtils.MUTED_VOLUME_VALUE);
        }

        [OnEventFire]
        public void SFXVolumeChanged(SliderBarValueChangedEvent e, SFXVolumeSliderBarNode sfxVolumeSliderBarNode, [JoinAll] SoundListenerNode listener) {
            SetSoundVolume(SoundType.SFX, listener.soundListenerResources.Resources.SfxMixer, sfxVolumeSliderBarNode.sliderBar.Value, false);
        }

        [OnEventFire]
        public void SFXVolumeSetToMin(SliderBarSetToMinValueEvent e, SFXVolumeSliderBarNode sfxVolumeSliderBarNode, [JoinAll] SoundListenerNode listener) {
            SetSoundVolume(SoundType.SFX, listener.soundListenerResources.Resources.SfxMixer, sfxVolumeSliderBarNode.sliderBar.Value, true);
        }

        [OnEventFire]
        public void MusicVolumeChanged(SliderBarValueChangedEvent e, MusicVolumeSliderBarNode musicVolumeSliderBarNode, [JoinAll] SoundListenerNode listener) {
            SetSoundVolume(SoundType.Music, SoundSettingsUtils.MUSIC_VOLUME_PARAM_KEY, listener.soundListenerResources.Resources.MusicMixer, musicVolumeSliderBarNode.sliderBar.Value,
                false);
        }

        [OnEventFire]
        public void MusicVolumeSetToMin(SliderBarSetToMinValueEvent e, MusicVolumeSliderBarNode musicVolumeSliderBarNode, [JoinAll] SoundListenerNode listener) {
            SetSoundVolume(SoundType.Music, SoundSettingsUtils.MUSIC_VOLUME_PARAM_KEY, listener.soundListenerResources.Resources.MusicMixer, musicVolumeSliderBarNode.sliderBar.Value,
                true);
        }

        [OnEventFire]
        public void UIVolumeChanged(SliderBarValueChangedEvent e, UIVolumeSliderBarNode uiVolumeSliderBarNode, [JoinAll] SoundListenerNode listener) {
            SetSoundVolume(SoundType.UI, listener.soundListenerResources.Resources.UIMixer, uiVolumeSliderBarNode.sliderBar.Value, false);
            SetSoundVolume(SoundType.UI, SoundSettingsUtils.LAZY_UI_VOLUME_PARAM_KEY, listener.soundListenerResources.Resources.MusicMixer, uiVolumeSliderBarNode.sliderBar.Value, false);
        }

        [OnEventFire]
        public void UIVolumeSetToMin(SliderBarSetToMinValueEvent e, UIVolumeSliderBarNode uiVolumeSliderBarNode, [JoinAll] SoundListenerNode listener) {
            SetSoundVolume(SoundType.UI, listener.soundListenerResources.Resources.UIMixer, uiVolumeSliderBarNode.sliderBar.Value, true);
            SetSoundVolume(SoundType.UI, SoundSettingsUtils.LAZY_UI_VOLUME_PARAM_KEY, listener.soundListenerResources.Resources.MusicMixer, uiVolumeSliderBarNode.sliderBar.Value, true);
        }

        void SetSoundVolume(SoundType soundType, AudioMixer mixer, float volume, bool isMuted) {
            SetSoundVolume(soundType, SoundSettingsUtils.VOLUME_PARAM_KEY, mixer, volume, isMuted);
        }

        void SetSoundVolume(SoundType soundType, string param, AudioMixer mixer, float volume, bool isMuted) {
            SoundSettingsUtils.SaveVolume(soundType, volume);
            SoundSettingsUtils.SaveMuteFlag(soundType, isMuted);
            mixer.SetFloat(param, !isMuted ? volume : SoundSettingsUtils.MUTED_VOLUME_VALUE);
        }

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

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerResourcesComponent soundListenerResources;
        }
    }
}