using System;
using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DailyBonusTeleportView : MonoBehaviour {
        public List<Image> teleports;

        public GameObject yelloCrystal;

        public GameObject brokenCrystal;

        public GameObject crystalOutline;

        public Button getNewTeleportButton;

        public Image fill;

        public GameObject upgradeTeleportView;

        public InactiveTeleportView inactiveTeleportView;

        public ActiveTeleportView activeTeleportView;

        public DetailTargetTeleportView detailTargetTeleportView;

        public GameObject brokenTeleport;

        public List<Image> lines;

        public Color activeColor;

        GameObject currentStateView;

        bool currentViewIsHiding;

        public Action<DailyBonusTeleportState> onStateChanged;

        public DailyBonusTeleportState State { get; private set; }

        public void OnEnable() {
            State = DailyBonusTeleportState.None;
            currentStateView = null;
            getNewTeleportButton.gameObject.SetActive(false);
            upgradeTeleportView.SetActive(false);
            inactiveTeleportView.gameObject.SetActive(false);
            activeTeleportView.gameObject.SetActive(false);
            detailTargetTeleportView.gameObject.SetActive(false);
            brokenTeleport.SetActive(false);
            fill.gameObject.SetActive(false);
            yelloCrystal.SetActive(false);
            brokenCrystal.SetActive(false);
        }

        public void ViewSelectedBonus(MapViewBonusElement element) {
            if (!currentViewIsHiding && activeTeleportView.gameObject == currentStateView) {
                activeTeleportView.ViewBonus(element);
            }
        }

        public void SetBrokenView() {
            foreach (Image teleport in teleports) {
                teleport.gameObject.SetActive(false);
            }

            brokenTeleport.SetActive(true);
            UpdateColorElements(DailyBonusTeleportState.Broken);
            ShowBrokenCrystal();
            ChangeState(getNewTeleportButton.gameObject, DailyBonusTeleportState.Broken);
        }

        public void SetUpgradableView(int zoneIndex) {
            SetTeleportCircleView(zoneIndex);
            UpdateColorElements(DailyBonusTeleportState.Upgradable);
            fill.gameObject.SetActive(true);
            fill.fillAmount = 1f;
            ShowYelloCrystal();
            ChangeState(upgradeTeleportView, DailyBonusTeleportState.Upgradable);
        }

        public void SetActiveView(int zoneIndex) {
            SetTeleportCircleView(zoneIndex);
            UpdateColorElements(DailyBonusTeleportState.Active);
            ShowYelloCrystal();
            ChangeState(activeTeleportView.gameObject, DailyBonusTeleportState.Active);
            activeTeleportView.UpdateView();
        }

        public void SetInactiveState(int zoneIndex, Date endDate, float durationInSec) {
            SetTeleportCircleView(zoneIndex);
            UpdateColorElements(DailyBonusTeleportState.Inactive);
            ShowYelloCrystal();
            bool successTeleportation = State == DailyBonusTeleportState.Active || State == DailyBonusTeleportState.DetailTarget;
            inactiveTeleportView.UpdateView(endDate, durationInSec, successTeleportation);
            ChangeState(inactiveTeleportView.gameObject, DailyBonusTeleportState.Inactive);
        }

        public void SetDetailTargetView(int zoneIndex, DailyBonusGarageItemReward detailGarageItem) {
            SetTeleportCircleView(zoneIndex);
            UpdateColorElements(DailyBonusTeleportState.DetailTarget);
            ShowYelloCrystal();
            ChangeState(detailTargetTeleportView.gameObject, DailyBonusTeleportState.DetailTarget);
            detailTargetTeleportView.UpdateView(detailGarageItem);
        }

        void ChangeState(GameObject newStateView, DailyBonusTeleportState newState) {
            if (currentStateView == null) {
                ShowStateView(newStateView);
            } else {
                currentStateView.GetComponent<AnimationEventListener>().SetHideHandler(delegate {
                    currentStateView.SetActive(false);
                    ShowStateView(newStateView);
                });

                currentStateView.GetComponent<Animator>().SetTrigger("hide");
                currentViewIsHiding = true;
            }

            if (State != newState) {
                State = newState;

                if (onStateChanged != null) {
                    onStateChanged(State);
                }
            }
        }

        void ShowStateView(GameObject newStateView) {
            newStateView.GetComponent<CanvasGroup>().alpha = 0f;
            newStateView.SetActive(true);
            currentStateView = newStateView;
            currentViewIsHiding = false;
        }

        void SetTeleportCircleView(int zoneIndex) {
            brokenTeleport.SetActive(false);
            zoneIndex = Math.Min(zoneIndex, teleports.Count - 1);

            if (!teleports[zoneIndex].gameObject.activeSelf) {
                foreach (Image teleport in teleports) {
                    teleport.gameObject.SetActive(false);
                }

                teleports[zoneIndex].gameObject.SetActive(true);
            }

            yelloCrystal.SetActive(true);
            SetZeroImageAlpha(yelloCrystal.GetComponent<Image>());
            yelloCrystal.GetComponent<Animator>().SetTrigger("show");

            brokenCrystal.GetComponent<AnimationEventListener>().SetHideHandler(delegate {
                brokenCrystal.SetActive(false);
            });

            brokenCrystal.GetComponent<Animator>().SetTrigger("hide");
        }

        void SetZeroImageAlpha(Image image) {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
        }

        void UpdateColorElements(DailyBonusTeleportState state) {
            switch (state) {
                case DailyBonusTeleportState.None:
                case DailyBonusTeleportState.Inactive:
                case DailyBonusTeleportState.Broken:
                    crystalOutline.GetComponent<Animator>().SetTrigger("hide");

                    crystalOutline.GetComponent<AnimationEventListener>().SetHideHandler(delegate {
                        crystalOutline.SetActive(false);
                    });

                    fill.gameObject.SetActive(false);
                    break;

                default:
                    crystalOutline.SetActive(true);
                    fill.gameObject.SetActive(true);
                    break;
            }

            foreach (Image teleport in teleports) {
                teleport.GetComponent<Animator>().SetBool("yello", IsChargedState(state));
            }

            foreach (Image line in lines) {
                line.GetComponent<Animator>().SetBool("yello", IsChargedState(state));
            }
        }

        bool IsChargedState(DailyBonusTeleportState state) =>
            state == DailyBonusTeleportState.Active || state == DailyBonusTeleportState.Upgradable || state == DailyBonusTeleportState.DetailTarget;

        void ShowBrokenCrystal() {
            SetZeroImageAlpha(brokenCrystal.GetComponent<Image>());
            brokenCrystal.SetActive(true);
            brokenCrystal.GetComponent<Animator>().SetTrigger("show");

            yelloCrystal.GetComponent<AnimationEventListener>().SetHideHandler(delegate {
                yelloCrystal.SetActive(false);
            });

            yelloCrystal.GetComponent<Animator>().SetTrigger("hide");
        }

        void ShowYelloCrystal() {
            if (!yelloCrystal.activeSelf) {
                yelloCrystal.SetActive(true);
            }

            brokenCrystal.GetComponent<Animator>().SetTrigger("hide");
            yelloCrystal.GetComponent<Animator>().SetTrigger("show");
        }
    }
}