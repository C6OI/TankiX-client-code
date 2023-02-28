using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class WarmingUpTimerNotificationUIComponent : BehaviourComponent {
        [SerializeField] AssetReference voiceReference;

        public AssetReference VoiceReference => voiceReference;

        public void PlaySound(GameObject voice) {
            Instantiate(voice);
        }
    }
}