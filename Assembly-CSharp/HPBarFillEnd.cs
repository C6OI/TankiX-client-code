using UnityEngine;

public class HPBarFillEnd : BarFillEnd {
    [SerializeField] AnimationCurve topCurve;

    [SerializeField] AnimationCurve bottomCurve;

    public override float FillAmount {
        set {
            base.FillAmount = value;
            image.offsetMax = new Vector2(image.offsetMax.x, 0f - topCurve.Evaluate(value));
            image.offsetMin = new Vector2(image.offsetMin.x, bottomCurve.Evaluate(value));
        }
    }
}