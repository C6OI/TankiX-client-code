using System;
using System.Collections.Generic;
using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialBuildSystem : ECSSystem {
        [OnEventFire]
        public void AddTutorials(NodeAddedEvent e, SingleNode<TutorialConfigurationComponent> tutorialData, SingleNode<SelfUserComponent> selfUser) {
            if (Environment.CommandLine.Contains("disableTutorials") || tutorialData.component.Tutorials == null || selfUser.Entity.HasComponent<SkipAllTutorialsComponent>()) {
                return;
            }

            foreach (string tutorial in tutorialData.component.Tutorials) {
                CreateEntity<TutorialDataTemplate>(tutorial);
            }
        }

        [OnEventFire]
        public void CompleteTutorial(NodeAddedEvent e, TankRentOfferNode rentOffer,
            [JoinBy(typeof(SpecialOfferGroupComponent))]
            SingleNode<PersonalSpecialOfferPropertiesComponent> personalOffer, [Context] [Combine] TutorialNode tutorial) {
            if (!tutorial.Entity.HasComponent<TutorialCompleteComponent>()) {
                tutorial.Entity.AddComponent<TutorialCompleteComponent>();
            }
        }

        [OnEventFire]
        public void TutorialAdded(NodeAddedEvent e, TutorialCompleteIdsNode userSteps, [Combine] TutorialNode tutorial) {
            bool flag = userSteps.registrationDate.RegistrationDate.UnityTime != 0f;
            bool flag2 = TutorialCompleted(tutorial.tutorialData.TutorialId, userSteps.tutorialCompleteIds.CompletedIds);
            bool flag3 = flag && tutorial.tutorialData.ForNewPlayer || !flag && tutorial.tutorialData.ForOldPlayer;

            if (flag2 || !flag3) {
                if (!tutorial.Entity.HasComponent<TutorialCompleteComponent>()) {
                    tutorial.Entity.AddComponent<TutorialCompleteComponent>();
                }

                if (!flag3) {
                    return;
                }
            }

            foreach (string key in tutorial.tutorialData.Steps.Keys) {
                Entity entity = CreateEntity(tutorial.tutorialData.Steps[key], tutorial.tutorialData.StepsPath + key);

                if (entity.GetComponent<TutorialStepDataComponent>().VisualStep) {
                    entity.AddComponent<TutorialVisualStepComponent>();
                }

                if (flag2) {
                    entity.AddComponent<TutorialStepCompleteComponent>();
                }

                tutorial.tutorialGroup.Attach(entity);
            }
        }

        public bool TutorialCompleted(long tutorialId, List<long> ids) {
            foreach (long id in ids) {
                if (tutorialId == id) {
                    return true;
                }
            }

            return false;
        }

        [OnEventFire]
        public void GetTutorialStepIndex(TutorialStepIndexEvent e, VisualTutorialStep tutorialStep, [JoinAll] ICollection<VisualTutorialStep> allSteps,
            [JoinAll] ICollection<CompletedVisualTutorialStep> allCompletedSteps) {
            e.CurrentStepNumber = allCompletedSteps.Count + 1;
            e.StepCountInTutorial = allSteps.Count;
        }

        [OnEventFire]
        public void CheckForSkipTutorials(CheckForSkipTutorial e, Node any, [JoinAll] SelfUser selfUSer) {
            e.canSkipTutorial = false;
        }

        public class TutorialNode : Node {
            public TutorialDataComponent tutorialData;

            public TutorialGroupComponent tutorialGroup;
        }

        public class TutorialCompletedNode : TutorialNode {
            public TutorialCompleteComponent tutorialComplete;
        }

        public class TutorialStep : Node {
            public TutorialGroupComponent tutorialGroup;
            public TutorialStepDataComponent tutorialStepData;
        }

        public class VisualTutorialStep : TutorialStep {
            public TutorialVisualStepComponent tutorialVisualStep;
        }

        public class CompletedVisualTutorialStep : TutorialStep {
            public TutorialStepCompleteComponent tutorialStepComplete;
            public TutorialVisualStepComponent tutorialVisualStep;
        }

        public class TutorialCompleteIdsNode : Node {
            public RegistrationDateComponent registrationDate;
            public SelfUserComponent selfUser;

            public TutorialCompleteIdsComponent tutorialCompleteIds;
        }

        public class UserSkipALlTutorialsNode : Node {
            public SelfUserComponent selfUser;

            public SkipAllTutorialsComponent skipAllTutorials;
        }

        public class SelfUser : Node {
            public SelfUserComponent selfUser;

            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class TankRentOfferNode : Node {
            public GoodsPriceComponent goodsPrice;

            public LegendaryTankSpecialOfferComponent legendaryTankSpecialOffer;
            public SpecialOfferComponent specialOffer;

            public SpecialOfferGroupComponent specialOfferGroup;
        }
    }
}