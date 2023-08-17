using System;
using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class UIController : MonoBehaviour {
        public CameraModeController cameraModeController;

        public TankContentController tankContentController;

        public UIViewController viewController;

        public TargetCameraController cameraController;

        void Awake() {
            tankContentController.Init();
            viewController.ChangeHullName(tankContentController.CurrentHullName);
            viewController.ChangeWeaponName(tankContentController.CurrentWeaponName);
        }

        public void Update() {
            if (Input.GetKeyUp(KeyCode.Space)) {
                cameraModeController.ChangeMode();
            }

            if (Input.GetKeyUp(KeyCode.Return)) {
                string filePath = string.Format("screen__{0}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
                ScreenShotUtil.TakeScreenshot(Camera.main, filePath, 4);
            }

            if (Input.GetKeyUp(KeyCode.Home)) {
                tankContentController.SetNextHull();
                viewController.ChangeHullName(tankContentController.CurrentHullName);
            }

            if (Input.GetKeyUp(KeyCode.End)) {
                tankContentController.SetPrevHull();
                viewController.ChangeHullName(tankContentController.CurrentHullName);
            }

            if (Input.GetKeyUp(KeyCode.PageUp)) {
                tankContentController.SetNextWeapon();
                viewController.ChangeWeaponName(tankContentController.CurrentWeaponName);
            }

            if (Input.GetKeyUp(KeyCode.PageDown)) {
                tankContentController.SetPrevWeapon();
                viewController.ChangeWeaponName(tankContentController.CurrentWeaponName);
            }

            if (Input.GetKeyUp(KeyCode.Insert)) {
                tankContentController.SetNextColoring();
            }

            if (Input.GetKeyUp(KeyCode.Delete)) {
                tankContentController.SetPrevColoring();
            }

            if (Input.GetKeyUp(KeyCode.Q)) {
                tankContentController.ChangeVisibleParts();
                viewController.hullName.gameObject.SetActive(tankContentController.IsHullVisible());
                viewController.weaponName.gameObject.SetActive(tankContentController.IsWeaponVisible());
            }

            if (Input.GetKeyUp(KeyCode.T)) {
                viewController.cameraTransform.text = string.Format("pos:{0}, rot: {1}",
                    cameraController.transform.position,
                    cameraModeController.transform.rotation.eulerAngles);
            }

            if (Input.GetKeyUp(KeyCode.R)) {
                cameraController.SetDefaultTransform();
            }

            if (Input.GetKeyUp(KeyCode.F4)) {
                cameraController.AutoRotate = !cameraController.AutoRotate;
            }
        }
    }
}