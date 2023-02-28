using System;

namespace Steamworks {
    [Serializable]
    public struct CGameID : IEquatable<CGameID>, IComparable<CGameID> {
        public enum EGameIDType {
            k_EGameIDTypeApp = 0,
            k_EGameIDTypeGameMod = 1,
            k_EGameIDTypeShortcut = 2,
            k_EGameIDTypeP2P = 3
        }

        public ulong m_GameID;

        public CGameID(ulong GameID) => m_GameID = GameID;

        public CGameID(AppId_t nAppID) {
            m_GameID = 0uL;
            SetAppID(nAppID);
        }

        public CGameID(AppId_t nAppID, uint nModID) {
            m_GameID = 0uL;
            SetAppID(nAppID);
            SetType(EGameIDType.k_EGameIDTypeGameMod);
            SetModID(nModID);
        }

        public int CompareTo(CGameID other) => m_GameID.CompareTo(other.m_GameID);

        public bool Equals(CGameID other) => m_GameID == other.m_GameID;

        public bool IsSteamApp() => Type() == EGameIDType.k_EGameIDTypeApp;

        public bool IsMod() => Type() == EGameIDType.k_EGameIDTypeGameMod;

        public bool IsShortcut() => Type() == EGameIDType.k_EGameIDTypeShortcut;

        public bool IsP2PFile() => Type() == EGameIDType.k_EGameIDTypeP2P;

        public AppId_t AppID() => new((uint)(m_GameID & 0xFFFFFF));

        public EGameIDType Type() => (EGameIDType)(m_GameID >> 24 & 0xFF);

        public uint ModID() => (uint)(m_GameID >> 32 & 0xFFFFFFFFu);

        public bool IsValid() {
            switch (Type()) {
                case EGameIDType.k_EGameIDTypeApp:
                    return AppID() != AppId_t.Invalid;

                case EGameIDType.k_EGameIDTypeGameMod:
                    return AppID() != AppId_t.Invalid && (ModID() & 0x80000000u) != 0;

                case EGameIDType.k_EGameIDTypeShortcut:
                    return (ModID() & 0x80000000u) != 0;

                case EGameIDType.k_EGameIDTypeP2P:
                    return AppID() == AppId_t.Invalid && (ModID() & 0x80000000u) != 0;

                default:
                    return false;
            }
        }

        public void Reset() {
            m_GameID = 0uL;
        }

        public void Set(ulong GameID) {
            m_GameID = GameID;
        }

        void SetAppID(AppId_t other) {
            m_GameID = m_GameID & 0xFFFFFFFFFF000000uL | ((uint)other & 0xFFFFFFuL) << 0;
        }

        void SetType(EGameIDType other) {
            m_GameID = m_GameID & 0xFFFFFFFF00FFFFFFuL | ((ulong)other & 0xFFuL) << 24;
        }

        void SetModID(uint other) {
            m_GameID = m_GameID & 0xFFFFFFFFu | (other & 0xFFFFFFFFuL) << 32;
        }

        public override string ToString() => m_GameID.ToString();

        public override bool Equals(object other) => other is CGameID && this == (CGameID)other;

        public override int GetHashCode() => m_GameID.GetHashCode();

        public static bool operator ==(CGameID x, CGameID y) => x.m_GameID == y.m_GameID;

        public static bool operator !=(CGameID x, CGameID y) => !(x == y);

        public static explicit operator CGameID(ulong value) => new(value);

        public static explicit operator ulong(CGameID that) => that.m_GameID;
    }
}