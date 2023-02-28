using Tanks.Battle.ClientCore.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientNavigation.API {
    public class FatalErrorScreenBehaviour : MonoBehaviour {
        [SerializeField] Text header;

        [SerializeField] Text text;

        [SerializeField] TextMeshProUGUI restart;

        [SerializeField] TextMeshProUGUI quit;

        [SerializeField] TextMeshProUGUI report;

        [SerializeField] ReportButtonBehaviour reportButtonBehaviour;

        void Start() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (ErrorScreenData.data != null) {
                header.text = ErrorScreenData.data.HeaderText;
                text.text = ErrorScreenData.data.ErrorText;
                restart.text = ErrorScreenData.data.RestartButtonLabel;
                quit.text = ErrorScreenData.data.ExitButtonLabel;
                report.text = ErrorScreenData.data.ReportButtonLabel;

                if (ErrorScreenData.data.ReConnectTime > 0) {
                    gameObject.AddComponent<ReConnectBehaviour>().ReConnectTime = ErrorScreenData.data.ReConnectTime;
                    restart.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}