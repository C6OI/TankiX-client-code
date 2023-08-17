using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BattleSoundsAssetComponent : BehaviourComponent {
        [SerializeField] BattleSoundsBehaviour battleSoundsBehaviour;

        public BattleSoundsBehaviour BattleSoundsBehaviour => battleSoundsBehaviour;
    }
}