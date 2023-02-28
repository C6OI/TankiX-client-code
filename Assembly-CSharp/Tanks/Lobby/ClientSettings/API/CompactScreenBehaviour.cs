using System;
using System.Linq;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.API {
    public class CompactScreenBehaviour : MonoBehaviour {
        Resolution avgRes;

        State state;

        void Update() {
            if (ApplicationFocusBehaviour.INSTANCE.Focused) {
                switch (state) {
                    case State.COMPACT:
                        ApplyCompactScreenData();
                        state = State.IDLE;
                        break;

                    case State.DESTRUCTION:
                        ApplyInitialScreenData();
                        Destroy(this);
                        break;
                }
            }
        }

        void OnApplicationQuit() {
            GraphicsSettings.INSTANCE.SaveWindowModeOnQuit();
        }

        public void InitCompactMode() {
            int avgWidth = Convert.ToInt32(GraphicsSettings.INSTANCE.ScreenResolutions.Average(r => r.width));
            int avgHeight = Convert.ToInt32(GraphicsSettings.INSTANCE.ScreenResolutions.Average(r => r.height));
            avgRes = GraphicsSettings.INSTANCE.ScreenResolutions.OrderBy(r => Mathf.Abs(r.width - avgWidth) + Mathf.Abs(r.height - avgHeight)).First();
            Resolution currentResolution = GraphicsSettings.INSTANCE.CurrentResolution;

            if (currentResolution.width + currentResolution.height < avgRes.width + avgRes.height) {
                avgRes = currentResolution;
            }

            ApplyCompactScreenData();
            state = !ApplicationFocusBehaviour.INSTANCE.Focused ? State.COMPACT : State.IDLE;
        }

        public void DisableCompactMode() {
            ApplyInitialScreenData();

            if (ApplicationFocusBehaviour.INSTANCE.Focused) {
                Destroy(this);
            } else {
                state = State.DESTRUCTION;
            }
        }

        void ApplyCompactScreenData() {
            Screen.SetResolution(avgRes.width, avgRes.height, false);
        }

        void ApplyInitialScreenData() {
            GraphicsSettings.INSTANCE.ApplyInitialScreenResolutionData();
        }

        enum State {
            IDLE = 0,
            COMPACT = 1,
            DESTRUCTION = 2
        }
    }
}