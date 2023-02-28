using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class LoginRewardItemTooltip : MonoBehaviour {
    [SerializeField] RectTransform text;

    [SerializeField] RectTransform back;

    public string Text {
        get => text.GetComponent<TextMeshProUGUI>().text;
        set => text.GetComponent<TextMeshProUGUI>().text = value;
    }

    void Update() {
        back.sizeDelta = new Vector2(270f, text.sizeDelta.y + 30f);
    }
}