using System;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TrackDustComponent : MonoBehaviour, Component {
        [NonSerialized] public float[] leftTrackDustDelay;

        [NonSerialized] public float[] rightTrackDustDelay;
    }
}