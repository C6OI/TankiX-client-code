using UnityEngine;

namespace Edelweiss.DecalSystem {
    public abstract class GenericDecalProjectorComponent<D, P, DM> : GenericDecalProjectorBaseComponent
        where D : GenericDecals<D, P, DM> where P : GenericDecalProjector<D, P, DM> where DM : GenericDecalsMesh<D, P, DM> {
        public D GetDecals() {
            D val = null;
            Transform transform = CachedTransform;

            while (transform != null && val == null) {
                val = transform.GetComponent<D>();
                transform = transform.parent;
            }

            return val;
        }
    }
}