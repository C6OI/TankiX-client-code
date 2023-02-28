using System.Collections.Generic;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientSettings.Impl {
    public class GraphicsSettingsIndexesComponent : Component {
        int fullScreenIndex;

        int windowedIndex;

        public int DefaultWindowModeIndex { get; set; }

        public int CurrentWindowModeIndex => !Screen.fullScreen ? windowedIndex : fullScreenIndex;

        public int CurrentSaturationLevelIndex { get; set; }

        public int DefaultSaturationLevelIndex { get; set; }

        public int CurrentVegetationQualityIndex { get; set; }

        public int DefaultVegetationQualityIndex { get; set; }

        public int CurrentGrassQualityIndex { get; set; }

        public int DefaultGrassQualityIndex { get; set; }

        public int CurrentRenderResolutionQualityIndex { get; set; }

        public int DefaultRenderResolutionQualityIndex { get; set; }

        public int CurrentAntialiasingQualityIndex { get; set; }

        public int DefaultAntialiasingQualityIndex { get; set; }

        public int CurrentCartridgeCaseAmountIndex { get; set; }

        public int DefaultCartridgeCaseAmountIndex { get; set; }

        public int CurrentVSyncQualityIndex { get; set; }

        public int DefaultVSyncQualityIndex { get; set; }

        public int CurrentAnisotropicQualityIndex { get; set; }

        public int DefaultAnisotropicQualityIndex { get; set; }

        public int CurrentTextureQualityIndex { get; set; }

        public int DefaultTextureQualityIndex { get; set; }

        public int CurrentShadowQualityIndex { get; set; }

        public int DefaultShadowQualityIndex { get; set; }

        public int CurrentParticleQualityIndex { get; set; }

        public int DefaultParticleQualityIndex { get; set; }

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
            DefaultScreenResolutionIndex = screenResolutions.FindIndex(r => r.width == defaultResolution.width && r.height == defaultResolution.height);
            CurrentScreenResolutionIndex = screenResolutions.FindIndex(r => r.width == currentResolution.width && r.height == currentResolution.height);
        }
    }
}