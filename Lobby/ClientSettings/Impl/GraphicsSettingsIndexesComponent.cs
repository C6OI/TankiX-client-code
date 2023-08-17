using System.Collections.Generic;
using Lobby.ClientSettings.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientSettings.Impl {
    public class GraphicsSettingsIndexesComponent : Component {
        int fullScreenIndex;

        int windowedIndex;

        public int DefaultWindowModeIndex { get; set; }

        public int CurrentWindowModeIndex => !Screen.fullScreen ? windowedIndex : fullScreenIndex;

        public int CurrentSaturationLevelIndex { get; set; }

        public int DefaultSaturationLevelIndex { get; set; }

        public int CurrentScreenResolutionIndex { get; set; }

        public int DefaultScreenResolutionIndex { get; set; }

        public void InitWindowModeIndexes(int fullScreenIndex, int windowedIndex) {
            this.fullScreenIndex = fullScreenIndex;
            this.windowedIndex = windowedIndex;
            DefaultWindowModeIndex = !GraphicsSettings.INSTANCE.WindowedByDefault ? fullScreenIndex : windowedIndex;
        }

        public void CalculateScreenResolutionIndexes() {
            List<Resolution> screenResolutions = GraphicsSettings.INSTANCE.ScreenResolutions;
            Resolution defaultResolution = GraphicsSettings.INSTANCE.DefaultResolution;
            Resolution currentResolution = GraphicsSettings.INSTANCE.CurrentResolution;

            DefaultScreenResolutionIndex = screenResolutions.FindIndex(r =>
                r.width == defaultResolution.width && r.height == defaultResolution.height);

            CurrentScreenResolutionIndex = screenResolutions.FindIndex(r =>
                r.width == currentResolution.width && r.height == currentResolution.height);
        }
    }
}