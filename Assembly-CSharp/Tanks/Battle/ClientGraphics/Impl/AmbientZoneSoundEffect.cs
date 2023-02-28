using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AmbientZoneSoundEffect : MonoBehaviour {
        [SerializeField] AmbientInnerSoundZone[] innerZones;

        [SerializeField] AmbientOuterSoundZone outerZone;

        AmbientSoundZone currentZone;

        int innerZonesLength;
        Transform listener;

        bool needToDestroy;

        HashSet<AmbientSoundZone> playingZones;

        void Update() {
            AmbientSoundZone ambientSoundZone = DefineCurrentAmbientZone();

            if (!ReferenceEquals(ambientSoundZone, currentZone)) {
                currentZone.FadeOut();
                ambientSoundZone.FadeIn();
                currentZone = ambientSoundZone;
            }
        }

        AmbientSoundZone DefineCurrentAmbientZone() {
            for (int i = 0; i < innerZonesLength; i++) {
                AmbientInnerSoundZone ambientInnerSoundZone = innerZones[i];

                if (ambientInnerSoundZone.IsActualZone(listener)) {
                    return ambientInnerSoundZone;
                }
            }

            return outerZone;
        }

        public void Play(Transform listener) {
            this.listener = listener;
            playingZones = new HashSet<AmbientSoundZone>();
            innerZonesLength = innerZones.Length;
            needToDestroy = false;

            for (int i = 0; i < innerZonesLength; i++) {
                AmbientInnerSoundZone ambientInnerSoundZone = innerZones[i];
                ambientInnerSoundZone.InitInnerZone();
            }

            transform.parent = listener;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            currentZone = DefineCurrentAmbientZone();
            currentZone.FadeIn();
            enabled = true;
        }

        public void DisableZoneTransition() {
            enabled = false;
        }

        public void Stop() {
            needToDestroy = true;

            for (int i = 0; i < innerZonesLength; i++) {
                AmbientInnerSoundZone ambientInnerSoundZone = innerZones[i];
                ambientInnerSoundZone.FinalizeInnerZone();
            }

            currentZone.FadeOut();
            DisableZoneTransition();
        }

        public void RegisterPlayingAmbientZone(AmbientSoundZone zone) {
            playingZones.Add(zone);
        }

        public void UnregisterPlayingAmbientZone(AmbientSoundZone zone) {
            playingZones.Remove(zone);
        }

        public void FinalizeEffect() {
            if (needToDestroy && playingZones.Count <= 0) {
                DestroyObject(gameObject);
            }
        }
    }
}