using System;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatMessage {
        public static Color SystemColor = Color.yellow;

        public string Author { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }

        public bool System { get; set; }

        public bool Self { get; set; }

        public ChatType ChatType { get; set; }

        public long ChatId { get; set; }

        public string AvatarId { get; set; }

        public string GetMessageText() => !System ? string.Format("<noparse>{0}</noparse>", Message) : Message;

        public string GetNickText() {
            string author = Author;
            string text = author;

            if (!System) {
                text = "@" + text;
            }

            return string.Format("<link=\"{0}\">{1}</link>", author, text);
        }

        public string GetEllipsis(Func<ChatType, Color> getChatColorFunc) {
            string text = string.Empty;
            string text2 = Message;

            if (!Self) {
                text = System ? Author : "@" + Author;
                text2 = text + ": " + Message;
            }

            int num = 32;
            bool flag = false;

            if (text2.Length <= num) {
                num = text2.Length;
            } else {
                flag = true;
            }

            int num2 = text2.IndexOf("\n");

            if (num2 > 0) {
                flag = true;

                if (num2 < num) {
                    num = num2;
                }
            }

            string text3 = getChatColorFunc(ChatType).ToHexString();
            text2 = text2.Substring(text.Length, num - text.Length);

            if (System) {
                text3 = SystemColor.ToHexString();
            } else {
                text2 = string.Format("<noparse>{0}</noparse>", text2);
            }

            string text4 = !flag ? string.Empty : "...";
            return string.Format("<color=#{0}>{1}</color>{2}{3}", text3, text, text2, text4);
        }
    }
}