using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    public static class IdleKickUtils {
        public static float CalculateTimeLeft(IdleCounterComponent idleCounter, IdleBeginTimeComponent idleBeginTime,
            IdleKickConfigComponent config) {
            float num = CalculateIdleTime(idleCounter, idleBeginTime.IdleBeginTime);
            float num2 = config.IdleKickTimeSec - num;
            return !(num2 < 0f) ? num2 : 0f;
        }

        public static float CalculateIdleTime(IdleCounterComponent idleCounter, Date? idleBeginTime) {
            if (!idleBeginTime.HasValue) {
                return 0f;
            }

            Date date = !idleCounter.SkipBeginTime.IsPresent() ? Date.Now : idleCounter.SkipBeginTime.Get();
            float num = date - idleBeginTime.Value;
            return num - idleCounter.SkippedMillis / 1000f;
        }
    }
}