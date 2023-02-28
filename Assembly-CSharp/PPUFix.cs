using UnityEngine;

public class PPUFix : MonoBehaviour {
    Canvas canvas;

    float prevPPU;

    void Start() {
        canvas = GetComponent<Canvas>();
    }

    void Update() {
        float num = 100f / canvas.scaleFactor;

        if (!Mathf.Approximately(num, prevPPU)) {
            prevPPU = num;
            canvas.referencePixelsPerUnit = num;
        }
    }
}