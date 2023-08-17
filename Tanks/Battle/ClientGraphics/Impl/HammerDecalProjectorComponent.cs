using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.impl {
    public class HammerDecalProjectorComponent : DynamicDecalProjectorComponent {
        [SerializeField] float combineHalfSize = 5f;

        public float CombineHalfSize {
            get => combineHalfSize;
            set => combineHalfSize = value;
        }
    }
}