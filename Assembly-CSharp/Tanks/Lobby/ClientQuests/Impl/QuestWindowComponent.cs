using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestWindowComponent : UIBehaviour, Component {
        [SerializeField] GameObject questPrefab;

        [SerializeField] GameObject questCellPrefab;

        [SerializeField] GameObject questsContainer;

        [SerializeField] GameObject background;

        List<Animator> animators;

        public Action HideDelegate;

        [Inject] public static EngineService EngineService { get; set; }

        public GameObject QuestPrefab => questPrefab;

        public GameObject QuestCellPrefab => questCellPrefab;

        public GameObject QuestsContainer => questsContainer;

        public bool ShowOnMainScreen { get; set; }

        public bool ShowProgress { get; set; }

        void Update() {
            if (InputMapping.Cancel && ShowOnMainScreen) {
                Hide();
            }
        }

        new void OnDisable() {
            ShowHiddenScreenParts();
        }

        public void Show(List<Animator> animators) {
            gameObject.SetActive(true);
            background.SetActive(true);

            if (!ShowOnMainScreen) {
                return;
            }

            MainScreenComponent.Instance.OverrideOnBack(Hide);
            this.animators = animators;

            foreach (Animator animator in animators) {
                animator.SetBool("Visible", false);
            }
        }

        public void HideWindow() {
            Hide();
        }

        void Hide() {
            if (HideDelegate != null) {
                HideDelegate();
                HideDelegate = null;
            } else if (ShowOnMainScreen) {
                MainScreenComponent.Instance.ClearOnBackOverride();
                ShowHiddenScreenParts();
            }

            gameObject.SetActive(false);
        }

        void ShowHiddenScreenParts() {
            if (animators == null) {
                return;
            }

            foreach (Animator animator in animators) {
                animator.SetBool("Visible", true);
            }

            animators = null;
        }
    }
}