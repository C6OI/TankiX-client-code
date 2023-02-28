using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    internal class TrackMarksComponent : BehaviourComponent {
        public int maxSectorsPerTrack = 100;

        public int hideSectorsFrom = 70;

        public Material material;

        public ParticleSystem[] particleSystems;

        public float baseAlpha = 1f;

        public int parts = 5;

        public float markWidth = 0.4f;

        public float markTestShift = 0.4f;

        public float maxDistance = 1000f;

        public float hitDetect = 1.5f;

        public float shiftToCenter;

        public long tick;
    }
}