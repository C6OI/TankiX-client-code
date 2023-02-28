using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EnableCameraDepthInForward : MonoBehaviour {
    void Start() {
        Set();
    }

    void Set() {
        if (GetComponent<Camera>().depthTextureMode == DepthTextureMode.None) {
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        }
    }
}