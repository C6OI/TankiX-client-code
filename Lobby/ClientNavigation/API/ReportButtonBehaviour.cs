using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientNavigation.API {
    [RequireComponent(typeof(Button))]
    public class ReportButtonBehaviour : MonoBehaviour {
        [SerializeField] string defaultReportUrl;

        public string ReportUrl { get; set; }

        void Awake() {
            ReportUrl = defaultReportUrl;
            GetComponent<Button>().onClick.AddListener(OpenUrl);
        }

        void OpenUrl() => Application.OpenURL(ReportUrl);
    }
}