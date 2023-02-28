using UnityEngine;

public class ME_LightCurves : MonoBehaviour {
    public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public float GraphTimeMultiplier = 1f;

    public float GraphIntensityMultiplier = 1f;

    public bool IsLoop;

    bool canUpdate;

    Light lightSource;

    float startTime;

    void Awake() {
        lightSource = GetComponent<Light>();
        lightSource.intensity = LightCurve.Evaluate(0f);
    }

    void Update() {
        float num = Time.time - startTime;

        if (canUpdate) {
            float intensity = LightCurve.Evaluate(num / GraphTimeMultiplier) * GraphIntensityMultiplier;
            lightSource.intensity = intensity;
        }

        if (num >= GraphTimeMultiplier) {
            if (IsLoop) {
                startTime = Time.time;
            } else {
                canUpdate = false;
            }
        }
    }

    void OnEnable() {
        startTime = Time.time;
        canUpdate = true;
    }
}