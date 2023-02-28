using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AnimatedText : MonoBehaviour {
        [SerializeField] protected TextMeshProUGUI message;

        [SerializeField] float textAnimationDelay = 0.01f;

        int currentCharIndex;

        string resultText;

        float timer;

        public bool TextAnimation { get; private set; } = true;

        public string ResultText {
            get => resultText;
            set {
                message.text = string.Empty;
                resultText = value;
            }
        }

        public int CurrentCharIndex {
            get => currentCharIndex;
            set {
                currentCharIndex = value;

                if (currentCharIndex < resultText.Length) {
                    string empty = string.Empty;
                    char c = resultText[currentCharIndex];
                    empty += c;

                    if (c == '<') {
                        while (c != '>' && currentCharIndex < resultText.Length - 1) {
                            currentCharIndex++;
                            c = resultText[currentCharIndex];
                            empty += c;
                        }
                    }

                    message.text += empty;
                } else {
                    message.text = resultText;
                    TextAnimation = false;
                }
            }
        }

        void Reset() {
            message = GetComponent<TextMeshProUGUI>();
        }

        void Update() {
            UpdateTextAnimation();
        }

        void UpdateTextAnimation() {
            if (TextAnimation) {
                timer += Time.deltaTime;

                if (timer > textAnimationDelay) {
                    timer = 0f;
                    CurrentCharIndex++;
                }
            }
        }

        public void Animate() {
            TextAnimation = true;
            CurrentCharIndex = 0;
        }

        public void ForceComplete() {
            CurrentCharIndex = resultText.Length;
        }
    }
}