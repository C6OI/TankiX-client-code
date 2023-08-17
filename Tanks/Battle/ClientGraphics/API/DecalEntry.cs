using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public struct DecalEntry {
        public GameObject gameObject;

        public Material material;

        public float timeToDestroy;

        public DecalEntryType type;
    }
}