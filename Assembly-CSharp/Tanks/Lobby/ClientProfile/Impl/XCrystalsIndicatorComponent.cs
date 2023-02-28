using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class XCrystalsIndicatorComponent : BehaviourComponent {
        [SerializeField] AnimatedLong number;

        public long Value {
            get => number.Value;
            set => number.Value = value;
        }

        public void SetValueWithoutAnimation(long value) {
            number.SetImmediate(value);
        }
    }
}