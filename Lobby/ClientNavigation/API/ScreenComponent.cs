using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientNavigation.API {
    public class ScreenComponent : MonoBehaviour, Component {
        [SerializeField] bool logInHistory = true;

        [SerializeField] bool showHangar = true;

        [SerializeField] bool rotateHangarCamera = true;

        [SerializeField] bool showItemNotifications = true;

        [HideInInspector] [SerializeField] [FormerlySerializedAs("visibleTopPanelItems")]
        List<string> visibleCommonScreenElements = new();

        [SerializeField] bool showNotifications = true;

        [Tooltip("Элемент экрана, который должен быть выбран по умолчанию")] [SerializeField]
        Selectable defaultControl;

        CanvasGroup canvasGroup;

        public List<string> VisibleCommonScreenElements => visibleCommonScreenElements;

        public bool LogInHistory => logInHistory;

        public bool ShowHangar => showHangar;

        public bool RotateHangarCamera => rotateHangarCamera;

        public bool ShowItemNotifications => showItemNotifications;

        public bool ShowNotifications => showNotifications;

        void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup == null) {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        void Reset() => visibleCommonScreenElements.Add(TopPanelElements.HOME_BUTTON.ToString());

        void OnEnable() => StartCoroutine(DelayFocus());

        IEnumerator DelayFocus() {
            yield return new WaitForSeconds(0f);

            if (defaultControl != null) {
                EventSystem.current.SetSelectedGameObject(null);
                defaultControl.Select();
            }
        }

        public void Lock() {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        public void Unlock() {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
    }
}