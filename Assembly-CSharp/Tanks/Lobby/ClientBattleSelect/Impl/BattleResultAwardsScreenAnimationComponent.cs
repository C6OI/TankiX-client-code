using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleResultAwardsScreenAnimationComponent : BehaviourComponent {
        [SerializeField] Animator headerAnimator;

        [SerializeField] Animator infoAnimator;

        [SerializeField] Animator tankInfoAnimator;

        [SerializeField] Animator specialOfferAnimator;

        [SerializeField] CircleProgressBar rankProgressBar;

        [SerializeField] CircleProgressBar containerProgressBar;

        [SerializeField] CircleProgressBar hullProgressBar;

        [SerializeField] CircleProgressBar turretProgressBar;

        public bool playActions;

        List<Action> actions;

        float showDelay = 0.2f;

        float timer;

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        void Update() {
            if (playActions) {
                timer += Time.deltaTime;

                if (timer > showDelay) {
                    timer = 0f;
                    NextAction();
                }
            }
        }

        void OnEnable() {
            timer = 0f;
            showDelay = 0.2f;
            playActions = true;
            actions = new List<Action> { ShowHeader, ShowInfo, ShowTankInfo, ShowSpecialOffer };
            rankProgressBar.StopAnimation();
            containerProgressBar.StopAnimation();
            hullProgressBar.StopAnimation();
            turretProgressBar.StopAnimation();
        }

        void OnDisable() {
            DisableAll();
        }

        void NextAction() {
            if (actions.Count > 0) {
                Action action = actions[0];
                actions.Remove(action);
                playActions = actions.Count > 0;
                action();
            }
        }

        public void ShowHeader() {
            headerAnimator.SetBool("on", true);
        }

        public void ShowInfo() {
            playActions = false;
            infoAnimator.SetBool("on", true);
            showDelay = 0.5f;
            ShowBattleResultsScreenNotificationEvent showBattleResultsScreenNotificationEvent = new();
            showBattleResultsScreenNotificationEvent.Index = 1;
            EngineService.Engine.ScheduleEvent(showBattleResultsScreenNotificationEvent, EngineService.EntityStub);

            if (!showBattleResultsScreenNotificationEvent.NotificationExist) {
                playActions = true;
            }

            rankProgressBar.Animate(1f);
            containerProgressBar.Animate(1f);
        }

        public void ShowTankInfo() {
            EngineService.Engine.ScheduleEvent<BuildSelfPlayerTankEvent>(EngineService.EntityStub);
            tankInfoAnimator.SetBool("on", true);
            hullProgressBar.Animate(1f);
            turretProgressBar.Animate(1f);
        }

        public void ShowSpecialOffer() {
            specialOfferAnimator.SetBool("on", true);
            ShowButtons();
        }

        public void ShowButtons() {
            GetComponentInParent<BattleResultCommonUIComponent>().ShowBottomPanel();
            GetComponentInParent<BattleResultCommonUIComponent>().ShowTopPanel();
        }

        public void DisableAll() {
            playActions = false;
            headerAnimator.SetBool("on", false);
            infoAnimator.SetBool("on", false);
            tankInfoAnimator.SetBool("on", false);
            specialOfferAnimator.SetBool("on", false);
            headerAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            infoAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            tankInfoAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            specialOfferAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
        }
    }
}