using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class EMPWaveGraphicsEffectComponent : BehaviourComponent {
        [SerializeField] EMPWaveGraphicsBehaviour empWaveAsset;

        public EMPWaveGraphicsBehaviour EMPWaveAsset => empWaveAsset;
    }
}