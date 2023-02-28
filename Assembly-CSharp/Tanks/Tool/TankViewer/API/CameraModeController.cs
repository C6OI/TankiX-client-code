using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class CameraModeController : MonoBehaviour {
        public Camera mainCamera;

        public Camera solidBackCamera;

        public List<Color> backColors;

        public Camera maskCamera;

        public GameObject plane;

        List<CameraMode> cameraModes;

        int currentModeIndex;

        void Awake() {
            cameraModes = new List<CameraMode>();
            DefaultMode defaultMode = new(mainCamera);
            cameraModes.Add(defaultMode);

            for (int i = 0; i < backColors.Count; i++) {
                cameraModes.Add(new SolidBackMode(solidBackCamera, backColors[i]));
            }

            cameraModes.Add(new MaskMode(maskCamera, plane));
            defaultMode.SwitchOn();
        }

        public void ChangeMode() {
            cameraModes[currentModeIndex].SwithOff();
            currentModeIndex = currentModeIndex < cameraModes.Count - 1 ? currentModeIndex + 1 : 0;
            cameraModes[currentModeIndex].SwitchOn();
        }
    }
}