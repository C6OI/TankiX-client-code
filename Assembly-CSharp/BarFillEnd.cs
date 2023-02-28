using UnityEngine;

[ExecuteInEditMode]
public class BarFillEnd : MonoBehaviour {
    [SerializeField] protected RectTransform image;

    float fillAmount;

    public virtual float FillAmount {
        get => fillAmount;
        set {
            fillAmount = value;
            image.anchoredPosition = new Vector2(GetComponent<RectTransform>().rect.width * value, image.anchoredPosition.y);
            image.gameObject.SetActive(value != 0f && value != 1f);
        }
    }
}