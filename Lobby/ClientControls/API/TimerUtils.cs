using System;
using System.Text;
using UnityEngine;

namespace Lobby.ClientControls.API {
    public static class TimerUtils {
        static readonly StringBuilder stringBuilder = new();

        public static string GetTimerText(float timeSeconds) {
            if (timeSeconds < 0f) {
                timeSeconds = 0f;
            }

            int num = Mathf.FloorToInt(timeSeconds / 60f / 60f);
            int num2 = Mathf.FloorToInt(timeSeconds / 60f) - 60 * num;
            int num3 = Mathf.FloorToInt(timeSeconds % 60f);
            stringBuilder.Length = 0;

            if (num > 0) {
                stringBuilder.Append(num);
                stringBuilder.Append(":");
            }

            if (num2 < 10) {
                stringBuilder.Append("0");
            }

            stringBuilder.Append(num2);
            stringBuilder.Append(":");

            if (num3 < 10) {
                stringBuilder.Append("0");
            }

            stringBuilder.Append(num3);
            return stringBuilder.ToString();
        }

        public static string GetTimerTextSeconds(TimeSpan timeSpan) {
            stringBuilder.Length = 0;
            stringBuilder.Append((int)timeSpan.TotalSeconds);
            return stringBuilder.ToString();
        }
    }
}