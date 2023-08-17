using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class FlamethrowerStreamEffect : StreamEffectBehaviour {
        public ParticleSystem muzzleParticleSystem;

        public ParticleSystem flameParticleSystem;

        public ParticleSystem smokeParticleSystem;

        public override void ApplySettings(StreamWeaponSettingsComponent streamWeaponSettings) {
            base.ApplySettings(streamWeaponSettings);
            muzzleParticleSystem.maxParticles = streamWeaponSettings.FlamethrowerMuzzleMaxParticles;
            flameParticleSystem.maxParticles = streamWeaponSettings.FlamethrowerFlameMaxParticles;
            smokeParticleSystem.maxParticles = streamWeaponSettings.FlamethrowerSmokeMaxParticles;
        }
    }
}