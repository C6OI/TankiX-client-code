using System;
using Tanks.Lobby.ClientUserProfile.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ActiveTeleportView : MonoBehaviour {
        public Image fill;

        public TextMeshProUGUI text;

        public MultipleBonusView cryBonusView;

        public MultipleBonusView xCryBonusView;

        public MultipleBonusView energyBonusView;

        public ContainerBonusView containerBonusView;

        public DetailBonusView detailBonusView;

        MapViewBonusElement currentBonusElement;

        GameObject currentView;

        void OnEnable() {
            UpdateView();
        }

        public void UpdateView() {
            fill.gameObject.SetActive(true);
            fill.fillAmount = 1f;
            ActivateAll();
            ShowView(text.gameObject);
        }

        public void ViewBonus(MapViewBonusElement element) {
            currentBonusElement = element;

            currentView.GetComponent<AnimationEventListener>().SetHideHandler(delegate {
                if (currentBonusElement == null) {
                    ShowView(text.gameObject);
                } else {
                    switch (currentBonusElement.dailyBonusData.DailyBonusType) {
                        case DailyBonusType.CRY:
                            ShowView(cryBonusView.gameObject);
                            cryBonusView.UpdateView(currentBonusElement.dailyBonusData.CryAmount);
                            break;

                        case DailyBonusType.CONTAINER:
                            ShowView(containerBonusView.gameObject);
                            containerBonusView.UpdateView(currentBonusElement.dailyBonusData.ContainerReward);
                            break;

                        case DailyBonusType.DETAIL:
                            ShowView(detailBonusView.gameObject);
                            detailBonusView.UpdateView(currentBonusElement.dailyBonusData.DetailReward);
                            break;

                        case DailyBonusType.XCRY:
                            ShowView(xCryBonusView.gameObject);
                            xCryBonusView.UpdateView(currentBonusElement.dailyBonusData.XcryAmount);
                            break;

                        case DailyBonusType.ENERGY:
                            ShowView(energyBonusView.gameObject);
                            energyBonusView.UpdateView(currentBonusElement.dailyBonusData.EnergyAmount);
                            break;

                        case DailyBonusType.NONE:
                            throw new ArgumentException("Unexpected DailyBonusType.NONE ");
                    }
                }
            });

            HideCurrentView();
        }

        void HideCurrentView() {
            currentView.GetComponent<Animator>().SetTrigger("hide");
        }

        void ActivateAll() {
            text.gameObject.SetActive(true);
            cryBonusView.gameObject.SetActive(true);
            xCryBonusView.gameObject.SetActive(true);
            energyBonusView.gameObject.SetActive(true);
            containerBonusView.gameObject.SetActive(true);
            detailBonusView.gameObject.SetActive(true);
        }

        void ShowView(GameObject view) {
            view.GetComponent<Animator>().SetTrigger("show");
            currentView = view;
        }
    }
}