using System.Collections.Generic;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HammerHitSoundEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject staticHitSoundAsset;

        [SerializeField] GameObject targetHitSoundAsset;

        [SerializeField] float staticHitSoundDuration;

        [SerializeField] float targetHitSoundDuration;

        public GameObject StaticHitSoundAsset {
            get => staticHitSoundAsset;
            set => staticHitSoundAsset = value;
        }

        public GameObject TargetHitSoundAsset {
            get => targetHitSoundAsset;
            set => targetHitSoundAsset = value;
        }

        public float StaticHitSoundDuration {
            get => staticHitSoundDuration;
            set => staticHitSoundDuration = value;
        }

        public float TargetHitSoundDuration {
            get => targetHitSoundDuration;
            set => targetHitSoundDuration = value;
        }

        public List<HitTarget> DifferentTargetsByHit { get; set; }

        void Awake() => DifferentTargetsByHit = new List<HitTarget>();
    }
}