using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankDeadStateTextureComponent : MonoBehaviour, Component {
        [SerializeField] Texture2D deadColorTexture;

        [SerializeField] Texture2D deadEmissionTexture;

        [SerializeField] AnimationCurve heatEmission;

        [SerializeField] AnimationCurve whiteToHeat;

        [SerializeField] AnimationCurve paintToHeat;

        public Date FadeStart { get; set; }

        public float LastFade { get; set; }

        public AnimationCurve HeatEmission => heatEmission;

        public AnimationCurve WhiteToHeatTexture => whiteToHeat;

        public AnimationCurve PaintTextureToWhiteHeat => paintToHeat;

        public Texture2D DeadColorTexture => deadColorTexture;

        public Texture2D DeadEmissionTexture => deadEmissionTexture;
    }
}