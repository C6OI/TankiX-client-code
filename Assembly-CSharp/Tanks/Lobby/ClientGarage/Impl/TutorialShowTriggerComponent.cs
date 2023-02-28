using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialShowTriggerComponent : BehaviourComponent {
        public enum EventTriggerType {
            Awake = 0,
            ClickAnyWhere = 1,
            ClickAnyWhereOrDelay = 2,
            CustomTrigger = 3
        }

        [SerializeField] protected long tutorialId;

        [SerializeField] protected long stepId;

        public EventTriggerType triggerType;

        public TutorialType tutorialType;

        public GameObject[] highlightedRects;

        [HideInInspector] public RectTransform popupPositionRect;

        [HideInInspector] public bool useOverlay = true;

        [HideInInspector] public bool showArrow;

        [HideInInspector] public RectTransform arrowPositionRect;

        [HideInInspector] public Button selectable;

        [HideInInspector] public float cameraOffset;

        [HideInInspector] public float showDelay;

        [HideInInspector] public float triggerDelay;

        [HideInInspector] public Button triggerButton;

        [HideInInspector] public TutorialHideTriggerComponent tutorialHideTrigger;

        [HideInInspector] public TutorialStepHandler stepCustomHandler;

        [HideInInspector] public GameObject outlinePrefab;

        [HideInInspector] public bool useImageInPopup;

        [HideInInspector] public string imageUid;

        [HideInInspector] public bool inBattleMode;

        public string ignorableDialogName;

        bool isShow;

        public long TutorialId => tutorialId;

        public long StepId => stepId;

        void Update() {
            if ((triggerType == EventTriggerType.ClickAnyWhere || triggerType == EventTriggerType.ClickAnyWhereOrDelay) && Input.GetMouseButtonDown(0)) {
                CancelInvoke();
                triggerDelay = 0f;
                Triggered();
            }
        }

        public void Show(Entity tutorialStep, int currentStepNumber, int stepCountInTutorial) {
            if (!isShow) {
                isShow = true;
                string message = tutorialStep.GetComponent<TutorialStepDataComponent>().Message;

                TutorialHighlightTankStepDataComponent tutorialHighlightTankStepDataComponent = !tutorialStep.HasComponent<TutorialHighlightTankStepDataComponent>()
                                                                                                    ? new TutorialHighlightTankStepDataComponent()
                                                                                                    : tutorialStep.GetComponent<TutorialHighlightTankStepDataComponent>();

                TutorialData tutorialData = new();
                tutorialData.Type = tutorialType;
                tutorialData.TutorialStep = tutorialStep;
                tutorialData.Message = message;
                tutorialData.PopupPositionRect = popupPositionRect;
                tutorialData.ShowDelay = showDelay;
                tutorialData.ImageUid = imageUid;
                tutorialData.InteractableButton = selectable;
                tutorialData.OutlinePrefab = outlinePrefab;
                tutorialData.CameraOffset = cameraOffset;
                tutorialData.HighlightHull = tutorialHighlightTankStepDataComponent.HighlightHull;
                tutorialData.HighlightWeapon = tutorialHighlightTankStepDataComponent.HighlightWeapon;
                tutorialData.CurrentStepNumber = currentStepNumber;
                tutorialData.StepCountInTutorial = stepCountInTutorial;
                tutorialData.InBattleMode = inBattleMode;
                TutorialData tutorialData2 = tutorialData;

                switch (tutorialType) {
                    case TutorialType.Default:
                        tutorialData2.ContinueOnClick = true;
                        ShowMaskedPopupStep(tutorialData2);
                        break;

                    case TutorialType.Interact:
                        ShowInteractStep(tutorialData2);
                        break;

                    case TutorialType.HighlightTankPart:
                        tutorialData2.ContinueOnClick = true;
                        ShowHighlightTankStep(tutorialData2);
                        break;

                    case TutorialType.CustomHandler:
                        stepCustomHandler.RunStep(tutorialData2);
                        break;
                }

                if (tutorialHideTrigger != null) {
                    tutorialHideTrigger.Activate(tutorialStep);
                }
            }
        }

        void ShowMaskedPopupStep(TutorialData data) {
            TutorialCanvas.Instance.Show(data, useOverlay, highlightedRects, arrowPositionRect);
        }

        void ShowInteractStep(TutorialData data) {
            TutorialCanvas.Instance.Show(data, useOverlay, highlightedRects, arrowPositionRect);
        }

        void ShowHighlightTankStep(TutorialData data) {
            TutorialCanvas.Instance.Show(data, useOverlay, highlightedRects);
        }

        public void Triggered() {
            if (ShowAllow() && !isShow) {
                if (triggerDelay == 0f) {
                    DelayedTrigger();
                } else {
                    Invoke("DelayedTrigger", triggerDelay);
                }
            }
        }

        void DelayedTrigger() {
            if (!isShow && GetComponent<EntityBehaviour>().Entity != null) {
                ScheduleEvent<TutorialTriggerEvent>(GetComponent<EntityBehaviour>().Entity);
            }
        }

        public void DestroyTrigger() {
            gameObject.SetActive(false);
        }

        public void SetSeleectable(Button selectable) {
            this.selectable = selectable;
        }

        public bool ShowAllow() {
            ITutorialShowStepValidator component = GetComponent<ITutorialShowStepValidator>();

            if (component != null) {
                return component.ShowAllowed(stepId);
            }

            return true;
        }
    }
}