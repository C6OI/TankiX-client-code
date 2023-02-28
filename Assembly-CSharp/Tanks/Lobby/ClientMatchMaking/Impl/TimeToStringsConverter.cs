using UnityEngine;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class TimeToStringsConverter : MonoBehaviour {
        public static string MillisecondsToTimerFormat(double milliseconds) {
            double seconds = milliseconds / 1000.0;
            return SecondsToTimerFormat(seconds);
        }

        public static string SecondsToTimerFormat(double seconds) {
            int num = (int)(seconds / 60.0);
            int num2 = (int)(seconds - num * 60);
            return string.Format("{0:0}:{1:00}", num, num2);
        }
    }
}