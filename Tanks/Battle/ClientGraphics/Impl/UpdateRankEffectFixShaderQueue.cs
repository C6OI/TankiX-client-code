using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectFixShaderQueue : MonoBehaviour {
        public int AddQueue = 1;

        void Start() {
            if (GetComponent<Renderer>() != null) {
                GetComponent<Renderer>().sharedMaterial.renderQueue += AddQueue;
            } else {
                Invoke("SetProjectorQueue", 0.1f);
            }
        }

        void Update() { }

        void SetProjectorQueue() => GetComponent<Projector>().material.renderQueue += AddQueue;
    }
}