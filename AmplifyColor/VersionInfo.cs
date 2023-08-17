using System;
using UnityEngine;

namespace AmplifyColor {
    [Serializable]
    public class VersionInfo {
        public const byte Major = 1;

        public const byte Minor = 4;

        public const byte Release = 4;

        static readonly string StageSuffix = "_dev007";

        static readonly string TrialSuffix = string.Empty;

        [SerializeField] int m_major;

        [SerializeField] int m_minor;

        [SerializeField] int m_release;

        VersionInfo() {
            m_major = 1;
            m_minor = 4;
            m_release = 4;
        }

        VersionInfo(byte major, byte minor, byte release) {
            m_major = major;
            m_minor = minor;
            m_release = release;
        }

        public int Number => m_major * 100 + m_minor * 10 + m_release;

        public static string StaticToString() =>
            string.Format("{0}.{1}.{2}", (byte)1, (byte)4, (byte)4) + StageSuffix + TrialSuffix;

        public override string ToString() =>
            string.Format("{0}.{1}.{2}", m_major, m_minor, m_release) + StageSuffix + TrialSuffix;

        public static VersionInfo Current() => new(1, 4, 4);

        public static bool Matches(VersionInfo version) =>
            version.m_major == 1 && version.m_minor == 4 && 4 == version.m_release;
    }
}