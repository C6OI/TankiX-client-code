using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientLoading.API {
    public class LoadingStatusView : MonoBehaviour {
        public LocalizedField loadFromNetworkText;

        public LocalizedField mbText;

        public LocalizedField loadFromDiskText;

        public TextMeshProUGUI loadingStatus;

        public void UpdateView(LoadBundlesTaskComponent loadBundlesTask) {
            if (loadBundlesTask.MBytesToLoadFromNetwork > 5) {
                loadingStatus.gameObject.SetActive(true);

                if (loadBundlesTask.MBytesLoadedFromNetwork < loadBundlesTask.MBytesToLoadFromNetwork) {
                    loadingStatus.text = string.Format("{0} \n{1} {3} / {2} {3}", loadFromNetworkText.Value, loadBundlesTask.MBytesLoadedFromNetwork,
                        loadBundlesTask.MBytesToLoadFromNetwork, mbText.Value);
                } else {
                    loadingStatus.text = loadFromDiskText.Value;
                }
            } else {
                loadingStatus.gameObject.SetActive(false);
            }
        }
    }
}