using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class UidIndicatorComponent : UIBehaviour, Component {
        [SerializeField] Text uidText;

        [SerializeField] TextMeshProUGUI uidTMPText;

        public UserUidInited OnUserUidInited;

        public string Uid {
            get {
                if (uidText != null) {
                    return uidText.text;
                }

                return uidTMPText.text;
            }
            set {
                if (uidText != null) {
                    uidText.text = value;
                } else {
                    uidTMPText.text = value;
                }

                OnUserUidInited.Invoke();
            }
        }

        public Color Color {
            get {
                if (uidText != null) {
                    return uidText.color;
                }

                return uidTMPText.color;
            }
            set {
                if (uidText != null) {
                    uidText.color = value;
                } else {
                    uidTMPText.color = value;
                }
            }
        }

        public FontStyles FontStyle {
            get {
                if (uidTMPText != null) {
                    return uidTMPText.fontStyle;
                }

                Debug.LogWarning("Only TextMeshProUGUI  supported!");
                return FontStyles.Normal;
            }
            set {
                if (uidTMPText != null) {
                    uidTMPText.fontStyle = value;
                } else {
                    Debug.LogWarning("Only TextMeshProUGUI  supported!");
                }
            }
        }

        new void Awake() {
            if (uidTMPText != null) {
                uidTMPText.text = string.Empty;
            }
        }

        new void OnDestroy() {
            OnUserUidInited.RemoveAllListeners();
        }

        [Serializable]
        public class UserUidInited : UnityEvent { }
    }
}