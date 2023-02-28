using UnityEngine;

namespace AssemblyCSharp {
    [ExecuteInEditMode]
    public class RealtimeShadowParameters : MonoBehaviour {
        public Color shadowColor;

        public float shadowStrength;

        void OnEnable() {
            Shader.SetGlobalColor("_ShadowMixColor", shadowColor);
            Shader.SetGlobalFloat("_ShadowMixStrength", shadowStrength);
        }
    }
}