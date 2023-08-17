using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    class TrackMarksComponent : MonoBehaviour, Component {
        public int maxSectorsPerTrack = 100;

        public int hideSectorsFrom = 70;

        public Material material;

        public float baseAlpha = 1f;

        public int parts = 5;

        public float markWidth = 0.4f;

        public float markTestShift = 0.4f;

        public float maxDistance = 1000f;

        public float hitDetect = 1.5f;

        public float shiftToCenter;
    }
}