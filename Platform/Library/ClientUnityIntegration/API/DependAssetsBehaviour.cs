using System.Collections.Generic;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public class DependAssetsBehaviour : MonoBehaviour {
        [SerializeField] List<Object> dependAssets;

        public List<Object> DependAssets {
            get => dependAssets;
            set => dependAssets = value;
        }
    }
}