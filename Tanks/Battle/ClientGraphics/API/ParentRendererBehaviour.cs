using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class ParentRendererBehaviour : MonoBehaviour {
        [SerializeField] Renderer parentRenderer;

        public Renderer ParentRenderer {
            get => parentRenderer;
            set => parentRenderer = value;
        }
    }
}