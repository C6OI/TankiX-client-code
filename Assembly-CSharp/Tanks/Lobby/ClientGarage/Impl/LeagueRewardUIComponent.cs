using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LeagueRewardUIComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI currentLeagueTitle;

        [SerializeField] LeagueRewardListUIComponent leagueChestList;

        [SerializeField] LeagueRewardListUIComponent seasonRewardList;

        [SerializeField] TextMeshProUGUI leaguePoints;

        [SerializeField] LocalizedField leaguePointsCurrentMax;

        [SerializeField] LocalizedField leaguePlaceCurrentMax;

        [SerializeField] LocalizedField leaguePointsCurrent;

        [SerializeField] LocalizedField leaguePointsNotCurrent;

        [SerializeField] LocalizedField leagueAccusative;

        [SerializeField] LocalizedField seasonEndsIn;

        [SerializeField] LocalizedField seasonEndsDays;

        [SerializeField] LocalizedField seasonEndsHours;

        [SerializeField] LocalizedField seasonEndsMinutes;

        [SerializeField] LocalizedField seasonEndsSeconds;

        [SerializeField] TextMeshProUGUI seasonEndsInText;

        [SerializeField] GameObject seasonRewardsTitleLayout;

        [SerializeField] LocalizedField chestTooltipLocalization;

        [SerializeField] LocalizedField chestTooltipLowLeagueLocalization;

        [SerializeField] TooltipShowBehaviour chestTooltip;

        [SerializeField] GameObject tabsPanel;

        readonly Dictionary<long, double> lastUserReputationInLeagues = new();

        LeagueCarouselUIComponent carousel;

        long chestScoreLimit;

        double currentUserReputation;

        int leaguesCount;

        int selectedBar;

        Entity userLeague;

        public int PlaceInTopLeague { get; set; }

        public LeagueCarouselUIComponent Carousel {
            get {
                if (carousel == null) {
                    carousel = GetComponentInChildren<LeagueCarouselUIComponent>(true);
                    LeagueCarouselUIComponent leagueCarouselUIComponent = carousel;
                    leagueCarouselUIComponent.itemSelected = (CarouselItemSelected)Delegate.Combine(leagueCarouselUIComponent.itemSelected, new CarouselItemSelected(LeagueSelected));
                }

                return carousel;
            }
        }

        void OnEnable() {
            SetRadioButton(0);
        }

        void OnDestroy() {
            LeagueCarouselUIComponent leagueCarouselUIComponent = Carousel;
            leagueCarouselUIComponent.itemSelected = (CarouselItemSelected)Delegate.Remove(leagueCarouselUIComponent.itemSelected, new CarouselItemSelected(LeagueSelected));
        }

        public string GetSeasonEndsAsString(Date endDate) {
            float unityTime = endDate.UnityTime;
            float unityTime2 = Date.Now.UnityTime;
            float num = !(unityTime2 < unityTime) ? 0f : unityTime - unityTime2;
            int num2 = Mathf.FloorToInt(num / 86400f);
            int num3 = Mathf.FloorToInt((num - num2 * 24 * 60 * 60) / 3600f);
            int num4 = Mathf.FloorToInt((num - num2 * 24 * 60 * 60 - num3 * 60 * 60) / 60f);
            int num5 = Mathf.FloorToInt(num - num2 * 24 * 60 * 60 - num3 * 60 * 60 - num4 * 60);
            string text = seasonEndsIn;

            if (num2 > 0) {
                text = text + num2 + seasonEndsDays;
                return text + num3 + seasonEndsHours;
            }

            if (num3 > 0) {
                text = text + num3 + seasonEndsHours;
                return text + num4 + seasonEndsMinutes;
            }

            text = text + num4 + seasonEndsMinutes;
            return text + num5 + seasonEndsSeconds;
        }

        public void SetSeasonEndsInText(string endsIn) {
            seasonEndsInText.text = endsIn;
        }

        public void SetSeasonEndDate(Date endDate) {
            string seasonEndsAsString = GetSeasonEndsAsString(endDate);
            SetSeasonEndsInText(seasonEndsAsString);
        }

        public void SetChestScoreLimit(long score) {
            chestScoreLimit = score;
        }

        public void SetLeaguesCount(int count) {
            leaguesCount = count;
        }

        public void SelectUserLeague(Entity entity, double userReputation) {
            userLeague = entity;
            currentUserReputation = userReputation;
            Carousel.SelectItem(entity);
        }

        public LeagueTitleUIComponent AddLeagueItem(Entity entity) => Carousel.AddLeagueItem(entity);

        void LeagueSelected(LeagueTitleUIComponent selectedLeague) {
            FillInfo(selectedLeague);
            FillLeagueChest(selectedLeague);
            FillSeasonReward(selectedLeague);
            FillTooltip(selectedLeague);
        }

        public void PutReputationToEnter(long legueId, double reputation) {
            lastUserReputationInLeagues[legueId] = reputation;
        }

        public void UpdateLeagueRewardUI() {
            FillInfo(Carousel.CurrentLeague);
        }

        void FillInfo(LeagueTitleUIComponent selectedLeague) {
            bool flag = selectedLeague.LeagueEntity.Equals(userLeague);
            currentLeagueTitle.color = !flag ? Color.clear : Color.white;
            string text = string.Empty;
            double d = 0.0;
            int leagueIndex = selectedLeague.LeagueEntity.GetComponent<LeagueConfigComponent>().LeagueIndex;
            GetLeagueByIndexEvent getLeagueByIndexEvent = new(leagueIndex + 1);
            ScheduleEvent(getLeagueByIndexEvent, selectedLeague.LeagueEntity);
            Entity leagueEntity = getLeagueByIndexEvent.leagueEntity;

            if (leagueEntity != null) {
                text = leagueEntity.GetComponent<LeagueNameComponent>().NameAccusative;
                d = GetReputationToEnter(leagueEntity);
            }

            if (flag) {
                if (leagueIndex.Equals(leaguesCount - 1)) {
                    leaguePoints.text = string.Format(leaguePointsCurrentMax.Value, ToBoldText(Math.Truncate(currentUserReputation).ToString())) + "\n" +
                                        string.Format(leaguePlaceCurrentMax.Value, ToBoldText(PlaceInTopLeague.ToString()));
                } else {
                    leaguePoints.text = string.Format(leaguePointsCurrent.Value, "<color=white><b>" + Math.Truncate(currentUserReputation), Math.Truncate(d) + "</b></color>",
                        text + " " + leagueAccusative.Value);
                }
            } else {
                double reputationToEnter = GetReputationToEnter(selectedLeague.LeagueEntity);
                double num = reputationToEnter - currentUserReputation;
                leaguePoints.text = !(num > 0.0) ? string.Empty : string.Format(leaguePointsNotCurrent.Value, "<color=white><b>" + Math.Ceiling(num) + "</b></color>");
            }
        }

        string ToBoldText(string text) => "<color=white><b>" + text + "</b></color>";

        double GetReputationToEnter(Entity league) {
            double reputationToEnter = league.GetComponent<LeagueConfigComponent>().ReputationToEnter;
            return !lastUserReputationInLeagues.ContainsKey(league.Id) ? reputationToEnter : Math.Max(lastUserReputationInLeagues[league.Id], reputationToEnter);
        }

        void FillLeagueChest(LeagueTitleUIComponent selectedLeague) {
            leagueChestList.Clear();
            long chestId = selectedLeague.LeagueEntity.GetComponent<ChestBattleRewardComponent>().ChestId;
            AddItemToList(chestId, 1, leagueChestList);
        }

        void FillSeasonReward(LeagueTitleUIComponent selectedLeague) {
            seasonRewardList.Clear();
            bool active = false;
            bool active2 = false;
            Entity leagueEntity = selectedLeague.LeagueEntity;

            if (!leagueEntity.HasComponent<CurrentSeasonRewardForClientComponent>()) {
                Debug.LogWarning("League doesn't have reward!");
                return;
            }

            List<EndSeasonRewardItem> rewards = selectedLeague.LeagueEntity.GetComponent<CurrentSeasonRewardForClientComponent>().Rewards;

            if (rewards.Count > 0) {
                if (selectedBar > rewards.Count - 1) {
                    selectedBar = 0;
                    SetRadioButton(0);
                }

                active2 = rewards.Count > 1;
                List<DroppedItem> items = rewards[selectedBar].Items;

                if (items != null) {
                    active = items.Count > 0;

                    foreach (DroppedItem item in items) {
                        AddItemToList(item, seasonRewardList);
                    }
                }
            } else {
                Debug.LogWarning("End season rewards is empty");
            }

            seasonRewardsTitleLayout.SetActive(active);
            tabsPanel.SetActive(active2);
        }

        void FillTooltip(LeagueTitleUIComponent selectedLeague) {
            string text = string.Format(chestTooltipLocalization.Value, chestScoreLimit);

            if (userLeague.GetComponent<LeagueConfigComponent>().LeagueIndex < 3) {
                text += chestTooltipLowLeagueLocalization.Value;
            }

            chestTooltip.TipText = text;
        }

        void AddItemToList(long itemId, int count, LeagueRewardListUIComponent list) {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(itemId);
            DescriptionItemComponent component = entity.GetComponent<DescriptionItemComponent>();
            ImageItemComponent component2 = entity.GetComponent<ImageItemComponent>();
            string text = count <= 1 ? string.Empty : " x" + count;
            string text2 = string.Empty;

            if (!entity.HasComponent<ContainerMarkerComponent>()) {
                text2 = MarketItemNameLocalization.Instance.GetCategoryName(entity) + " ";
            }

            list.AddItem(text2 + component.Name + text + "\n" + component.Description, component2.SpriteUid);
        }

        void AddItemToList(DroppedItem item, LeagueRewardListUIComponent list) {
            AddItemToList(item.marketItemEntity.Id, item.Amount, list);
        }

        public void SelectBar(int value) {
            selectedBar = value;
            FillSeasonReward(Carousel.CurrentLeague);
            SetRadioButton(value);
        }

        void SetRadioButton(int value) {
            RadioButton[] componentsInChildren = GetComponentsInChildren<RadioButton>(true);

            if (componentsInChildren.Length > value) {
                componentsInChildren[componentsInChildren.Length - 1 - value].Activate();
            }
        }
    }
}