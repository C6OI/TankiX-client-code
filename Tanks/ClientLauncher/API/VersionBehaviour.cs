using UnityEngine;

namespace Tanks.ClientLauncher.API {
    public class VersionBehaviour : MonoBehaviour {
        [SerializeField] string currentVersion;

        [SerializeField] string distributionUrl;

        public string CurrentVersion {
            get => currentVersion;
            set => currentVersion = value;
        }

        public string DistributionUrl {
            get => distributionUrl;
            set => distributionUrl = value;
        }
    }
}