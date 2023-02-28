using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankFrictionSoundEffectComponent : MonoBehaviour, Component {
        [SerializeField] float minValuableFrictionPower;

        [SerializeField] float maxValuableFrictionPower = 1f;

        [SerializeField] SoundController metallFrictionSourcePrefab;

        [SerializeField] SoundController stoneFrictionSourcePrefab;

        [SerializeField] SoundController frictionContactSourcePrefab;

        public SoundController MetallFrictionSourcePrefab {
            get => metallFrictionSourcePrefab;
            set => metallFrictionSourcePrefab = value;
        }

        public SoundController StoneFrictionSourcePrefab {
            get => stoneFrictionSourcePrefab;
            set => stoneFrictionSourcePrefab = value;
        }

        public SoundController FrictionContactSourcePrefab {
            get => frictionContactSourcePrefab;
            set => frictionContactSourcePrefab = value;
        }

        public float MinValuableFrictionPower {
            get => minValuableFrictionPower;
            set => minValuableFrictionPower = value;
        }

        public float MaxValuableFrictionPower {
            get => maxValuableFrictionPower;
            set => maxValuableFrictionPower = value;
        }

        public SoundController MetallFrictionSourceInstance { get; set; }

        public SoundController StoneFrictionSourceInstance { get; set; }

        public SoundController FrictionContactSourceInstance { get; set; }
    }
}