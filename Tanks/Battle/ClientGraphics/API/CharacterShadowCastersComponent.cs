using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class CharacterShadowCastersComponent : Component {
        Transform[] casters;

        public Transform[] Casters {
            get => casters;
            set {
                casters = value;
                Renderers = new List<Renderer>();
                Transform[] array = casters;

                foreach (Transform element in array) {
                    CollectRendereres(element, Renderers);
                }

                HasBounds = Renderers.Count > 0;
            }
        }

        public List<Renderer> Renderers { get; private set; }

        public bool HasBounds { get; private set; }

        public Bounds BoundsInWorldSpace {
            get {
                Bounds bounds = Renderers[0].bounds;

                for (int i = 1; i < Renderers.Count; i++) {
                    bounds.Encapsulate(Renderers[i].bounds);
                }

                return bounds;
            }
        }

        void CollectRendereres(Transform element, List<Renderer> renderers) {
            bool includeInactive = true;
            Renderer[] componentsInChildren = element.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive);
            Renderer[] componentsInChildren2 = element.GetComponentsInChildren<MeshRenderer>(includeInactive);
            renderers.AddRange(componentsInChildren);
            renderers.AddRange(componentsInChildren2);
        }
    }
}