using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanSoundManagerComponent : Component {
        public VulcanSoundManagerComponent() => SoundsWithDelay = new Dictionary<AudioSource, float>();

        public AudioSource CurrentSound { get; set; }

        public Dictionary<AudioSource, float> SoundsWithDelay { get; set; }
    }
}