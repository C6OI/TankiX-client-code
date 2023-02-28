using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class StreamingDecalProjectorComponent : DynamicDecalProjectorComponent {
        [SerializeField] float decalCreationPeriod = 0.2f;

        public float DecalCreationPeriod => decalCreationPeriod;

        public float LastDecalCreationTime { get; set; }
    }
}