using System.Text;
using Tanks.Battle.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientLoading.API {
    public class LoadProgressBarView : MonoBehaviour {
        public ProgressBarComponent progressBar;

        public RectTransform endLine;

        public TextMeshProUGUI progressText;

        RectTransform _progressBarRectTransform;

        RectTransform _progressTextRectTransform;

        readonly StringBuilder stringBuilder = new();

        RectTransform progressBarRectTransform {
            get {
                if (_progressBarRectTransform == null) {
                    _progressBarRectTransform = progressBar.GetComponent<RectTransform>();
                }

                return _progressBarRectTransform;
            }
        }

        RectTransform progressTextRectTransform {
            get {
                if (_progressTextRectTransform == null) {
                    _progressTextRectTransform = progressText.GetComponent<RectTransform>();
                }

                return _progressTextRectTransform;
            }
        }

        public float ProgressValue {
            get => progressBar.ProgressValue;
            set {
                progressBar.ProgressValue = value;
                LateUpdate();
            }
        }

        void LateUpdate() {
            if (!(progressBarRectTransform == null)) {
                float progressValue = progressBar.ProgressBar.ProgressValue;
                Rect rect = progressBarRectTransform.rect;
                float positionX = rect.x + rect.width * progressValue;
                SetPosition(positionX, endLine, 1f);
                UpdateProgressText(progressValue);
                SetPosition(positionX, progressTextRectTransform, progressTextRectTransform.rect.width / 2f);
            }
        }

        void UpdateProgressText(float progress) {
            if (!(progressText == null)) {
                stringBuilder.Length = 0;
                stringBuilder.Append(Mathf.Ceil(progress * 100f));
                stringBuilder.Append("%");
                progressText.text = stringBuilder.ToString();

                if (progress >= 1f) {
                    progressText.GetComponent<Animator>().SetBool("hide", true);
                }
            }
        }

        void SetPosition(float positionX, RectTransform uiElement, float borderOffset) {
            Rect rect = progressBarRectTransform.rect;
            Vector2 anchoredPosition = uiElement.anchoredPosition;
            anchoredPosition.x = Mathf.Clamp(positionX, rect.x + borderOffset, rect.x + rect.width - borderOffset);
            uiElement.anchoredPosition = anchoredPosition;
        }
    }
}