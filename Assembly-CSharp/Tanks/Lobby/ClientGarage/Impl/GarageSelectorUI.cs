using System;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageSelectorUI : MonoBehaviour {
        [SerializeField] GameObject hullButton;

        [SerializeField] GameObject turretButton;

        [SerializeField] GameObject modulesButton;

        [SerializeField] GameObject visualButton;

        [SerializeField] Animator hullAnimator;

        [SerializeField] Animator turretAnimator;

        Action onEnable;

        public Action onHullSelected;

        public Action onTurretSelected;

        void Awake() {
            hullButton.GetComponent<Button>().onClick.AddListener(OnHullSelected);
            turretButton.GetComponent<Button>().onClick.AddListener(OnSelectTurret);
        }

        void OnEnable() {
            if (onEnable != null) {
                onEnable();
            }
        }

        public void SelectVisual() {
            SetSelectionButton(visualButton);
        }

        public void SelectModules() {
            SetSelectionButton(modulesButton);
        }

        public void SelectHull() {
            SetSelectionButton(hullButton);

            if (!gameObject.activeInHierarchy) {
                onEnable = delegate { };
            }
        }

        void OnSelectTurret() {
            SelectTurret();
            onTurretSelected();
        }

        void OnHullSelected() {
            SelectHull();
            onHullSelected();
        }

        public void SelectTurret() {
            SetSelectionButton(turretButton);

            if (!gameObject.activeInHierarchy) {
                onEnable = delegate { };
            }
        }

        void SetSelectionButton(GameObject button) {
            button.GetComponent<RadioButton>().Activate();
        }
    }
}