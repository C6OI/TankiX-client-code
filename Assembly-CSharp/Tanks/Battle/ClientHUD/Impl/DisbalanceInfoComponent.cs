using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    [SerialVersionUID(1504088159533L)]
    public class DisbalanceInfoComponent : MonoBehaviour, Component {
        [SerializeField] Timer timer;

        [SerializeField] Animator animator;

        [SerializeField] TextMeshProUGUI tmp;

        [SerializeField] LocalizedField winCtfUid;

        [SerializeField] LocalizedField looseCtfUid;

        [SerializeField] LocalizedField winTdmUid;

        [SerializeField] LocalizedField looseTdmUid;

        public Timer Timer => timer;

        void OnDisable() {
            animator.SetTrigger("Enable");
        }

        public void ShowDisbalanceInfo(bool winner, BattleMode battleMode) {
            string text = "www";

            switch (battleMode) {
                case BattleMode.CTF:
                    text = !winner ? looseCtfUid.Value : winCtfUid.Value;
                    break;

                case BattleMode.TDM:
                    text = !winner ? looseTdmUid.Value : winTdmUid.Value;
                    break;
            }

            tmp.text = text;
            animator.SetTrigger("Show");
            SendMessage("RefreshCurve");
        }

        public void HideDisbalanceInfo() {
            animator.SetTrigger("Hide");
        }
    }
}