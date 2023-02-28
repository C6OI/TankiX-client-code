using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusParachuteMapEffectComponent : BehaviourComponent {
        [SerializeField] GameObject parachute;

        public GameObject Parachute => parachute;
    }
}