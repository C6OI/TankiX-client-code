using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl.Tutorial;
using Tanks.Lobby.ClientNavigation.API;
using tanks.modules.lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialSkipSystem : ECSSystem {
        [OnEventComplete]
        public void ShowButton(NodeAddedEvent e, SingleNode<TutorialScreenComponent> active, [JoinAll] SingleNode<MainScreenComponent> mainScreen,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            TutorialCanvas instance = TutorialCanvas.Instance;
            GameObject skipTutorialButton = instance.SkipTutorialButton;
            skipTutorialButton.SetActive(true);
            SkipTutorialConfirmWindowComponent componentInChildren = instance.GetComponentInChildren<SkipTutorialConfirmWindowComponent>(true);
            Selectable[] componentsInChildren = componentInChildren.GetComponentsInChildren<Selectable>();

            foreach (Selectable selectable in componentsInChildren) {
                instance.AddAllowSelectable(selectable);
                selectable.interactable = true;
            }
        }

        [OnEventFire]
        public void SkipTutorialByButton(ButtonClickEvent e, SingleNode<SkipTutorialButtonComponent> SkipTutorial, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            CompleteActiveTutorial(SkipTutorial.Entity);
        }

        [OnEventFire]
        public void SkipTutorialByEsc(UpdateEvent e, SingleNode<SkipTutorialButtonComponent> SkipTutorial) {
            if (InputMapping.Cancel) {
                CompleteActiveTutorial(SkipTutorial.Entity);
            }
        }

        void ShowConfirmationDialog([JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            TutorialCanvas instance = TutorialCanvas.Instance;
            SkipTutorialConfirmWindowComponent componentInChildren = instance.GetComponentInChildren<SkipTutorialConfirmWindowComponent>(true);
            componentInChildren.Show(animators);
        }

        [OnEventFire]
        public void ConfirmOnDialog(DialogConfirmEvent e, SingleNode<SkipTutorialConfirmWindowComponent> skipDialog, [JoinAll] SelfUserNode selfUser,
            [JoinAll] ICollection<TutorialStepNode> tutorials) {
            CompleteActiveTutorial(skipDialog.Entity);
        }

        void CompleteActiveTutorial(Entity any) {
            ScheduleEvent<CompleteActiveTutorialEvent>(any);
        }

        [OnEventFire]
        public void CompleteResearchModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<SelectModuleForResearchTutorStepHandler> stepHandler) {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void CompleteResearchModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<ModulesTutorialStep4Handler> stepHandler) {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void CompleteResearchModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<EquipModulesTutorStepHandler> stepHandler) {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void CompleteResearchModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<ModulesTutorStep7Handler> stepHandler) {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void CompleteResearchModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<ModulesTutorStep8Handler> stepHandler) {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void CompleteUpgradeModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<UpgradeModuleTutorStep7Handler> stepHandler) {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void TutorialComplete(TutorialCompleteEvent e, TutorialNode tutorial, [JoinByTutorial] ICollection<TutorialStepNode> steps,
            [JoinAll] SingleNode<ClientSessionComponent> session) {
            if (!tutorial.Entity.HasComponent<TutorialCompleteComponent>()) {
                tutorial.Entity.AddComponent<TutorialCompleteComponent>();
            }

            foreach (TutorialStepNode step in steps) {
                if (!step.Entity.HasComponent<TutorialStepCompleteComponent>()) {
                    ScheduleEvent(new TutorialActionEvent(tutorial.tutorialData.TutorialId, step.tutorialStepData.StepId, TutorialAction.START), session);
                    ScheduleEvent(new TutorialActionEvent(tutorial.tutorialData.TutorialId, step.tutorialStepData.StepId, TutorialAction.END), session);
                    step.Entity.AddComponent<TutorialStepCompleteComponent>();
                }
            }

            TutorialCanvas.Instance.Hide();
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
        }

        [Not(typeof(TutorialStepCompleteComponent))]
        public class TutorialStepNode : Node {
            public TutorialStepDataComponent tutorialStepData;
        }

        [Not(typeof(TutorialCompleteComponent))]
        public class TutorialNode : Node {
            public TutorialDataComponent tutorialData;

            public TutorialGroupComponent tutorialGroup;

            public TutorialRequiredCompletedTutorialsComponent tutorialRequiredCompletedTutorials;
        }
    }
}