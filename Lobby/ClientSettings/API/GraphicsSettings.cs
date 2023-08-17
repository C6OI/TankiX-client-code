using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lobby.ClientSettings.API {
    public class GraphicsSettings {
        const string SATURATION_SHADER_PARAMETER = "_GraphicsSettingsSaturationLevel";

        const string QUALITY_LEVEL_KEY = "QUALITY_LEVEL_INDEX";

        const string SCREEN_RESOLUTION_WIDTH_KEY = "SCREEN_RESOLUTION_WIDTH";

        const string SCREEN_RESOLUTION_HEIGHT_KEY = "SCREEN_RESOLUTION_HEIGHT";

        const string SATURATION_LEVEL_KEY = "SATURATION_LEVEL";

        const string WINDOW_MODE_KEY = "WINDOW_MODE";

        const string NO_COMPACT_WINDOW_KEY = "NO_COMPACT_WINDOW";

        CompactScreenBehaviour compactScreen;

        int currentQualityLevel;

        Resolution currentResolution;

        float currentSaturationLevel;

        public static GraphicsSettings INSTANCE { get; set; }

        public bool WindowedByDefault { get; private set; }

        public bool InitialWindowed { get; private set; }

        public int DefaultQualityLevel => DefaultQuality.Level;

        public Quality DefaultQuality { get; private set; }

        public bool UltraEnabled { get; private set; }

        public int CurrentQualityLevel {
            get => currentQualityLevel;
            private set {
                currentQualityLevel = value;
                PlayerPrefs.SetInt("QUALITY_LEVEL_INDEX", currentQualityLevel);
            }
        }

        public float DefaultSaturationLevel { get; private set; }

        public float CurrentSaturationLevel {
            get => currentSaturationLevel;
            private set {
                currentSaturationLevel = value;
                PlayerPrefs.SetFloat("SATURATION_LEVEL", currentSaturationLevel);
            }
        }

        public List<Resolution> ScreenResolutions { get; private set; }

        public Resolution DefaultResolution { get; private set; }

        public Resolution CurrentResolution {
            get => currentResolution;
            private set {
                currentResolution = value;
                PlayerPrefs.SetInt("SCREEN_RESOLUTION_WIDTH", currentResolution.width);
                PlayerPrefs.SetInt("SCREEN_RESOLUTION_HEIGHT", currentResolution.height);
            }
        }

        public void ApplyQualityLevel(int qualityLevel) => CurrentQualityLevel = qualityLevel;

        public void ApplyWindowMode(bool windowed) => Screen.fullScreen = !windowed;

        public void ApplySaturationLevel(float currentValue, float defaultValue) {
            DefaultSaturationLevel = defaultValue;
            ApplySaturationLevel(currentValue);
        }

        public void ApplySaturationLevel(float currentValue) {
            CurrentSaturationLevel = currentValue;
            Shader.SetGlobalFloat("_GraphicsSettingsSaturationLevel", currentSaturationLevel);
        }

        public void ApplyScreenResolution(int width, int height, bool windowed) {
            PlayerPrefs.SetInt("SCREEN_RESOLUTION_WIDTH", width);
            PlayerPrefs.SetInt("SCREEN_RESOLUTION_HEIGHT", height);
            Screen.SetResolution(width, height, !windowed);
        }

        public void InitSaturationLevelSettings(float defaultSaturationLevel) {
            DefaultSaturationLevel = defaultSaturationLevel;

            if (PlayerPrefs.HasKey("SATURATION_LEVEL")) {
                currentSaturationLevel = PlayerPrefs.GetFloat("SATURATION_LEVEL");
            } else {
                CurrentSaturationLevel = DefaultSaturationLevel;
            }
        }

        public void InitQualitySettings(Quality defaultQuality, bool ultraEnabled) {
            DefaultQuality = defaultQuality;
            UltraEnabled = ultraEnabled;

            if (PlayerPrefs.HasKey("QUALITY_LEVEL_INDEX")) {
                currentQualityLevel = PlayerPrefs.GetInt("QUALITY_LEVEL_INDEX");
            } else {
                CurrentQualityLevel = DefaultQualityLevel;
            }

            if (!ultraEnabled && Quality.ultra.Level == currentQualityLevel) {
                currentQualityLevel = Quality.maximum.Level;
            }

            QualitySettings.SetQualityLevel(CurrentQualityLevel, true);
        }

        public void InitScreenResolutionSettings(List<Resolution> avaiableResolutions, Resolution defaultResolution) {
            ScreenResolutions = avaiableResolutions;
            DefaultResolution = defaultResolution;
            bool flag = false;

            if (!PlayerPrefs.HasKey("SCREEN_RESOLUTION_WIDTH") || !PlayerPrefs.HasKey("SCREEN_RESOLUTION_HEIGHT")) {
                CurrentResolution = DefaultResolution;
            } else {
                flag = true;
                int @int = PlayerPrefs.GetInt("SCREEN_RESOLUTION_WIDTH");
                int int2 = PlayerPrefs.GetInt("SCREEN_RESOLUTION_HEIGHT");
                currentResolution = default;
                currentResolution.width = @int;
                currentResolution.height = int2;

                CurrentResolution = ScreenResolutions.OrderBy(r =>
                    Mathf.Abs(r.width - currentResolution.width) + Mathf.Abs(r.height - currentResolution.height)).First();
            }

            bool flag2 = true;

            if (!NeedCompactWindow()) {
                flag2 = !flag ? !WindowedByDefault : DefineScreenMode();
            }

            InitialWindowed = !flag2;
        }

        public void ApplyInitialScreenResolutionData() =>
            Screen.SetResolution(currentResolution.width, currentResolution.height, !InitialWindowed);

        bool DefineScreenMode() {
            if (PlayerPrefs.HasKey("WINDOW_MODE")) {
                bool result = !Convert.ToBoolean(PlayerPrefs.GetInt("WINDOW_MODE"));
                PlayerPrefs.DeleteKey("WINDOW_MODE");
                return result;
            }

            return Screen.fullScreen;
        }

        public void SaveWindowModeOnQuit() => PlayerPrefs.SetInt("WINDOW_MODE", Convert.ToInt32(InitialWindowed));

        public void InitWindowModeSettings(bool isWindowedByDefault) => WindowedByDefault = isWindowedByDefault;

        public void EnableCompactScreen(CompactScreenBehaviour compactScreen) {
            this.compactScreen = compactScreen;
            compactScreen.InitCompactMode();
        }

        public void DisableCompactScreen() {
            if (!(compactScreen == null)) {
                compactScreen.DisableCompactMode();
                PlayerPrefs.SetInt("NO_COMPACT_WINDOW", 1);

                if (PlayerPrefs.HasKey("WINDOW_MODE")) {
                    PlayerPrefs.DeleteKey("WINDOW_MODE");
                }
            }
        }

        public bool NeedCompactWindow() => !PlayerPrefs.HasKey("NO_COMPACT_WINDOW");
    }
}