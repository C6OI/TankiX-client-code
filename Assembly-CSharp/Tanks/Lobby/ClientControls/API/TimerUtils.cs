using System;
using System.Text;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public static class TimerUtils {
        static readonly StringBuilder stringBuilder = new();

        public static string GetTimerText(float timeSeconds) {
            stringBuilder.Length = 0;
            return GetTimerText(stringBuilder, timeSeconds);
        }

        public static string GetTimerText(StringBuilder builder, float timeSeconds) {
            if (timeSeconds < 0f) {
                timeSeconds = 0f;
            }

            int num = Mathf.FloorToInt(timeSeconds / 60f / 60f);
            int num2 = Mathf.FloorToInt(timeSeconds / 60f) - 60 * num;
            int num3 = Mathf.FloorToInt(timeSeconds % 60f);

            if (num > 0) {
                builder.Append(num);
                builder.Append(":");
            }

            if (num2 < 10) {
                builder.Append("0");
            }

            builder.Append(num2);
            builder.Append(":");

            if (num3 < 10) {
                builder.Append("0");
            }

            builder.Append(num3);
            return builder.ToString();
        }

        public static string GetTimerTextSeconds(TimeSpan timeSpan) {
            stringBuilder.Length = 0;
            stringBuilder.Append((int)timeSpan.TotalSeconds);
            return stringBuilder.ToString();
        }
    }
}