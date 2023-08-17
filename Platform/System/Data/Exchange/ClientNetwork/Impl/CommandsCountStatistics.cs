using System.Collections.Generic;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class CommandsCountStatistics {
        const int RANGES = 100;

        readonly List<KeyValuePair<float, int>> entries = new(100);

        readonly float warnLimitTime;

        int newestPos;

        public CommandsCountStatistics(float warnLimitTime) {
            this.warnLimitTime = warnLimitTime;

            for (int i = 0; i < 100; i++) {
                entries.Add(new KeyValuePair<float, int>(0f, 0));
            }
        }

        public void AddCommands(int commandsCount, float time) {
            ResetOldEntries(time);
            float time2 = GetTime(newestPos);

            if (IsOldTime(time2, time)) {
                entries[newestPos] = new KeyValuePair<float, int>(time, commandsCount);
                return;
            }

            time2 = entries[newestPos].Key;
            float num = (time - time2) / warnLimitTime;
            int offset = (int)(num * 100f);
            newestPos = OffsetPos(newestPos, offset);

            entries[newestPos] =
                new KeyValuePair<float, int>(entries[newestPos].Key, entries[newestPos].Value + commandsCount);
        }

        public int GetCommandsInFixedPeriod(float time) {
            ResetOldEntries(time);
            int num = newestPos;
            int num2 = 0;
            int num3 = 0;

            while (!IsOldTime(GetTime(num), time) && num3++ < 100) {
                num2 += entries[num].Value;
                num = PrevPos(num);
            }

            return num2;
        }

        void ResetOldEntries(float time) {
            int num = 0;
            int num2 = NextPos(newestPos);

            while (IsOldTime(GetTime(num2), time) && num++ < 100) {
                entries[num2] = new KeyValuePair<float, int>(0f, 0);
                num2 = NextPos(num2);
            }
        }

        bool IsOldTime(float time, float nowTime) => nowTime - warnLimitTime > time;

        float GetTime(int pos) => entries[pos].Key;

        int NextPos(int pos) => OffsetPos(pos, 1);

        int PrevPos(int pos) => OffsetPos(pos, 99);

        int OffsetPos(int pos, int offset) => (pos + offset) % 100;
    }
}