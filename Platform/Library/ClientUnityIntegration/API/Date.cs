using System;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public struct Date : IComparable<Date> {
        [Inject] public static UnityTime Time { get; set; }

        public float UnityTime { get; set; }

        public static Date Now => new(Time.realtimeSinceStartup);

        public Date(float unityTime) => UnityTime = unityTime;

        public static float FromServerTime(long diffToServer, long serverTime) {
            long num = serverTime - diffToServer;
            return num / 1000f;
        }

        public long ToServerTime(long diffToServer) => (long)(UnityTime * 1000f) + diffToServer;

        public Date AddSeconds(float seconds) => new(UnityTime + seconds);

        public Date AddMilliseconds(long milliseconds) => new(UnityTime + milliseconds / 1000f);

        public float GetProgress(Date beginDate, Date endDate) => GetProgress(beginDate, endDate - beginDate);

        public float GetProgress(Date beginDate, float durationSeconds) {
            float num = UnityTime - beginDate.UnityTime;
            return Mathf.Clamp01(num / durationSeconds);
        }

        public override int GetHashCode() => UnityTime.GetHashCode();

        public int CompareTo(Date other) => UnityTime.CompareTo(other.UnityTime);

        public override string ToString() => UnityTime.ToString();

        public static Date operator +(Date self, float seconds) => new(self.UnityTime + seconds);

        public static Date operator -(Date self, float seconds) => new(self.UnityTime - seconds);

        public static float operator -(Date self, Date other) => self.UnityTime - other.UnityTime;

        public static bool operator ==(Date t1, Date t2) => t1.UnityTime == t2.UnityTime;

        public static bool operator !=(Date t1, Date t2) => t1.UnityTime != t2.UnityTime;

        public static bool operator <(Date t1, Date t2) => t1.UnityTime < t2.UnityTime;

        public static bool operator <=(Date t1, Date t2) => t1.UnityTime <= t2.UnityTime;

        public static bool operator >(Date t1, Date t2) => t1.UnityTime > t2.UnityTime;

        public static bool operator >=(Date t1, Date t2) => t1.UnityTime >= t2.UnityTime;
    }
}