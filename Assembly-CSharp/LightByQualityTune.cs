using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightByQualityTune : MonoBehaviour {
    public float lowQualityIntensity = 1f;

    public Color lowQualityColor = new(1f, 1f, 1f, 1f);

    public int lowQualityLevel = 1;

    public int disableQualityLevel = -1;

    void Start() {
        int qualityLevel = QualitySettings.GetQualityLevel();

        if (qualityLevel <= disableQualityLevel) {
            gameObject.SetActive(false);
        }

        if (qualityLevel <= lowQualityLevel) {
            Light component = GetComponent<Light>();
            component.intensity = lowQualityIntensity;
            component.color = lowQualityColor;
        }
    }
}