using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tool.TankViewer.API {
    public class UIController : MonoBehaviour {
        public TankContentController tankContentController;

        public UIViewController viewController;

        public CameraController cameraController;

        public Dropdown modeDropdown;

        public GameObject coloringViewer;

        public GameObject dron;

        public GameObject spiderMine;

        public GameObject container;

        bool createColoringState;

        void Awake() {
            tankContentController.Init();
            viewController.ChangeHullName(tankContentController.CurrentHullName);
            viewController.ChangeWeaponName(tankContentController.CurrentWeaponName);
        }

        public void Update() {
            if (createColoringState) {
                return;
            }

            if (cameraController != null) {
                if (Input.GetKeyUp(KeyCode.Space)) {
                    cameraController.ChangeMode();
                }

                if (Input.GetKeyUp(KeyCode.R)) {
                    cameraController.targetCameraController.SetDefaultTransform();
                }

                if (Input.GetKeyUp(KeyCode.F4)) {
                    cameraController.targetCameraController.AutoRotate = !cameraController.targetCameraController.AutoRotate;
                }

                if (Input.GetKeyUp(KeyCode.T)) {
                    viewController.cameraTransform.text = string.Format("pos:{0}, rot: {1}", cameraController.transform.position, cameraController.transform.rotation.eulerAngles);
                }

                if (Input.GetKeyUp(KeyCode.G)) {
                    cameraController.ChangeController();
                }
            }

            if (Input.GetKeyUp(KeyCode.P)) {
                string filePath = string.Format("screen__{0}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
                ScreenShotUtil.TakeScreenshotAndOpenIt(Camera.main, filePath, 4);
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
        }

        public void OnCreateColoringButtonClick() {
            createColoringState = true;
            modeDropdown.enabled = false;
        }

        public void OnCreateColoringFinished() {
            createColoringState = false;
            modeDropdown.enabled = true;
        }

        public void OnModeDropdownChange(Dropdown dropdown) {
            if (dropdown.value > 3) {
                throw new Exception("Invalid mode dropdown value: " + dropdown.value);
            }

            coloringViewer.SetActive(dropdown.value == 0);
            tankContentController.SetVisible(dropdown.value == 0);
            dron.SetActive(dropdown.value == 1);
            spiderMine.SetActive(dropdown.value == 2);
            container.SetActive(dropdown.value == 3);
        }
    }
}