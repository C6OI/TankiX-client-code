using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModulePropertyView : MonoBehaviour {
        [SerializeField] TextMeshProUGUI propertyName;

        [SerializeField] TextMeshProUGUI currentParam;

        [SerializeField] TextMeshProUGUI nextParam;

        [SerializeField] ImageSkin icon;

        [SerializeField] GameObject Progress;

        [SerializeField] Image currentProgress;

        [SerializeField] Image nextProgress;

        public GameObject FillNext;

        public GameObject NextString;

        float current;

        string format;

        float next;

        string units;

        public float CurentProgressBar {
            set => current = value;
        }

        public float nextProgressBar {
            set => next = value;
        }

        public string Units {
            set => units = value;
        }

        public string PropertyName {
            set => propertyName.text = value;
        }

        public string Format {
            get => format ?? "{0:0}";
            set {
                if (string.IsNullOrEmpty(value)) {
                    format = "{0:0}";
                } else {
                    format = "{0:" + value + "}";
                }
            }
        }

        public string CurrentParamString {
            set => currentParam.text = string.Format(Format, value);
        }

        public string NextParamString {
            set => nextParam.text = string.Format(Format, value);
        }

        public float CurrentParam {
            set {
                currentParam.text = string.Format(Format, value) + " " + units;
                current = value;
            }
        }

        public float NextParam {
            set {
                nextParam.text = string.Format(Format, value) + " " + units;

                if (nextParam.text == currentParam.text) {
                    nextParam.text = string.Format(Format, " ");
                }

                next = value;
            }
        }

        public bool ProgressBar {
            set => Progress.SetActive(value);
        }

        public string SpriteUid {
            set => icon.SpriteUid = value;
        }

        public float MaxParam {
            set {
                currentProgress.fillAmount = current / value;
                nextProgress.fillAmount = next / value;
            }
        }
    }
}