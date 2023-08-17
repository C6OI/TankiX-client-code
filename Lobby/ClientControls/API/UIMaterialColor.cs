using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    public class UIMaterialColor : MonoBehaviour {
        Graphic graphic;

        Material material;

        void Awake() {
            graphic = GetComponent<Graphic>();
            material = new Material(graphic.material);
            graphic.material = material;
        }

        void Update() {
            if (material.color != graphic.color) {
                material.SetColor("_Color", graphic.color);
            }
        }
    }
}