using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using tanks.modules.lobby.ClientGarage.Impl;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialShowSystem : ECSSystem {
        public TutorialNode activeTutorial;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void StepAdded(NodeAddedEvent e, SingleNode<TutorialShowTriggerComponent> tutorialTrigger, [JoinAll] ICollection<TutorialNode> tutorials) {
            if (tutorials.Count == 0) {
                return;
            }

            bool flag = false;
            bool flag2 = false;

            foreach (TutorialNode tutorial in tutorials) {
                if (tutorial.tutorialData.TutorialId != tutorialTrigger.component.TutorialId) {
                    continue;
                }

                flag = tutorial.Entity.HasComponent<TutorialCompleteComponent>();
                IList<TutorialStepNode> list = Select<TutorialStepNode>(tutorial.Entity, typeof(TutorialGroupComponent));

                foreach (TutorialStepNode item in list) {
                    if (item.tutorialStepData.StepId == tutorialTrigger.component.StepId) {
                        flag2 = true;
                        break;
                    }
                }

                break;
            }

            if (flag || !flag2) {
                tutorialTrigger.component.DestroyTrigger();
                Log.InfoFormat("TutorialShowTriggerComponent added, step {0} not found", tutorialTrigger.component.StepId);
            } else if (tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.ClickAnyWhere &&
                       tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.CustomTrigger) {
                tutorialTrigger.component.Triggered();
            }
        }

        [OnEventFire]
        public void ShowTutorialOnEulaClose(NodeRemoveEvent e, SingleNode<EulaNotificationComponent> eula, [JoinAll] SingleNode<MainScreenComponent> screen) {
            NewEvent<TriggerOnEulaCloseEvent>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowTutorialOnPPClose(NodeRemoveEvent e, SingleNode<PrivacyPolicyNotificationComponent> pp, [JoinAll] SingleNode<MainScreenComponent> screen) {
            NewEvent<TriggerOnEulaCloseEvent>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowTutorialOnEulaClose(TriggerOnEulaCloseEvent e, Node any, [JoinAll] [Combine] SingleNode<TutorialShowTriggerComponent> tutorialTrigger) {
            if (tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.ClickAnyWhere &&
                tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.CustomTrigger) {
                tutorialTrigger.component.Triggered();
            }
        }

        [OnEventFire]
        public void ShowTutorialOnLoginRewardClose(NodeRemoveEvent e, SingleNode<LoginRewardsNotificationComponent> loginRewardNotification,
            [JoinAll] SingleNode<MainScreenComponent> screen) {
            NewEvent<TriggerOnEulaCloseEvent>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void CheckForActiveNotifications(CheckForActiveNotificationsEvent e, Node any,
            [JoinAll] ICollection<SingleNode<ReleaseGiftsNotificationComponent>> releaseGiftsNotification, [JoinAll] ICollection<SingleNode<EulaNotificationComponent>> eulaNotification,
            [JoinAll] ICollection<SingleNode<PrivacyPolicyNotificationComponent>> privacyPolicyNotification,
            [JoinAll] ICollection<SingleNode<LoginRewardsNotificationComponent>> loginRewardNotification) {
            e.NotificationExist = releaseGiftsNotification.Count > 0 || eulaNotification.Count > 0 || loginRewardNotification.Count > 0 || privacyPolicyNotification.Count > 0;
        }

        [OnEventFire]
        public void PauseTutorialOnLoginReward(NodeAddedEvent e, SingleNode<LoginRewardDialog> loginRewardDialog) {
            if (TutorialCanvas.Instance.IsShow) {
                TutorialCanvas.Instance.Pause();
            }
        }

        [OnEventFire]
        public void ContinueTutorialOnLoginRewardRemoved(NodeRemoveEvent e, SingleNode<LoginRewardDialog> loginRewardDialog) {
            TutorialCanvas.Instance.Continue();
        }

        [OnEventFire]
        public void TryShowTutorial(TutorialTriggerEvent e, SingleNode<TutorialShowTriggerComponent> tutorialTrigger, [JoinAll] ICollection<TutorialNode> tutorials,
            [JoinAll] ICollection<NotificationNode> notifications, [JoinAll] SingleNode<Dialogs60Component> dialogs60, [JoinAll] Optional<TurnOffTitorialUserNode> reservedUser,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            CheckForActiveNotificationsEvent checkForActiveNotificationsEvent = new();
            ScheduleEvent(checkForActiveNotificationsEvent, tutorialTrigger);

            if (reservedUser.IsPresent() || selfUser.Entity.HasComponent<SkipAllTutorialsComponent>() || checkForActiveNotificationsEvent.NotificationExist) {
                return;
            }

            Log.InfoFormat("TryShowTutorial {0}", tutorialTrigger.component.StepId);

            if (!ShowTutorial(tutorialTrigger, tutorials)) {
                return;
            }

            tutorialTrigger.component.DestroyTrigger();

            foreach (NotificationNode notification in notifications) {
                ScheduleEvent<CloseNotificationEvent>(notification);
            }

            dialogs60.component.CloseAll(tutorialTrigger.component.ignorableDialogName);
        }

        [OnEventFire]
        public void ShowTutorialOnReleaseClose(NodeRemoveEvent e, SingleNode<ReleaseGiftsNotificationComponent> eula, [JoinAll] SingleNode<MainScreenComponent> screen) {
            NewEvent<TriggerOnReleaseCloseEvent>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowTutorialOnReleaseClose(TriggerOnReleaseCloseEvent e, Node any, [JoinAll] [Combine] SingleNode<TutorialShowTriggerComponent> tutorialTrigger) {
            if (tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.ClickAnyWhere &&
                tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.CustomTrigger) {
                tutorialTrigger.component.Triggered();
            }
        }

        [OnEventFire]
        public void CloseEmailNotification(SetNotificationVisibleEvent e, EmailConfirmationNotificationNode emailConfirmationNotification) {
            if (activeTutorial != null) {
                ScheduleEvent<CloseNotificationEvent>(emailConfirmationNotification);
            }
        }

        [OnEventFire]
        public void StepComplete(TutorialStepCompleteEvent e, TutorialStepNode stepNode, [JoinByTutorial] ICollection<TutorialStepNode> steps,
            [JoinAll] ICollection<SingleNode<TutorialShowTriggerComponent>> triggers, [JoinAll] ICollection<TutorialNode> tutorials,
            [JoinAll] SingleNode<ClientSessionComponent> session) {
            if (!stepNode.Entity.HasComponent<TutorialStepCompleteComponent>()) {
                stepNode.Entity.AddComponent<TutorialStepCompleteComponent>();
            }

            TutorialNode tutorialNode = Select<TutorialNode>(stepNode.Entity, typeof(TutorialGroupComponent)).Single();
            ScheduleEvent(new TutorialActionEvent(tutorialNode.tutorialData.TutorialId, stepNode.tutorialStepData.StepId, TutorialAction.END), session);
            bool flag = true;

            if (!tutorialNode.Entity.HasComponent<TutorialCompleteComponent>()) {
                foreach (TutorialStepNode step in steps) {
                    flag = step.Entity.HasComponent<TutorialStepCompleteComponent>();

                    if (!flag) {
                        break;
                    }
                }
            }

            if (flag) {
                activeTutorial = null;

                if (!tutorialNode.Entity.HasComponent<TutorialCompleteComponent>()) {
                    tutorialNode.Entity.AddComponent<TutorialCompleteComponent>();
                }

                Log.Info("Tutorial complete, save on server");
                ScheduleEvent(new ApplyTutorialIdEvent(tutorialNode.tutorialData.TutorialId), session);
            }

            foreach (SingleNode<TutorialShowTriggerComponent> trigger in triggers) {
                if ((trigger.component.triggerType == TutorialShowTriggerComponent.EventTriggerType.Awake ||
                     trigger.component.triggerType == TutorialShowTriggerComponent.EventTriggerType.ClickAnyWhereOrDelay) && ShowTutorial(trigger, tutorials)) {
                    break;
                }
            }
        }

        public bool ShowTutorial(SingleNode<TutorialShowTriggerComponent> tutorialTrigger, ICollection<TutorialNode> tutorials) {
            Log.InfoFormat("Try Show Tutorial {0}", tutorialTrigger.component.StepId);

            if (!tutorialTrigger.component.ShowAllow()) {
                return false;
            }

            TutorialNode tutorialById = GetTutorialById(tutorialTrigger.component.TutorialId, tutorials);

            if (tutorialById == null) {
                return false;
            }

            if (!RequiredTutorialsShowed(tutorialById, tutorials)) {
                return false;
            }

            if (tutorialById.Entity.HasComponent<TutorialCompleteComponent>()) {
                Log.InfoFormat("Tutorial already complete {0}", tutorialById.tutorialData.TutorialId);
                return false;
            }

            if (activeTutorial != null && !tutorialById.Entity.Equals(activeTutorial.Entity)) {
                Log.Info("already have active tutorial");
                return false;
            }

            IList<TutorialStepNode> list = Select<TutorialStepNode>(tutorialById.Entity, typeof(TutorialGroupComponent));
            bool flag = true;

            for (int i = 0; i < list.Count; i++) {
                bool flag2 = list[i].Entity.HasComponent<TutorialStepCompleteComponent>();

                if (flag && !flag2 && list[i].tutorialStepData.StepId == tutorialTrigger.component.StepId) {
                    if (activeTutorial == null) {
                        activeTutorial = tutorialById;
                        Log.InfoFormat("Active tutorial: {0}", tutorialById.tutorialData.TutorialId);
                    }

                    Log.InfoFormat("Show step: {0} id: {1}", tutorialTrigger.component.name, tutorialTrigger.component.StepId);
                    ScheduleEvent<ShowTutorialStepEvent>(list[i].Entity);
                    TutorialStepIndexEvent tutorialStepIndexEvent = new();
                    ScheduleEvent(tutorialStepIndexEvent, list[i].Entity);
                    tutorialTrigger.component.Show(list[i].Entity, tutorialStepIndexEvent.CurrentStepNumber, tutorialStepIndexEvent.StepCountInTutorial);
                    return true;
                }

                flag = flag2;
            }

            return false;
        }

        [OnEventFire]
        public void LogTutorialStep(ShowTutorialStepEvent e, TutorialStepNode step, [JoinByTutorial] TutorialNode tutorial, [JoinAll] SingleNode<ClientSessionComponent> session) {
            ScheduleEvent(new TutorialActionEvent(tutorial.tutorialData.TutorialId, step.tutorialStepData.StepId, TutorialAction.START), session);
        }

        bool RequiredTutorialsShowed(TutorialNode tutorial, ICollection<TutorialNode> tutorials) {
            List<long> tutorialsIds = tutorial.tutorialRequiredCompletedTutorials.TutorialsIds;

            if (tutorialsIds == null) {
                return true;
            }

            foreach (long item in tutorialsIds) {
                TutorialNode tutorialById = GetTutorialById(item, tutorials);

                if (tutorialById != null && !tutorialById.Entity.HasComponent<TutorialCompleteComponent>()) {
                    return false;
                }
            }

            return true;
        }

        TutorialNode GetTutorialById(long id, ICollection<TutorialNode> tutorials) {
            foreach (TutorialNode tutorial in tutorials) {
                if (tutorial.tutorialData.TutorialId == id) {
                    return tutorial;
                }
            }

            return null;
        }

        TutorialStepNode GetStepById(long id, ICollection<TutorialStepNode> steps) {
            foreach (TutorialStepNode step in steps) {
                if (step.tutorialStepData.StepId == id) {
                    return step;
                }
            }

            return null;
        }

        [OnEventFire]
        public void CheckTutorial(CheckForTutorialEvent e, Node any) {
            e.TutorialIsActive = activeTutorial != null;
        }

        [OnEventFire]
        public void SkipAllTutorials(SkipAllTutorialsEvent e, Node any, [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            selfUser.Entity.AddComponent<SkipAllTutorialsComponent>();
        }

        [OnEventFire]
        public void AddItemHandler(NodeAddedEvent e, SingleNode<AddItemStepHandler> stepHandler, [JoinAll] SingleNode<ClientSessionComponent> session) {
            ApplyTutorialIdEvent eventInstance = new(stepHandler.component.stepId);
            ScheduleEvent(eventInstance, session);
        }

        [OnEventFire]
        public void AddItemHandler(NodeAddedEvent e, SingleNode<ResetFreeEnergyStepHandler> stepHandler, [JoinAll] SingleNode<ClientSessionComponent> session) {
            ApplyTutorialIdEvent eventInstance = new(stepHandler.component.stepId);
            ScheduleEvent(eventInstance, session);
        }

        [OnEventFire]
        public void AddItemResult(TutorialIdResultEvent e, Node any, [JoinAll] ICollection<SingleNode<AddItemStepHandler>> stepHandlers, [JoinAll] ICollection<TutorialStepNode> steps,
            [JoinAll] SingleNode<ClientSessionComponent> session) {
            foreach (SingleNode<AddItemStepHandler> stepHandler in stepHandlers) {
                if (stepHandler.component.stepId == e.Id) {
                    TutorialStepNode stepById = GetStepById(stepHandler.component.stepId, steps);
                    AddItemResultHandler(e, stepHandler.component, stepById, session);
                }
            }
        }

        [OnEventFire]
        public void AddItemResult(TutorialIdResultEvent e, Node any, [JoinAll] SingleNode<ResetFreeEnergyStepHandler> stepHandler, [JoinAll] ICollection<TutorialStepNode> steps,
            [JoinAll] SingleNode<ClientSessionComponent> session) {
            if (stepHandler.component.stepId == e.Id) {
                TutorialStepNode stepById = GetStepById(stepHandler.component.stepId, steps);
                AddItemResultHandler(e, stepHandler.component, stepById, session);
            }
        }

        void AddItemResultHandler(TutorialIdResultEvent e, AddItemStepHandler stepHandler, TutorialStepNode step, SingleNode<ClientSessionComponent> session) {
            if (e.ActionExecuted) {
                if (step != null) {
                    IList<TutorialNode> list = Select<TutorialNode>(step.Entity, typeof(TutorialGroupComponent));

                    if (list.Count > 0) {
                        TutorialNode tutorialNode = list.Single();
                        ScheduleEvent(new ApplyTutorialIdEvent(tutorialNode.tutorialData.TutorialId), session);
                    }
                }

                stepHandler.Success(e.Id);
            } else {
                stepHandler.Fail(e.Id);
            }
        }

        [OnEventFire]
        public void OpenTutorialContainerDialog(OpenTutorialContainerDialogEvent e, Node any, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            TutorialContainerDialog tutorialContainerDialog2 = e.dialog = dialogs.component.Get<TutorialContainerDialog>();
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(e.ItemId);

            if (item != null) {
                Entity marketItem = item.MarketItem;

                if (marketItem != null && marketItem.HasComponent<ImageItemComponent>()) {
                    tutorialContainerDialog2.ConatinerImageUID = marketItem.GetComponent<ImageItemComponent>().SpriteUid;
                }
            }

            tutorialContainerDialog2.ChestCount = e.ItemsCount;
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            tutorialContainerDialog2.Show(animators);
        }

        [OnEventFire]
        public void OpenTutorialContainer(OpenTutorialContainerEvent e, Node any) {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(e.ItemId);

            if (item != null) {
                IList<GamePlayChestItemNode> list = Select<GamePlayChestItemNode>(item.MarketItem, typeof(MarketItemGroupComponent));

                if (list.Count > 0) {
                    ScheduleEvent(new OpenContainerEvent(), list.Single().Entity);
                }
            }
        }

        [OnEventFire]
        public void GetChangeTurretTutorialValidationData(GetChangeTurretTutorialValidationDataEvent e, Node any, [JoinAll] MountedWeaponNode mountedWeapon,
            [JoinByUser] SingleNode<SelfUserComponent> selfUser, [JoinByUser] ICollection<UserWeaponNode> userWeapons) {
            e.MountedWeaponId = mountedWeapon.marketItemGroup.Key;
            e.TutorialItemAlreadyMounted = mountedWeapon.marketItemGroup.Key == e.ItemId;
            e.TutorialItemAlreadyBought = userWeapons.Any(userWeapon => userWeapon.marketItemGroup.Key == e.ItemId);
        }

        [OnEventFire]
        public void CheckBoughtWepon(CheckBoughtItemEvent e, Node any, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinByUser] ICollection<UserWeaponNode> userWeapons) {
            e.TutorialItemAlreadyBought = userWeapons.Any(userWeapon => userWeapon.marketItemGroup.Key == e.ItemId);
        }

        [OnEventFire]
        public void CheckMountedModule(CheckMountedModuleEvent e, Node any, [JoinAll] ICollection<SlotNode> slots) {
            foreach (SlotNode slot in slots) {
                if (slot.slotUserItemInfo.Slot == e.MountSlot) {
                    IList<UserModuleNode> list = Select<UserModuleNode>(slot.Entity, typeof(ModuleGroupComponent));

                    if (list.Count > 0) {
                        UserModuleNode userModuleNode = list[0];

                        if (userModuleNode.marketItemGroup.Key == e.ItemId) {
                            e.ModuleMounted = true;
                            return;
                        }
                    }

                    break;
                }
            }

            e.ModuleMounted = false;
        }

        [OnEventFire]
        public void ModuleResearchAvailable(CheckModuleForResearchEvent e, Node any, [JoinAll] ICollection<MarketModuleNode> modules) {
            foreach (MarketModuleNode module in modules) {
                if (module.marketItemGroup.Key != e.ItemId) {
                    continue;
                }

                IList<UserModuleNode> list = Select<UserModuleNode>(module.Entity, typeof(MarketItemGroupComponent));

                if (list.Count > 0) {
                    break;
                }

                IList<ModuleCardNode> list2 = Select<ModuleCardNode>(module.Entity, typeof(ParentGroupComponent));

                if (list2.Count > 0) {
                    ModulePrice craftPrice = module.moduleCardsComposition.CraftPrice;

                    if (list2[0].userItemCounter.Count >= craftPrice.Cards) {
                        e.ResearchAvailable = true;
                    }
                }

                break;
            }
        }

        [OnEventFire]
        public void ModuleUpgradeAvailable(CheckModuleForUpgradeEvent e, Node any, [JoinAll] ICollection<UserModuleNode> modules,
            [JoinAll] SingleNode<NewModulesScreenUIComponent> screen) {
            foreach (UserModuleNode module in modules) {
                if (module.marketItemGroup.Key != e.ItemId) {
                    continue;
                }

                long level = module.moduleUpgradeLevel.Level;

                if (level != 0) {
                    break;
                }

                IList<ModuleCardNode> list = Select<ModuleCardNode>(module.Entity, typeof(ParentGroupComponent));

                if (list.Count > 0) {
                    ModulePrice modulePrice = module.moduleCardsComposition.UpgradePrices[0];

                    if (list[0].userItemCounter.Count >= modulePrice.Cards && screen.component.tankPartModeController.GetMode() == TankPartModuleType.WEAPON) {
                        e.UpgradeAvailable = true;
                    }
                }

                break;
            }
        }

        [OnEventFire]
        public void SkipActiveTutorial(CompleteActiveTutorialEvent e, Node any) {
            if (activeTutorial != null) {
                ScheduleEvent<TutorialCompleteEvent>(activeTutorial.Entity);
            }
        }

        [OnEventFire]
        public void CompleteTutorialOnEsc(CompleteTutorialByEscEvent e, TutorialStepNode step, [JoinByTutorial] TutorialNode tutorial) {
            if (!tutorial.Entity.HasComponent<TutorialCompleteComponent>()) {
                tutorial.Entity.AddComponent<TutorialCompleteComponent>();
            }
        }

        public class TutorialNode : Node {
            public TutorialDataComponent tutorialData;

            public TutorialGroupComponent tutorialGroup;

            public TutorialRequiredCompletedTutorialsComponent tutorialRequiredCompletedTutorials;
        }

        public class ActiveTutorialNode : TutorialNode {
            public ActiveTutorialComponent activeTutorial;
        }

        public class TutorialStepNode : Node {
            public TutorialGroupComponent tutorialGroup;
            public TutorialStepDataComponent tutorialStepData;
        }

        public class UserWeaponNode : Node {
            public MarketItemGroupComponent marketItemGroup;
            public UserItemComponent userItem;

            public WeaponItemComponent weaponItem;
        }

        public class MountedWeaponNode : UserWeaponNode {
            public MountedItemComponent mountedItem;
        }

        public class SlotNode : Node {
            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;
        }

        public class MarketModuleNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public ModuleCardsCompositionComponent moduleCardsComposition;
            public ModuleItemComponent moduleItem;
        }

        public class UserModuleNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public ModuleCardsCompositionComponent moduleCardsComposition;

            public ModuleGroupComponent moduleGroup;
            public ModuleItemComponent moduleItem;

            public ModuleUpgradeLevelComponent moduleUpgradeLevel;

            public UserItemComponent userItem;
        }

        public class ModuleCardNode : Node {
            public ModuleCardItemComponent moduleCardItem;

            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }

        public class GamePlayChestItemNode : Node {
            public ContainerMarkerComponent containerMarker;
            public GameplayChestItemComponent gameplayChestItem;

            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }

        public class EmailConfirmationNotificationNode : Node {
            public ActiveNotificationComponent activeNotification;

            public EmailConfirmationNotificationComponent emailConfirmationNotification;

            public NotifficationMappingComponent notifficationMapping;
            public NotificationComponent notification;

            public NotificationConfigComponent notificationConfig;

            public NotificationMessageComponent notificationMessage;
        }

        public class ItemWithImageNode : Node {
            public ItemIconComponent itemIcon;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class TurnOffTitorialUserNode : Node {
            public SelfUserComponent selfUser;

            public TurnOffTutorialUserComponent turnOffTutorialUser;
        }

        [Not(typeof(NewCardItemNotificationComponent))]
        public class NotificationNode : Node {
            public ActiveNotificationComponent activeNotification;
        }

        public class TriggerOnReleaseCloseEvent : Event { }

        public class CheckForActiveNotificationsEvent : Event {
            public bool NotificationExist;
        }
    }
}