using System;

namespace Steamworks {
    [Serializable]
    public struct servernetadr_t {
        uint m_unIP;
        ushort m_usConnectionPort;

        ushort m_usQueryPort;

        public void Init(uint ip, ushort usQueryPort, ushort usConnectionPort) {
            m_unIP = ip;
            m_usQueryPort = usQueryPort;
            m_usConnectionPort = usConnectionPort;
        }

        public ushort GetQueryPort() => m_usQueryPort;

        public void SetQueryPort(ushort usPort) {
            m_usQueryPort = usPort;
        }

        public ushort GetConnectionPort() => m_usConnectionPort;

        public void SetConnectionPort(ushort usPort) {
            m_usConnectionPort = usPort;
        }

        public uint GetIP() => m_unIP;

        public void SetIP(uint unIP) {
            m_unIP = unIP;
        }

        public string GetConnectionAddressString() => ToString(m_unIP, m_usConnectionPort);

        public string GetQueryAddressString() => ToString(m_unIP, m_usQueryPort);

        public static string ToString(uint unIP, ushort usPort) =>
            string.Format("{0}.{1}.{2}.{3}:{4}", unIP >> 24 & 0xFFuL, unIP >> 16 & 0xFFuL, unIP >> 8 & 0xFFuL, unIP & 0xFFuL, usPort);

        public static bool operator <(servernetadr_t x, servernetadr_t y) => x.m_unIP < y.m_unIP || x.m_unIP == y.m_unIP && x.m_usQueryPort < y.m_usQueryPort;

        public static bool operator >(servernetadr_t x, servernetadr_t y) => x.m_unIP > y.m_unIP || x.m_unIP == y.m_unIP && x.m_usQueryPort > y.m_usQueryPort;

        public override bool Equals(object other) => other is servernetadr_t && this == (servernetadr_t)other;

        public override int GetHashCode() => m_unIP.GetHashCode() + m_usQueryPort.GetHashCode() + m_usConnectionPort.GetHashCode();

        public static bool operator ==(servernetadr_t x, servernetadr_t y) => x.m_unIP == y.m_unIP && x.m_usQueryPort == y.m_usQueryPort && x.m_usConnectionPort == y.m_usConnectionPort;

        public static bool operator !=(servernetadr_t x, servernetadr_t y) => !(x == y);

        public bool Equals(servernetadr_t other) => m_unIP == other.m_unIP && m_usQueryPort == other.m_usQueryPort && m_usConnectionPort == other.m_usConnectionPort;

        public int CompareTo(servernetadr_t other) =>
            m_unIP.CompareTo(other.m_unIP) + m_usQueryPort.CompareTo(other.m_usQueryPort) + m_usConnectionPort.CompareTo(other.m_usConnectionPort);
    }
}