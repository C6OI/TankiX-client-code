using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.API {
    public class SoundListenerComponent : BehaviourComponent {
        [SerializeField] float delayForLobbyState = 0.33f;

        [SerializeField] float delayForBattleEnterState = 0.05f;

        [SerializeField] float delayForBattleState = 1.5f;

        public float DelayForLobbyState => delayForLobbyState;

        public float DelayForBattleEnterState => delayForBattleEnterState;

        public float DelayForBattleState => delayForBattleState;

        void Awake() {
            AudioListener.pause = false;
        }

        void OnDestroy() {
            AudioListener.pause = true;
        }

        void OnApplicationQuit() {
            AudioListener.pause = true;
        }
    }
}