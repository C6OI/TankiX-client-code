using System.Collections.Generic;
using UnityEngine;

namespace Lobby.ClientSettings.API {
    public abstract class GraphicsSettingsAnalyzer : MonoBehaviour {
        public abstract Quality GetDefaultQuality();

        public abstract Resolution GetDefaultResolution(List<Resolution> resolutions);

        public abstract void Init();
    }
}