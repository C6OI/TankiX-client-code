using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(Graphic))]
    public class CopyColor : MonoBehaviour {
        [SerializeField] List<Graphic> targets;

        Graphic source;

        void Awake() {
            source = GetComponent<Graphic>();
        }

        void Update() {
            foreach (Graphic target in targets) {
                target.color = source.color;
            }
        }
    }
}