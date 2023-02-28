using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Tanks.Lobby.ClientProfile.API {
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct LevelInfo {
        public int Experience { get; private set; }

        public int Level { get; private set; }

        public int MaxExperience { get; private set; }

        public long AbsolutExperience { get; private set; }

        public bool IsMaxLevel { get; private set; }

        public float Progress => Mathf.Clamp01(Experience / (float)MaxExperience);

        public LevelInfo(int level) {
            this = default;
            Level = level;
        }

        public bool Equals(LevelInfo other) => AbsolutExperience == other.AbsolutExperience;

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            return obj is LevelInfo && Equals((LevelInfo)obj);
        }

        public override int GetHashCode() => AbsolutExperience.GetHashCode();

        public void ClampExp() {
            Experience = Mathf.Min(MaxExperience, Experience);
        }

        public static LevelInfo Get(long absExp, int[] levels) {
            int num = Math.Abs(Array.BinarySearch(levels, (int)absExp) + 1);
            bool flag = false;

            if (num >= levels.Length) {
                num = levels.Length - 1;
                flag = true;
            }

            int num2 = num != 0 ? levels[num - 1] : 0;
            int num3 = levels[num];
            LevelInfo result = default;
            result.Experience = (int)(absExp - num2);
            result.Level = !flag ? num : num + 1;
            result.MaxExperience = num3 - num2;
            result.AbsolutExperience = absExp;
            result.IsMaxLevel = flag;
            return result;
        }

        public static bool operator ==(LevelInfo left, LevelInfo right) => left.Experience == right.Experience && left.Level == right.Level;

        public static bool operator !=(LevelInfo left, LevelInfo right) => left.Experience != right.Experience || left.Level != right.Level;
    }
}