using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public static class UIExtensions {
        static string GetHex(int num) {
            string text = "0123456789ABCDEF";
            return text[num].ToString();
        }

        public static string ToHexString(this Color color) {
            int num = (int)(color.r * 255f);
            int num2 = (int)(color.g * 255f);
            int num3 = (int)(color.b * 255f);
            return GetHex(num / 16) + GetHex(num % 16) + GetHex(num2 / 16) + GetHex(num2 % 16) + GetHex(num3 / 16) + GetHex(num3 % 16);
        }
    }
}