using System.Collections;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PrivacyPolicyDialog : ConfirmDialogComponent {
        [SerializeField] LocalizedField fileName;

        [SerializeField] TextMeshProUGUI text;

        IEnumerator LoadText() {
            string path = "file://" + Application.dataPath + "/config/clientlocal/privacypolicy/" + fileName.Value + ".txt";
            WWW www = new(path);
            yield return www;

            text.text = www.text;
            text.gameObject.AddComponent<TMPLink>();
        }

        public new virtual void OnHide() {
            base.OnHide();
            text.text = string.Empty;
        }

        public override void OnShow() {
            base.OnShow();
            StartCoroutine(LoadText());
        }

        public override void Hide() { }

        public void HideByAcceptButton() {
            if (show) {
                MainScreenComponent.Instance.ClearOnBackOverride();
                show = false;

                if (this != null) {
                    GetComponent<Animator>().SetBool("show", false);
                }

                ShowHiddenScreenParts();
            }
        }
    }
}