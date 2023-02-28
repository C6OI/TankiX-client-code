using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SpiderMineSoundsComponent : BehaviourComponent {
        [SerializeField] SoundController runSoundController;

        public SoundController RunSoundController => runSoundController;
    }
}