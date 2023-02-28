using UnityEngine;

public class GearRotateComponent : MonoBehaviour {
    public float angle = 1f;

    void Update() {
        transform.Rotate(Vector3.back, angle);
    }
}