using System;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class MessageObject : ECSBehaviour, IPointerClickHandler, IEventSystemHandler {
        [SerializeField] bool first;

        [SerializeField] RectTransform back;

        [SerializeField] ImageSkin userAvatarImageSkin;

        [SerializeField] GameObject userAvatar;

        [SerializeField] GameObject systemAvatar;

        [SerializeField] bool self;

        [SerializeField] TMP_Text nick;

        [SerializeField] TMP_Text text;

        [SerializeField] TMP_Text time;

        [SerializeField] GameObject _tooltipPrefab;

        public bool First => first;

        public ChatMessage Message { get; private set; }

        void Start() {
            if ((bool)nick) {
                nick.gameObject.GetComponent<ChatMessageClickHandler>().Handler = OnClick;
            }

            text.gameObject.GetComponent<ChatMessageClickHandler>().Handler = OnClick;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if ((bool)_tooltipPrefab && eventData.button == PointerEventData.InputButton.Right) {
                string data = Message.Author + ":" + Message.Message;
                TooltipController.Instance.ShowTooltip(Input.mousePosition, data, _tooltipPrefab, false);
            }
        }

        public void Set(ChatMessage message, Func<ChatType, Color> getChatColorFunc) {
            if (first && !self) {
                nick.text = message.GetNickText();
            }

            text.text = message.GetMessageText();
            time.text = message.Time;

            if (!self && first) {
                userAvatar.SetActive(!message.System);
                systemAvatar.SetActive(message.System);

                if (!message.System) {
                    userAvatarImageSkin.SpriteUid = message.AvatarId;
                }
            }

            Message = message;
        }

        public void OnClick(PointerEventData eventData, string link) {
            if (!Message.System) {
                ScheduleEvent(new ChatMessageClickEvent {
                    EventData = eventData,
                    Link = link
                }, new EntityStub());

                if (Input.GetKey(KeyCode.LeftShift)) {
                    GUIUtility.systemCopyBuffer = link;
                }
            }
        }
    }
}