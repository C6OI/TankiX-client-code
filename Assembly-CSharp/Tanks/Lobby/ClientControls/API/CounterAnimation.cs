using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(Text))]
    public class CounterAnimation : MonoBehaviour {
        [Range(0f, 1f)] public float value;

        int targetValue;

        Text text;

        void Update() {
            text.text = ((int)(value * targetValue)).ToStringSeparatedByThousands();
        }

        void OnEnable() {
            text = GetComponent<Text>();
            targetValue = int.Parse(text.text);
        }
    }
}