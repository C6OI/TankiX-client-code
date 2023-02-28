using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialCanvas : MonoBehaviour {
        public static TutorialCanvas Instance;

        [SerializeField] public GameObject overlay;

        [SerializeField] TutorialIntroDialog introDialog;

        [SerializeField] public TutorialDialog dialog;

        [SerializeField] public TutorialArrow arrow;

        [SerializeField] GameObject tutorialScreen;

        [SerializeField] GameObject skipTutorialButton;

        [SerializeField] CanvasGroup backgroundCanvasGroup;

        [SerializeField] Canvas overlayCanvas;

        [SerializeField] Camera outlineCamera;

        [SerializeField] Camera tutorialCamera;

        readonly List<GameObject> additionalMaskRects = new();

        readonly List<Selectable> allowSelectables = new();

        Material backgroundMaterial;

        readonly List<Selectable> disabledSelectables = new();

        readonly List<GameObject> maskedObjects = new();

        public GameObject SkipTutorialButton => skipTutorialButton;

        public bool IsShow { get; private set; }

        public bool IsPaused { get; private set; }

        public bool AllowCancelHandler { get; private set; } = true;

        public Camera OutlineCamera => outlineCamera;

        void Awake() {
            Image component = backgroundCanvasGroup.GetComponent<Image>();
            backgroundMaterial = new Material(component.material);
            component.material = backgroundMaterial;
        }

        void Start() {
            Instance = this;
        }

        void Update() {
            CheckForOverlayCamera();
            backgroundMaterial.SetColor("_TintColor", new Color(0.078f, 0.078f, 0.078f, backgroundCanvasGroup.alpha * 0.8f));
            backgroundMaterial.SetFloat("_Size", backgroundCanvasGroup.alpha * 6f);
        }

        void CheckForOverlayCamera() {
            if (overlayCanvas.worldCamera == null) {
                overlayCanvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
                overlayCanvas.planeDistance = 10f;
            }
        }

        public void ShowIntroDialog(TutorialData data, bool canSkipTutorial) {
            tutorialCamera.gameObject.SetActive(true);
            BlockInteractable();
            CheckForOverlayCamera();
            overlay.SetActive(true);
            introDialog.Show(data.TutorialStep, canSkipTutorial);
            GetComponent<Animator>().SetBool("show", true);
            overlayCanvas.GetComponent<Animator>().SetBool("show", true);
            IsShow = true;
            tutorialScreen.SetActive(true);
            MainScreenComponent.Instance.ToggleNews(false);
        }

        public void Show(TutorialData data, bool useOverlay, GameObject[] highlightedRects = null, RectTransform arrowPositionRect = null) {
            tutorialCamera.gameObject.SetActive(true);
            CheckForOverlayCamera();
            dialog.Init(data);
            AllowCancelHandler = false;
            overlay.SetActive(useOverlay);
            CreateMasks(highlightedRects);
            BlockInteractable();

            if (arrowPositionRect != null) {
                SetArrowPosition(arrowPositionRect);
            }

            Invoke("DelayedShow", data.ShowDelay);
            tutorialScreen.SetActive(true);
        }

        void DelayedShow() {
            MainScreenComponent.Instance.ToggleNews(false);
            BlockInteractable();
            CancelInvoke();
            GetComponent<Animator>().SetBool("show", true);
            overlayCanvas.GetComponent<Animator>().SetBool("show", true);
            TutorialDialog tutorialDialog = dialog;
            tutorialDialog.PopupContinue = (TutorialPopupContinue)Delegate.Combine(tutorialDialog.PopupContinue, new TutorialPopupContinue(Hide));
            dialog.Show();
            IsShow = true;
            tutorialScreen.SetActive(true);
        }

        void SetArrowPosition(RectTransform arrowPositionRect) {
            arrow.Setup(arrowPositionRect);
            arrow.gameObject.SetActive(true);
        }

        public void Hide() {
            CancelInvoke();

            if (GetComponent<Animator>().GetBool("show")) {
                GetComponent<Animator>().SetBool("show", false);
                overlayCanvas.GetComponent<Animator>().SetBool("show", false);
            } else {
                IsShow = true;
                Hidden();
            }
        }

        void Hidden() {
            if (IsShow) {
                AllowCancelHandler = true;
                IsShow = false;
                UnblockInteractable();
                ClearMasks();
                tutorialCamera.gameObject.SetActive(false);
                allowSelectables.Clear();
                arrow.gameObject.SetActive(false);
                dialog.gameObject.SetActive(false);
                tutorialScreen.SetActive(false);
                skipTutorialButton.SetActive(false);
                MainScreenComponent.Instance.ToggleNews(true);
                dialog.TutorialHidden();
                introDialog.TutorialHidden();
            }
        }

        public void BlockInteractable() {
            Selectable[] array = FindObjectsOfType<Selectable>();
            Selectable[] array2 = array;

            foreach (Selectable selectable in array2) {
                if (selectable.interactable && !allowSelectables.Contains(selectable) && selectable.gameObject != skipTutorialButton) {
                    disabledSelectables.Add(selectable);
                    selectable.interactable = false;
                }
            }
        }

        public void UnblockInteractable() {
            foreach (Selectable disabledSelectable in disabledSelectables) {
                if (disabledSelectable != null) {
                    disabledSelectable.interactable = true;
                }
            }

            disabledSelectables.Clear();
        }

        public void AddAllowSelectable(Selectable selectable) {
            if (!allowSelectables.Contains(selectable)) {
                allowSelectables.Add(selectable);
            }
        }

        void CreateMasks(GameObject[] rects) {
            ClearMasks();

            if (rects != null) {
                foreach (GameObject rect in rects) {
                    CreateMask(rect);
                }
            }

            foreach (GameObject additionalMaskRect in additionalMaskRects) {
                CreateMask(additionalMaskRect);
            }

            additionalMaskRects.Clear();
        }

        void CreateMask(GameObject rect) {
            Canvas component = rect.GetComponent<Canvas>();

            if (component == null) {
                component = rect.gameObject.AddComponent<Canvas>();
                component.overrideSorting = true;
                component.sortingLayerName = "Overlay";
                component.sortingOrder = 30;
                rect.gameObject.AddComponent<GraphicRaycaster>();
                maskedObjects.Add(rect.gameObject);
            }
        }

        void ClearMasks() {
            foreach (GameObject maskedObject in maskedObjects) {
                if (maskedObject != null) {
                    GraphicRaycaster component = maskedObject.GetComponent<GraphicRaycaster>();

                    if (component != null) {
                        Destroy(component);
                    }

                    Canvas component2 = maskedObject.GetComponent<Canvas>();

                    if (component2 != null) {
                        Destroy(component2);
                    }
                }
            }

            Canvas.ForceUpdateCanvases();
            maskedObjects.Clear();
        }

        public void ShowOverlay() {
            GetComponent<Animator>().SetBool("show", false);
        }

        public void AddAdditionalMaskRect(GameObject maskRect) {
            additionalMaskRects.Add(maskRect);
        }

        public void SetupActivePopup(TutorialData data) {
            dialog.OverrideData(data);
        }

        public void CardsNotificationsEnabled(bool value) {
            GetComponent<Animator>().SetBool("showOverlay", !value);
        }

        public void Pause() {
            Debug.Log("Pause");
            ToggleMask(false);
            IsPaused = true;
            GetComponent<Animator>().SetBool("pause", true);
            overlayCanvas.GetComponent<Animator>().SetBool("pause", true);
        }

        public void Continue() {
            if (IsPaused) {
                Debug.Log("Continue");
                ToggleMask(true);
                GetComponent<Animator>().SetBool("pause", false);
                overlayCanvas.GetComponent<Animator>().SetBool("pause", false);
            }
        }

        void ToggleMask(bool value) {
            foreach (GameObject maskedObject in maskedObjects) {
                if (maskedObject != null) {
                    Canvas component = maskedObject.GetComponent<Canvas>();

                    if (component != null) {
                        component.enabled = value;
                    }
                }
            }
        }
    }
}