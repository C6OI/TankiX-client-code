using System;
using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;
using TMPro;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DailyBonusScreenComponent : BehaviourComponent {
        public DailyBonusMapView mapView;

        public DailyBonusTeleportView teleportView;

        public TeleportHeaderView teleportHeaderView;

        public Button takeBonusButton;

        public Button takeContainerButton;

        public Button takeDetailTarget;

        public CellsProgressBar cellsProgressBar;

        public LocalizedField noItemsFound;

        public LocalizedField itemsFound;

        public LocalizedField allItemsFound;

        public TextMeshProUGUI foundItemsLabel;

        public DailyBonusGarageItemReward completeDetailGarageItem;

        DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode;

        bool needUpdate;

        DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public void Awake() {
            takeBonusButton.interactable = mapView.selectedBonusElement != null;
            takeContainerButton.interactable = mapView.selectedBonusElement != null;
            takeDetailTarget.interactable = mapView.selectedBonusElement != null;
            DailyBonusMapView dailyBonusMapView = mapView;

            dailyBonusMapView.onSelectedBonusElementChanged = (Action<MapViewBonusElement>)Delegate.Combine(dailyBonusMapView.onSelectedBonusElementChanged,
                new Action<MapViewBonusElement>(UpdateTakeBonusButtonInteractable));

            DailyBonusMapView dailyBonusMapView2 = mapView;

            dailyBonusMapView2.onSelectedBonusElementChanged =
                (Action<MapViewBonusElement>)Delegate.Combine(dailyBonusMapView2.onSelectedBonusElementChanged, new Action<MapViewBonusElement>(teleportView.ViewSelectedBonus));

            DailyBonusTeleportView dailyBonusTeleportView = teleportView;

            dailyBonusTeleportView.onStateChanged =
                (Action<DailyBonusTeleportState>)Delegate.Combine(dailyBonusTeleportView.onStateChanged, new Action<DailyBonusTeleportState>(mapView.UpdateInteractable));

            mapView.UpdateInteractable(teleportView.State);
        }

        void Update() {
            if (needUpdate) {
                UpdateView();
                needUpdate = false;
            }

            if (InputMapping.Cancel) {
                Hide();
            } else if (teleportView.State == DailyBonusTeleportState.Inactive) {
                Date date = userDailyBonusNode.userDailyBonusNextReceivingDate.Date;

                if (date <= Date.Now) {
                    SetActiveOrUpgradableTeleportView(userDailyBonusNode, dailyBonusConfigNode);
                }
            }
        }

        public void Show() {
            MainScreenComponent.Instance.OverrideOnBack(Hide);
            gameObject.SetActive(true);
        }

        void UpdateFoundItemsLabel(DailyBonusCycleComponent cycle) {
            string text = string.Empty;

            if (userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards.Count < cycle.DailyBonuses.Length) {
                text = itemsFound;

                if (userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards.Count == 0) {
                    text = noItemsFound;
                }
            } else if (userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards.Count == cycle.DailyBonuses.Length) {
                text = allItemsFound;
            }

            foundItemsLabel.SetText(text.ToUpper());
        }

        public void Hide() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            gameObject.SetActive(false);
        }

        public void UpdateView(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode) {
            this.userDailyBonusNode = userDailyBonusNode;
            this.dailyBonusConfigNode = dailyBonusConfigNode;
            UpdateView();
        }

        public void UpdateViewInNextFrame(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode) {
            this.userDailyBonusNode = userDailyBonusNode;
            this.dailyBonusConfigNode = dailyBonusConfigNode;
            needUpdate = true;
        }

        public void DisableAllButtons() {
            takeBonusButton.interactable = false;
            takeDetailTarget.interactable = false;
            takeContainerButton.interactable = false;
            GetComponentsInChildren<UpgradeTeleportButtonComponent>(true)[0].GetComponent<Button>().interactable = false;
            GetComponentsInChildren<GetNewTeleportButtonComponent>(true)[0].GetComponent<Button>().interactable = false;
        }

        void UpdateView() {
            DailyBonusCycleComponent cycle = GetCycle(userDailyBonusNode, dailyBonusConfigNode);
            cellsProgressBar.Init(cycle.DailyBonuses.Length, cycle.DailyBonuses, userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards);
            mapView.UpdateView(cycle, userDailyBonusNode);
            UpdateTeleport(userDailyBonusNode, dailyBonusConfigNode);
            UpdateFoundItemsLabel(cycle);
        }

        void UpdateTeleport(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode) {
            int zoneIndex = (int)userDailyBonusNode.userDailyBonusZone.ZoneNumber;
            completeDetailGarageItem = GetCompleteUntakenDetailTargetItem(userDailyBonusNode, dailyBonusConfigNode);

            if (completeDetailGarageItem != null) {
                teleportView.SetDetailTargetView(zoneIndex, completeDetailGarageItem);
                takeDetailTarget.gameObject.SetActive(true);
                takeBonusButton.gameObject.SetActive(false);
                takeContainerButton.gameObject.SetActive(false);
                takeDetailTarget.interactable = true;
                return;
            }

            takeDetailTarget.gameObject.SetActive(false);
            UpdateTakeBonusButtonInteractable(mapView.selectedBonusElement);

            if (UserTookAllBonuses(userDailyBonusNode, dailyBonusConfigNode)) {
                teleportView.SetBrokenView();
                teleportHeaderView.SetBrokenView();
                return;
            }

            teleportHeaderView.UpdateView(zoneIndex);
            Date date = userDailyBonusNode.userDailyBonusNextReceivingDate.Date;

            if (date <= Date.Now) {
                SetActiveOrUpgradableTeleportView(userDailyBonusNode, dailyBonusConfigNode);
                return;
            }

            float durationInSec = userDailyBonusNode.userDailyBonusNextReceivingDate.TotalMillisLength / 1000f;
            teleportView.SetInactiveState(zoneIndex, date, durationInSec);
        }

        void SetActiveOrUpgradableTeleportView(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode) {
            int num = (int)userDailyBonusNode.userDailyBonusZone.ZoneNumber;

            if (UserTookAllBonusesInCurrentZone(userDailyBonusNode, dailyBonusConfigNode)) {
                teleportView.SetUpgradableView(num);
            } else {
                teleportView.SetActiveView(num);
            }
        }

        DailyBonusGarageItemReward GetCompleteUntakenDetailTargetItem(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode,
            DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode) {
            DailyBonusCycleComponent cycle = GetCycle(userDailyBonusNode, dailyBonusConfigNode);
            int num = cycle.Zones[userDailyBonusNode.userDailyBonusZone.ZoneNumber];
            DailyBonusData[] dailyBonuses = cycle.DailyBonuses;
            List<long> receivedRewards = userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards;

            for (int i = 0; i <= num; i++) {
                DailyBonusData dailyBonusData = dailyBonuses[i];

                if (receivedRewards.Contains(dailyBonusData.Code) && dailyBonusData.DailyBonusType == DailyBonusType.DETAIL) {
                    DetailItem item = GarageItemsRegistry.GetItem<DetailItem>(dailyBonusData.DetailReward.MarketItemId);

                    if (item.Count == item.RequiredCount) {
                        return dailyBonusData.DetailReward;
                    }
                }
            }

            return null;
        }

        bool UserTookAllBonusesInCurrentZone(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode) {
            DailyBonusCycleComponent cycle = GetCycle(userDailyBonusNode, dailyBonusConfigNode);
            int num = cycle.Zones[userDailyBonusNode.userDailyBonusZone.ZoneNumber];
            DailyBonusData[] dailyBonuses = cycle.DailyBonuses;
            List<long> receivedRewards = userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards;

            for (int i = 0; i <= num; i++) {
                if (!receivedRewards.Contains(dailyBonuses[i].Code)) {
                    return false;
                }
            }

            return true;
        }

        bool UserTookAllBonuses(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode) {
            DailyBonusCycleComponent cycle = GetCycle(userDailyBonusNode, dailyBonusConfigNode);
            return userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards.Count.Equals(cycle.DailyBonuses.Length);
        }

        void UpdateTakeBonusButtonInteractable(MapViewBonusElement bonusElement) {
            takeContainerButton.gameObject.SetActive(true);
            takeBonusButton.gameObject.SetActive(true);
            bool flag = bonusElement != null;
            takeBonusButton.interactable = flag;
            takeContainerButton.interactable = flag;

            if (GetCompleteUntakenDetailTargetItem(userDailyBonusNode, dailyBonusConfigNode) != null) {
                takeContainerButton.gameObject.SetActive(false);
                takeBonusButton.gameObject.SetActive(false);
                takeDetailTarget.interactable = true;
            } else if (!flag) {
                takeContainerButton.gameObject.SetActive(false);
            } else {
                bool flag2 = bonusElement.dailyBonusData.DailyBonusType == DailyBonusType.CONTAINER;
                takeContainerButton.gameObject.SetActive(flag2);
                takeBonusButton.gameObject.SetActive(!flag2);
            }
        }

        public DailyBonusCycleComponent GetCycle(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, DailyBonusScreenSystem.DailyBonusConfig dailyBonusConfigNode) {
            if (userDailyBonusNode.userDailyBonusCycle.CycleNumber.Equals(0L)) {
                return dailyBonusConfigNode.dailyBonusFirstCycle;
            }

            return dailyBonusConfigNode.dailyBonusEndlessCycle;
        }
    }
}