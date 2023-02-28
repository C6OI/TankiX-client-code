using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialIntroDialog : MonoBehaviour {
    [SerializeField] protected AnimatedText animatedText;

    [SerializeField] Button yesButton;

    [SerializeField] Button noButton;

    [SerializeField] Button sarcasmButton;

    [SerializeField] Button startTutorial;

    [SerializeField] Button skipTutorial;

    [SerializeField] LocalizedField yesText;

    [SerializeField] LocalizedField introText;

    [SerializeField] LocalizedField introWithoutQuestionText;

    [SerializeField] LocalizedField confirmText;

    [SerializeField] LocalizedField tipText;

    [SerializeField] LocalizedField sarcasmText;

    bool canSkipTutorial;

    float showTimer;

    Entity tutorialStep;

    [Inject] public static EngineServiceInternal EngineService { get; set; }

    public void Show(Entity tutorialStep, bool canSkipTutorial) {
        this.tutorialStep = tutorialStep;
        this.canSkipTutorial = canSkipTutorial;
        animatedText.ResultText = !canSkipTutorial ? introWithoutQuestionText.Value : introText.Value;
        animatedText.Animate();
        showTimer = 0f;
        gameObject.SetActive(true);
        yesButton.gameObject.SetActive(true);
        yesButton.GetComponentInChildren<TextMeshProUGUI>().text = !canSkipTutorial ? "ok" : yesText.Value;
        noButton.gameObject.SetActive(canSkipTutorial);
        sarcasmButton.gameObject.SetActive(true);
        startTutorial.gameObject.SetActive(false);
        skipTutorial.gameObject.SetActive(false);
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(ContinueTutorial);
        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(ShowConfirmText);
        sarcasmButton.onClick.RemoveAllListeners();
        sarcasmButton.onClick.AddListener(ShowSarcasm);
        startTutorial.onClick.RemoveAllListeners();
        startTutorial.onClick.AddListener(ContinueTutorial);
        skipTutorial.onClick.RemoveAllListeners();
        skipTutorial.onClick.AddListener(DisableTutorials);
        yesButton.interactable = true;
        noButton.interactable = true;
        sarcasmButton.interactable = true;
        sarcasmButton.interactable = true;
        startTutorial.interactable = true;
        skipTutorial.interactable = true;
    }

    void ShowConfirmText() {
        if (canSkipTutorial) {
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            sarcasmButton.gameObject.SetActive(false);
            animatedText.ResultText = confirmText.Value + "\n\n<color=#A0A0A0>" + tipText.Value;
            animatedText.Animate();
            startTutorial.gameObject.SetActive(true);
            skipTutorial.gameObject.SetActive(true);
        }
    }

    void ShowSarcasm() {
        sarcasmButton.gameObject.SetActive(false);
        animatedText.ResultText = sarcasmText.Value;
        animatedText.Animate();
    }

    void DisableTutorials() {
        if (tutorialStep != null) {
            EngineService.Engine.ScheduleEvent<SkipAllTutorialsEvent>(tutorialStep);
            tutorialStep = null;
            TutorialCanvas.Instance.Hide();
        }
    }

    void ContinueTutorial() {
        TutorialCanvas.Instance.Hide();
    }

    public void TutorialHidden() {
        gameObject.SetActive(false);
        Entity entity = tutorialStep;
        tutorialStep = null;

        if (entity != null) {
            EngineService.Engine.ScheduleEvent<TutorialStepCompleteEvent>(entity);
        }
    }
}