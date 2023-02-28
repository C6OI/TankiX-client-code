using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatMessageClickHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler {
        [SerializeField] TMP_Text textMessage;

        public Action<PointerEventData, string> Handler;

        public void OnPointerClick(PointerEventData eventData) {
            int num = TMP_TextUtilities.FindIntersectingLink(textMessage, eventData.position, eventData.pressEventCamera);

            if (num == -1) {
                return;
            }

            TMP_LinkInfo tMP_LinkInfo = textMessage.textInfo.linkInfo[num];
            string linkID = tMP_LinkInfo.GetLinkID();

            if (!(linkID == string.Empty)) {
                if (Handler != null) {
                    Handler(eventData, linkID);
                } else {
                    Application.OpenURL(linkID);
                }
            }
        }
    }
}