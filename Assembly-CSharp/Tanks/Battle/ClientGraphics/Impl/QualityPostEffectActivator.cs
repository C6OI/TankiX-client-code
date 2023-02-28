using System.Collections.Generic;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class QualityPostEffectActivator : MonoBehaviour {
        public Quality.QualityLevel quality;

        public DepthTextureMode depthTextureMode;

        [SerializeField] List<MonoBehaviour> postEffects;

        public void Start() {
            if (QualitySettings.GetQualityLevel() != (int)quality) {
                return;
            }

            Camera.main.depthTextureMode = depthTextureMode;

            foreach (MonoBehaviour postEffect in postEffects) {
                postEffect.enabled = true;
            }
        }
    }
}