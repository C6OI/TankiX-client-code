using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleResultBestPlayerScreenAnimationComponent : MonoBehaviour {
        [SerializeField] Animator mainContentAnimator;

        [SerializeField] Animator avatarAnimator;

        [SerializeField] Animator infoAnimator;

        [SerializeField] Animator info1Animator;

        [SerializeField] Animator buttonsAnimator;

        List<Action> actions;

        bool playActions;

        float showDelay = 0.2f;

        float timer;

        [Inject] public static EngineServiceInternal EngineService { get; set; }

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
            actions = new List<Action> { ShowAvatar, ShowInfo, ShowInfo1, ShowTank, ShowButtons, ShowModules };
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

        public void ShowAvatar() {
            avatarAnimator.SetBool("on", true);
        }

        public void ShowInfo() {
            infoAnimator.SetBool("on", true);
        }

        public void ShowInfo1() {
            info1Animator.SetBool("on", true);
        }

        public void ShowTank() {
            EngineService.Engine.ScheduleEvent<BuildBestPlayerTankEvent>(EngineService.EntityStub);
            mainContentAnimator.SetBool("showTank", true);
        }

        public void ShowModules() {
            GetComponentInChildren<MVPModulesInfoComponent>().AnimateCards();
        }

        public void ShowButtons() {
            buttonsAnimator.SetBool("on", true);
        }

        public void DisableAll() {
            mainContentAnimator.SetBool("showTank", false);
            avatarAnimator.SetBool("on", false);
            infoAnimator.SetBool("on", false);
            info1Animator.SetBool("on", false);
            buttonsAnimator.SetBool("on", false);
            mainContentAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            avatarAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            infoAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            info1Animator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            buttonsAnimator.GetComponentInChildren<CanvasGroup>().alpha = 0f;
        }
    }
}