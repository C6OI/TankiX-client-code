using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class FreezeStreamEffect : StreamEffectBehaviour {
        public ParticleSystem muzzleParticleSystem;

        public ParticleSystem mistParticleSystem;

        public ParticleSystem snowParticleSystem;

        public override void ApplySettings(StreamWeaponSettingsComponent streamWeaponSettings) {
            base.ApplySettings(streamWeaponSettings);
            muzzleParticleSystem.maxParticles = streamWeaponSettings.FreezeMuzzleMaxParticles;
            mistParticleSystem.maxParticles = streamWeaponSettings.FreezeMistMaxParticles;
            snowParticleSystem.maxParticles = streamWeaponSettings.FreezeSnowMaxParticles;
        }
    }
}