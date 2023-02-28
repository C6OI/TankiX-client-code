using System.Collections;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class MultikillUIComponent : BehaviourComponent {
        static readonly string ACTIVATE_TRIGGER = "Activate";

        static readonly string DEACTIVATE_TRIGGER = "Deactivate";

        [SerializeField] Animator animator;

        [SerializeField] AssetReference voiceReference;

        [SerializeField] LocalizedField multikillText;

        [SerializeField] LocalizedField streakText;

        [SerializeField] TextMeshProUGUI multikillTextField;

        [SerializeField] TextMeshProUGUI streakTextField;

        [SerializeField] AnimatedLong scoreText;

        [SerializeField] bool disableVoice;

        Coroutine coroutine;

        GameObject voiceInstance;

        public Animator Animator => animator;

        public AssetReference VoiceReference => voiceReference;

        public GameObject Voice { get; set; }

        public void ActivateEffect(int score = 0, int kills = 0, string userName = "") {
            if (multikillText != null && !string.IsNullOrEmpty(multikillText.Value)) {
                multikillTextField.text = multikillText.Value;
            }

            scoreText.Value = score;

            if (kills > 0) {
                streakTextField.text = string.Format(streakText.Value, kills);
            } else if (!string.IsNullOrEmpty(userName)) {
                streakTextField.text = string.Format(streakText.Value, userName);
                streakTextField.gameObject.SetActive(true);
            } else {
                streakTextField.gameObject.SetActive(false);
            }

            CancelCoroutine();
            coroutine = StartCoroutine(SetTrigger(ACTIVATE_TRIGGER));
        }

        public void DeactivateEffect() {
            CancelCoroutine();
            animator.SetTrigger(DEACTIVATE_TRIGGER);
        }

        void CancelCoroutine() {
            if (coroutine != null) {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        IEnumerator SetTrigger(string state) {
            yield return new WaitForEndOfFrame();

            animator.SetTrigger(state);
        }

        public void PlayVoice() {
            if (!(Voice == null) && !disableVoice) {
                voiceInstance = Instantiate(Voice);
            }
        }

        public void StopVoice() {
            if (!(Voice == null)) {
                Destroy(voiceInstance);
            }
        }
    }
}