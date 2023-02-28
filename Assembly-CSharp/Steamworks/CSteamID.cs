using System;
using System.Runtime.InteropServices;

namespace Steamworks {
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct CSteamID : IEquatable<CSteamID>, IComparable<CSteamID> {
        public static readonly CSteamID Nil = default;

        public static readonly CSteamID OutofDateGS = new(new AccountID_t(0u), 0u, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

        public static readonly CSteamID LanModeGS = new(new AccountID_t(0u), 0u, EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeInvalid);

        public static readonly CSteamID NotInitYetGS = new(new AccountID_t(1u), 0u, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

        public static readonly CSteamID NonSteamGS = new(new AccountID_t(2u), 0u, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

        public ulong m_SteamID;

        public CSteamID(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType) {
            m_SteamID = 0uL;
            Set(unAccountID, eUniverse, eAccountType);
        }

        public CSteamID(AccountID_t unAccountID, uint unAccountInstance, EUniverse eUniverse, EAccountType eAccountType) {
            m_SteamID = 0uL;
            InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
        }

        public CSteamID(ulong ulSteamID) => m_SteamID = ulSteamID;

        public int CompareTo(CSteamID other) => m_SteamID.CompareTo(other.m_SteamID);

        public bool Equals(CSteamID other) => m_SteamID == other.m_SteamID;

        public void Set(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType) {
            SetAccountID(unAccountID);
            SetEUniverse(eUniverse);
            SetEAccountType(eAccountType);

            if (eAccountType == EAccountType.k_EAccountTypeClan || eAccountType == EAccountType.k_EAccountTypeGameServer) {
                SetAccountInstance(0u);
            } else {
                SetAccountInstance(1u);
            }
        }

        public void InstancedSet(AccountID_t unAccountID, uint unInstance, EUniverse eUniverse, EAccountType eAccountType) {
            SetAccountID(unAccountID);
            SetEUniverse(eUniverse);
            SetEAccountType(eAccountType);
            SetAccountInstance(unInstance);
        }

        public void Clear() {
            m_SteamID = 0uL;
        }

        public void CreateBlankAnonLogon(EUniverse eUniverse) {
            SetAccountID(new AccountID_t(0u));
            SetEUniverse(eUniverse);
            SetEAccountType(EAccountType.k_EAccountTypeAnonGameServer);
            SetAccountInstance(0u);
        }

        public void CreateBlankAnonUserLogon(EUniverse eUniverse) {
            SetAccountID(new AccountID_t(0u));
            SetEUniverse(eUniverse);
            SetEAccountType(EAccountType.k_EAccountTypeAnonUser);
            SetAccountInstance(0u);
        }

        public bool BBlankAnonAccount() => GetAccountID() == new AccountID_t(0u) && BAnonAccount() && GetUnAccountInstance() == 0;

        public bool BGameServerAccount() => GetEAccountType() == EAccountType.k_EAccountTypeGameServer || GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;

        public bool BPersistentGameServerAccount() => GetEAccountType() == EAccountType.k_EAccountTypeGameServer;

        public bool BAnonGameServerAccount() => GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;

        public bool BContentServerAccount() => GetEAccountType() == EAccountType.k_EAccountTypeContentServer;

        public bool BClanAccount() => GetEAccountType() == EAccountType.k_EAccountTypeClan;

        public bool BChatAccount() => GetEAccountType() == EAccountType.k_EAccountTypeChat;

        public bool IsLobby() => GetEAccountType() == EAccountType.k_EAccountTypeChat && (GetUnAccountInstance() & 0x40000) != 0;

        public bool BIndividualAccount() => GetEAccountType() == EAccountType.k_EAccountTypeIndividual || GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;

        public bool BAnonAccount() => GetEAccountType() == EAccountType.k_EAccountTypeAnonUser || GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;

        public bool BAnonUserAccount() => GetEAccountType() == EAccountType.k_EAccountTypeAnonUser;

        public bool BConsoleUserAccount() => GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;

        public void SetAccountID(AccountID_t other) {
            m_SteamID = m_SteamID & 0xFFFFFFFF00000000uL | ((uint)other & 0xFFFFFFFFuL) << 0;
        }

        public void SetAccountInstance(uint other) {
            m_SteamID = m_SteamID & 0xFFF00000FFFFFFFFuL | (other & 0xFFFFFuL) << 32;
        }

        public void SetEAccountType(EAccountType other) {
            m_SteamID = m_SteamID & 0xFF0FFFFFFFFFFFFFuL | ((ulong)other & 0xFuL) << 52;
        }

        public void SetEUniverse(EUniverse other) {
            m_SteamID = m_SteamID & 0xFFFFFFFFFFFFFFuL | ((ulong)other & 0xFFuL) << 56;
        }

        public void ClearIndividualInstance() {
            if (BIndividualAccount()) {
                SetAccountInstance(0u);
            }
        }

        public bool HasNoIndividualInstance() => BIndividualAccount() && GetUnAccountInstance() == 0;

        public AccountID_t GetAccountID() => new((uint)(m_SteamID & 0xFFFFFFFFu));

        public uint GetUnAccountInstance() => (uint)(m_SteamID >> 32 & 0xFFFFF);

        public EAccountType GetEAccountType() => (EAccountType)(m_SteamID >> 52 & 0xF);

        public EUniverse GetEUniverse() => (EUniverse)(m_SteamID >> 56 & 0xFF);

        public bool IsValid() {
            if (GetEAccountType() <= EAccountType.k_EAccountTypeInvalid || GetEAccountType() >= EAccountType.k_EAccountTypeMax) {
                return false;
            }

            if (GetEUniverse() <= EUniverse.k_EUniverseInvalid || GetEUniverse() >= EUniverse.k_EUniverseMax) {
                return false;
            }

            if (GetEAccountType() == EAccountType.k_EAccountTypeIndividual && (GetAccountID() == new AccountID_t(0u) || GetUnAccountInstance() > 4)) {
                return false;
            }

            if (GetEAccountType() == EAccountType.k_EAccountTypeClan && (GetAccountID() == new AccountID_t(0u) || GetUnAccountInstance() != 0)) {
                return false;
            }

            if (GetEAccountType() == EAccountType.k_EAccountTypeGameServer && GetAccountID() == new AccountID_t(0u)) {
                return false;
            }

            return true;
        }

        public override string ToString() => m_SteamID.ToString();

        public override bool Equals(object other) => other is CSteamID && this == (CSteamID)other;

        public override int GetHashCode() => m_SteamID.GetHashCode();

        public static bool operator ==(CSteamID x, CSteamID y) => x.m_SteamID == y.m_SteamID;

        public static bool operator !=(CSteamID x, CSteamID y) => !(x == y);

        public static explicit operator CSteamID(ulong value) => new(value);

        public static explicit operator ulong(CSteamID that) => that.m_SteamID;
    }
}