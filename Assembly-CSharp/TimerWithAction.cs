using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerWithAction : MonoBehaviour {
    [Header("Time To Action")] [SerializeField]
    float _startTime;

    [Header("Action")] [SerializeField] Button.ButtonClickedEvent _onTimeEndAction;

    [Header("Description")] [SerializeField]
    LocalizedField _actionDescription;

    [SerializeField] TextMeshProUGUI _descriptionText;

    public float StartTime {
        get => _startTime;
        set => _startTime = value;
    }

    public Button.ButtonClickedEvent OnTimeEndAction {
        get => _onTimeEndAction;
        set => _onTimeEndAction = value;
    }

    public LocalizedField ActionDescription {
        get => _actionDescription;
        set => _actionDescription = value;
    }

    public TextMeshProUGUI DescriptionText {
        get => _descriptionText;
        set => _descriptionText = value;
    }

    public float CurrentTime { get; set; }

    void Update() {
        if (!(CurrentTime <= 0f)) {
            if (DescriptionText != null && !string.IsNullOrEmpty(ActionDescription.Value)) {
                DescriptionText.text = string.Format(ActionDescription, CurrentTime);
            }

            CurrentTime -= Time.deltaTime;

            if (!(CurrentTime > 0f)) {
                CurrentTime = 0f;
                OnTimeEndAction.Invoke();
            }
        }
    }

    void OnEnable() {
        if (CurrentTime <= StartTime) {
            CurrentTime = StartTime;
        }
    }
}