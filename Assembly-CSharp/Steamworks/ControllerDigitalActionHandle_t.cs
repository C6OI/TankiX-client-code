using System;

namespace Steamworks {
    [Serializable]
    public struct ControllerDigitalActionHandle_t : IEquatable<ControllerDigitalActionHandle_t>, IComparable<ControllerDigitalActionHandle_t> {
        public ulong m_ControllerDigitalActionHandle;

        public ControllerDigitalActionHandle_t(ulong value) => m_ControllerDigitalActionHandle = value;

        public int CompareTo(ControllerDigitalActionHandle_t other) => m_ControllerDigitalActionHandle.CompareTo(other.m_ControllerDigitalActionHandle);

        public bool Equals(ControllerDigitalActionHandle_t other) => m_ControllerDigitalActionHandle == other.m_ControllerDigitalActionHandle;

        public override string ToString() => m_ControllerDigitalActionHandle.ToString();

        public override bool Equals(object other) => other is ControllerDigitalActionHandle_t && this == (ControllerDigitalActionHandle_t)other;

        public override int GetHashCode() => m_ControllerDigitalActionHandle.GetHashCode();

        public static bool operator ==(ControllerDigitalActionHandle_t x, ControllerDigitalActionHandle_t y) => x.m_ControllerDigitalActionHandle == y.m_ControllerDigitalActionHandle;

        public static bool operator !=(ControllerDigitalActionHandle_t x, ControllerDigitalActionHandle_t y) => !(x == y);

        public static explicit operator ControllerDigitalActionHandle_t(ulong value) => new(value);

        public static explicit operator ulong(ControllerDigitalActionHandle_t that) => that.m_ControllerDigitalActionHandle;
    }
}