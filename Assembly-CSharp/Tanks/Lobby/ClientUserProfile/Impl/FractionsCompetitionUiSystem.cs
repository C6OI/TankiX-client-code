using System;
using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionsCompetitionUiSystem : ECSSystem {
        [OnEventFire]
        public void HideOrShowCompetitionElements(NodeAddedEvent e, SingleNode<FractionsCompetitionHideObjectsComponent> objects, [JoinAll] Optional<FractionCompetitionNode> competition,
            [JoinAll] Optional<SelfUserWithFractionNode> userWithFraction, [JoinAll] ICollection<InvolvedFractionNode> fractionsInCompetition) {
            bool flag = competition.IsPresent();
            bool flag2 = userWithFraction.IsPresent();
            bool flag3 = fractionsInCompetition.Count > 0;
            bool active = flag && (flag2 || flag3);
            GameObject[] objectsToHide = objects.component.ObjectsToHide;

            foreach (GameObject gameObject in objectsToHide) {
                gameObject.SetActive(active);
            }
        }

        [OnEventFire]
        public void RewardNotification(NodeAddedEvent e, FractionCompetitionDialogNode popup, [JoinByUser] RewardNotificationNode notification,
            [JoinAll] FractionCompetitionNode fractionCompetition, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            FractionsCompetitionInfoComponent fractionsCompetitionInfo = fractionCompetition.fractionsCompetitionInfo;
            Dictionary<long, int> rewards = notification.fractionsCompetitionRewardNotification.Rewards;
            bool flag = notification.fractionsCompetitionRewardNotification.CrysForWin > 0;
            PopupDialogComponent popupDialog = popup.popupDialog;
            popupDialog.headerText.text = fractionsCompetitionInfo.CompetitionTitle;
            popupDialog.rewardHeader.text = fractionsCompetitionInfo.HereYourRewardMessage;
            popupDialog.buttonText.text = fractionsCompetitionInfo.RewardsButtonText;
            popupDialog.text.text = !flag ? fractionsCompetitionInfo.LoserFinishMessage : fractionsCompetitionInfo.WinnerFinishMessage;
            FractionInfoComponent component = GetEntityById(notification.fractionsCompetitionRewardNotification.WinnerFractionId).GetComponent<FractionInfoComponent>();
            string fractionLogoImageUid = component.FractionLogoImageUid;
            popupDialog.leagueIcon.SpriteUid = fractionLogoImageUid;
            popupDialog.leagueIcon.GetComponent<Image>().preserveAspect = true;
            popupDialog.itemsContainer.DestroyChildren();
            List<LeagueEntranceItemComponent> list = new();

            foreach (KeyValuePair<long, int> item in rewards) {
                LeagueEntranceItemComponent leagueEntranceItemComponent = Object.Instantiate(popupDialog.itemPrefab, popupDialog.itemsContainer, false);
                Entity entityById = GetEntityById(item.Key);
                leagueEntranceItemComponent.preview.SpriteUid = entityById.GetComponent<ImageItemComponent>().SpriteUid;
                long num = !entityById.HasComponent<CrystalItemComponent>() ? item.Value : notification.fractionsCompetitionRewardNotification.CrysForWin + item.Value;
                bool flag2 = num > 1;
                leagueEntranceItemComponent.text.text = entityById.GetComponent<DescriptionItemComponent>().Name + (!flag2 ? string.Empty : " x");

                if (flag2) {
                    leagueEntranceItemComponent.count.Value = num;
                    leagueEntranceItemComponent.count.gameObject.SetActive(true);
                } else {
                    leagueEntranceItemComponent.count.gameObject.SetActive(false);
                }

                leagueEntranceItemComponent.gameObject.SetActive(true);
                list.Add(leagueEntranceItemComponent);
            }

            list.Sort((LeagueEntranceItemComponent a, LeagueEntranceItemComponent b) => a.count.Value.CompareTo(b.count.Value));
            AnimationTriggerDelayBehaviour component2 = popupDialog.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>();

            for (int i = 0; i < list.Count; i++) {
                component2.dealy = (float)(i + 1) * popupDialog.itemsShowDelay;
                list[i].transform.SetAsLastSibling();
            }

            popup.fractionsCompetitionNotificationDialog.OpenFractionsWindowButton.WillOpen = false;
            List<Animator> animators = !screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators;
            popupDialog.Show(animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<FractionsCompetitionButtonComponent> button, [JoinAll] [Combine] RewardNotificationNode notification,
            [JoinAll] FractionCompetitionDialogNode popup) {
            popup.popupDialog.Hide();
            ScheduleEvent<NotificationShownEvent>(notification);
        }

        [OnEventComplete]
        public void FractionCompetitionStarted(NodeAddedEvent e, FractionCompetitionDialogNode popup, [Combine] [JoinByUser] StartNotificationNode notification,
            [JoinAll] FractionCompetitionNode fractionCompetition, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            FractionsCompetitionInfoComponent fractionsCompetitionInfo = fractionCompetition.fractionsCompetitionInfo;
            PopupDialogComponent popupDialog = popup.popupDialog;
            popupDialog.headerText.text = fractionsCompetitionInfo.CompetitionTitle;
            popupDialog.text.text = fractionsCompetitionInfo.CompetitionStartMessage;
            popupDialog.rewardHeader.text = fractionsCompetitionInfo.MainQuestionMessage;
            popupDialog.buttonText.text = fractionsCompetitionInfo.TakePartButtonText;
            popupDialog.leagueIcon.SpriteUid = fractionsCompetitionInfo.CompetitionLogoUid;
            popupDialog.leagueIcon.GetComponent<Image>().preserveAspect = true;
            popupDialog.itemsContainer.DestroyChildren();
            AnimationTriggerDelayBehaviour component = popupDialog.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>();
            int num = 0;

            foreach (long key in fractionCompetition.fractionsCompetitionScores.Scores.Keys) {
                FractionInfoComponent component2 = GetEntityById(key).GetComponent<FractionInfoComponent>();
                string fractionName = component2.FractionName;
                string fractionLogoImageUid = component2.FractionLogoImageUid;
                component.dealy = (float)(num++ + 1) * popupDialog.itemsShowDelay;
                LeagueEntranceItemComponent leagueEntranceItemComponent = Object.Instantiate(popupDialog.itemPrefab, popupDialog.itemsContainer, false);
                leagueEntranceItemComponent.preview.SpriteUid = fractionLogoImageUid;
                leagueEntranceItemComponent.text.text = fractionName;
                leagueEntranceItemComponent.gameObject.SetActive(true);
            }

            popup.fractionsCompetitionNotificationDialog.OpenFractionsWindowButton.WillOpen = true;
            List<Animator> animators = !screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators;
            popupDialog.Show(animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<FractionsCompetitionButtonComponent> button, [JoinAll] [Combine] StartNotificationNode notification,
            [JoinAll] FractionCompetitionDialogNode popup) {
            popup.popupDialog.Hide();
            ScheduleEvent<NotificationShownEvent>(notification);
        }

        [OnEventFire]
        public void ShowDialog(OpenFractionsCompetitionDialogEvent e, [JoinAll] SelfUserNode user, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] ICollection<InvolvedFractionNode> fractionsInCompetition) {
            FractionsCompetitionDialogComponent fractionsCompetitionDialogComponent = dialogs.component.Get<FractionsCompetitionDialogComponent>();
            fractionsCompetitionDialogComponent.Show();
            bool flag = !user.Entity.HasComponent<FractionGroupComponent>();

            if (fractionsInCompetition.Count > 0 && flag) {
                fractionsCompetitionDialogComponent.FractionSelectWindow.gameObject.SetActive(true);
            } else {
                fractionsCompetitionDialogComponent.CurrentCompetitionWindow.gameObject.SetActive(true);
            }
        }

        [OnEventFire]
        public void OnFractionCompetitionButton(ButtonClickEvent e, SingleNode<FractionsCompetitionButtonComponent> button) {
            if (button.component.WillOpen) {
                ScheduleEvent<OpenFractionsCompetitionDialogEvent>(button);
            }
        }

        [OnEventFire]
        public void OnFractionButton(ButtonClickEvent e, SingleNode<FractionButtonComponent> button, [JoinAll] SingleNode<FractionsCompetitionDialogComponent> dialog,
            [JoinAll] SelfUserNode user) {
            Entity fractionEntity = button.component.FractionEntity;

            switch (button.component.Action) {
                case FractionButtonComponent.FractionActions.SELECT:
                    if (!user.Entity.HasComponent<FractionGroupComponent>()) {
                        NewEvent<ApplyPlayerToFractionEvent>().Attach(fractionEntity).Attach(user.Entity).Schedule();
                    }

                    break;

                case FractionButtonComponent.FractionActions.AWARDS: {
                    FractionInfoComponent component = fractionEntity.GetComponent<FractionInfoComponent>();
                    CompetitionAwardWindowComponent competitionAwardWindow = dialog.component.CompetitionAwardWindow;
                    competitionAwardWindow.gameObject.SetActive(true);
                    competitionAwardWindow.FractionName = component.FractionName;
                    competitionAwardWindow.FractionRewardDescription = component.FractionRewardDescription;
                    competitionAwardWindow.RewardImageUid = component.FractionRewardImageUid;
                    competitionAwardWindow.FractionColor = GetColorFromHex(component.FractionColor);
                    break;
                }

                case FractionButtonComponent.FractionActions.LEARN_MORE:
                    dialog.component.LearnMoreWindow.gameObject.SetActive(true);
                    break;
            }
        }

        [OnEventFire]
        public void OnUserWithFractionAdded(NodeAddedEvent e, SelfUserWithFractionNode user, [JoinAll] SingleNode<FractionsCompetitionDialogComponent> dialog,
            [JoinAll] SingleNode<FractionSelectWindowComponent> window) {
            dialog.component.FractionSelectWindow.gameObject.SetActive(false);
            dialog.component.CurrentCompetitionWindow.gameObject.SetActive(true);
        }

        [OnEventFire]
        public void FillFractionSelectWindow(NodeAddedEvent e, SingleNode<FractionSelectWindowComponent> window, [JoinAll] ICollection<InvolvedFractionNode> fractionsInCompetition) {
            Transform transform = window.component.FractionDescriptionContainer.transform;
            FractionDescriptionBehaviour fractionDescriptionPrefab = window.component.FractionDescriptionPrefab;
            List<InvolvedFractionNode> list = fractionsInCompetition.ToList();
            list.Sort((InvolvedFractionNode a, InvolvedFractionNode b) => a.fractionInvolvedInCompetition.UserCount.CompareTo(b.fractionInvolvedInCompetition.UserCount));
            transform.DestroyChildren();

            foreach (InvolvedFractionNode item in list) {
                FractionDescriptionBehaviour fractionDescriptionBehaviour = Object.Instantiate(fractionDescriptionPrefab, transform);
                fractionDescriptionBehaviour.FractionTitle = item.fractionInfo.FractionName;
                fractionDescriptionBehaviour.FractionSlogan = item.fractionInfo.FractionSlogan;
                fractionDescriptionBehaviour.FractionDescription = item.fractionInfo.FractionDescription;
                fractionDescriptionBehaviour.LogoUid = item.fractionInfo.FractionLogoImageUid;
                fractionDescriptionBehaviour.FractionId = item.Entity;
                fractionDescriptionBehaviour.gameObject.SetActive(true);
            }
        }

        [OnEventFire]
        public void FillCurrentCompetitionWindow(NodeAddedEvent e, SingleNode<CurrentCompetitionWindowComponent> window, [JoinAll] Optional<SelfUserWithFractionNode> user) {
            window.component.PlayerInfoElement.gameObject.SetActive(user.IsPresent());
        }

        [OnEventFire]
        public void FillFractionRewardWindow(NodeAddedEvent e, SingleNode<FractionRewardUiComponent> window, [JoinAll] SelfUserWithFractionNode user,
            [JoinByFraction] FractionNode fraction) {
            window.component.RewardImageUid = fraction.fractionInfo.FractionRewardImageUid;
        }

        [OnEventFire]
        public void FillFactionContainer(NodeAddedEvent e, SingleNode<FractionContainerComponent> container, [JoinAll] SelfUserNode user, [JoinByFraction] Optional<FractionNode> fraction,
            [JoinAll] FractionCompetitionNode fractionCompetition) {
            FractionContainerComponent component = container.component;

            switch (component.Target) {
                case FractionContainerComponent.FractionContainerTargets.PLAYER_FRACTION: {
                    if (!fraction.IsPresent()) {
                        component.IsAvailable = false;
                        break;
                    }

                    component.IsAvailable = true;
                    FractionInfoComponent fractionInfo = fraction.Get().fractionInfo;
                    component.FractionTitle = fractionInfo.FractionName;
                    component.FractionLogoUid = fractionInfo.FractionLogoImageUid;
                    component.FractionColor = GetColorFromHex(fractionInfo.FractionColor);
                    break;
                }

                case FractionContainerComponent.FractionContainerTargets.WINNER_FRACTION: {
                    if (!fractionCompetition.Entity.HasComponent<FinishedFractionsCompetitionComponent>()) {
                        component.IsAvailable = false;
                        break;
                    }

                    component.IsAvailable = true;
                    Entity winner = fractionCompetition.Entity.GetComponent<FinishedFractionsCompetitionComponent>().Winner;
                    FractionInfoComponent component2 = winner.GetComponent<FractionInfoComponent>();
                    component.FractionTitle = component2.FractionName;
                    component.FractionLogoUid = component2.FractionLogoImageUid;
                    component.FractionColor = GetColorFromHex(component2.FractionColor);
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [OnEventFire]
        public void FillLearnMoreWindow(NodeAddedEvent e, SingleNode<FractionLearnMoreWindowComponent> window, [JoinAll] FractionCompetitionNode competition) {
            FractionsCompetitionInfoComponent fractionsCompetitionInfo = competition.fractionsCompetitionInfo;
            window.component.CompetitionTitle = fractionsCompetitionInfo.CompetitionTitle;
            window.component.CompetitionDescription = fractionsCompetitionInfo.CompetitionDescription;
            window.component.CompetitionLogoUid = fractionsCompetitionInfo.CompetitionLogoUid;
        }

        [OnEventFire]
        public void UpdateUserScores(NodeAddedEvent e, SingleNode<FractionUserScoreUiComponent> scores, [JoinAll] SelfUserNode user) {
            scores.component.Scores = user.fractionUserScore.TotalEarnedPoints;
        }

        [OnEventFire]
        public void UpdateFractionsScores(NodeAddedEvent e, SingleNode<FractionScoresContainerComponent> container, [JoinAll] FractionCompetitionNode competition) {
            container.component.TotalCryFund = competition.fractionsCompetitionScores.TotalCryFund;

            foreach (long key in competition.fractionsCompetitionScores.Scores.Keys) {
                Entity entityById = GetEntityById(key);
                long scores = competition.fractionsCompetitionScores.Scores[key];
                FractionInfoComponent component = entityById.GetComponent<FractionInfoComponent>();
                container.component.UpdateScores(key, component, scores);
            }

            SelectWinner(competition.Entity, container.component);
        }

        [OnEventComplete]
        public void UpdateFractionsScores(UpdateClientFractionScoresEvent e, FractionCompetitionNode competition,
            [JoinAll] ICollection<SingleNode<FractionScoresContainerComponent>> containers) {
            foreach (long key in competition.fractionsCompetitionScores.Scores.Keys) {
                Entity entityById = GetEntityById(key);
                long scores = competition.fractionsCompetitionScores.Scores[key];
                FractionInfoComponent component = entityById.GetComponent<FractionInfoComponent>();

                foreach (SingleNode<FractionScoresContainerComponent> container in containers) {
                    container.component.TotalCryFund = competition.fractionsCompetitionScores.TotalCryFund;
                    container.component.UpdateScores(key, component, scores);
                    SelectWinner(competition.Entity, container.component);
                }
            }
        }

        [OnEventFire]
        public void ChangeColorOnFractionAdded(NodeAddedEvent e, SelfUserWithFractionNode user, [JoinByFraction] FractionNode fraction,
            [Combine] [JoinAll] SingleNode<FractionImageColorComponent> image) {
            string fractionColor = fraction.fractionInfo.FractionColor;
            Color defaultColor = image.component.DefaultColor;
            TryToRecolorImage(image.component.ImagesToRecolor, fractionColor, defaultColor);
        }

        [OnEventFire]
        public void ClearColorOnRemoved(NodeRemoveEvent e, SelfUserWithFractionNode user, [JoinByFraction] FractionNode fraction,
            [Combine] [JoinAll] SingleNode<FractionImageColorComponent> image) {
            RecolorAllImages(image.component.ImagesToRecolor, image.component.DefaultColor);
        }

        [OnEventFire]
        public void RefreshFractionColor(NodeAddedEvent e, SingleNode<FractionImageColorComponent> image, [JoinAll] SelfUserNode user, [JoinByFraction] Optional<FractionNode> fraction) {
            if (fraction.IsPresent()) {
                string fractionColor = fraction.Get().fractionInfo.FractionColor;
                Color defaultColor = image.component.DefaultColor;
                TryToRecolorImage(image.component.ImagesToRecolor, fractionColor, defaultColor);
            } else {
                RecolorAllImages(image.component.ImagesToRecolor, image.component.DefaultColor);
            }
        }

        [OnEventFire]
        public void RegisterFinished(NodeAddedEvent e, SingleNode<FinishedFractionsCompetitionComponent> node) { }

        void SelectWinner(Entity competitionEntity, FractionScoresContainerComponent container) {
            container.WinnerId = !competitionEntity.HasComponent<FinishedFractionsCompetitionComponent>() ? 0
                                     : competitionEntity.GetComponent<FinishedFractionsCompetitionComponent>().Winner.Id;
        }

        void TryToRecolorImage(Image[] imagesToRecolor, string fractionHexColor, Color defaultColor) {
            Color color;
            bool flag = ColorUtility.TryParseHtmlString(fractionHexColor, out color);
            RecolorAllImages(imagesToRecolor, !flag ? defaultColor : color);
        }

        void RecolorAllImages(Image[] images, Color color) {
            foreach (Image image in images) {
                image.color = color;
            }
        }

        public static Color GetColorFromHex(string colorHex, Color defaultColor = default) {
            Color color;
            return !ColorUtility.TryParseHtmlString(colorHex, out color) ? defaultColor : color;
        }

        public class SelfUserNode : Node {
            public FractionUserScoreComponent fractionUserScore;

            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserRankComponent userRank;
        }

        public class FractionCompetitionNode : Node {
            public FractionsCompetitionInfoComponent fractionsCompetitionInfo;

            public FractionsCompetitionScoresComponent fractionsCompetitionScores;
        }

        public class FractionCompetitionDialogNode : Node {
            public FractionsCompetitionNotificationDialogComponent fractionsCompetitionNotificationDialog;

            public PopupDialogComponent popupDialog;
        }

        public class SelfUserWithFractionNode : SelfUserNode {
            public FractionGroupComponent fractionGroup;
        }

        public class FractionNode : Node {
            public FractionComponent fraction;

            public FractionGroupComponent fractionGroup;

            public FractionInfoComponent fractionInfo;
        }

        public class InvolvedFractionNode : FractionNode {
            public FractionInvolvedInCompetitionComponent fractionInvolvedInCompetition;
        }

        public class StartNotificationNode : Node {
            public FractionsCompetitionStartNotificationComponent fractionsCompetitionStartNotification;

            public ResourceDataComponent resourceData;
        }

        public class RewardNotificationNode : Node {
            public FractionsCompetitionRewardNotificationComponent fractionsCompetitionRewardNotification;

            public ResourceDataComponent resourceData;
        }
    }
}