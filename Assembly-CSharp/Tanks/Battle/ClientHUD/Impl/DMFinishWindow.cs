using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    public class DMFinishWindow : LocalizedControl {
        [SerializeField] Text timeIsUpText;

        public string TimeIsUpText {
            set => timeIsUpText.text = value;
        }

        public void Show() {
            gameObject.SetActive(true);
            GetComponent<CanvasGroup>().alpha = 0f;
            GetComponent<Animator>().SetTrigger("Show");
        }
    }
}