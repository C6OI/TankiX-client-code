using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SelfUserRankSoundEffectInstanceComponent : Component {
        public SelfUserRankSoundEffectInstanceComponent(AudioSource source) => Source = source;

        public AudioSource Source { get; set; }
    }
}