using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanSlowDownAfterSpeedUpSoundEffectComponent : AbstractVulcanSoundEffectComponent {
        [SerializeField] float additionalStartTimeOffset;

        public float AdditionalStartTimeOffset {
            get => additionalStartTimeOffset;
            set => additionalStartTimeOffset = value;
        }
    }
}