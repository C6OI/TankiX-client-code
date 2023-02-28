using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

public class ColliderTest : MonoBehaviour {
    bool flag;
    Vector3 hitPoint;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo)) {
                ForceFieldEffect component = GetComponent<ForceFieldEffect>();
                component.DrawWave(hitInfo.point, false);
                Debug.Log("Draw");
            }
        }
    }
}