using System;

namespace Steamworks {
    [Serializable]
    public struct ControllerAnalogActionHandle_t : IEquatable<ControllerAnalogActionHandle_t>, IComparable<ControllerAnalogActionHandle_t> {
        public ulong m_ControllerAnalogActionHandle;

        public ControllerAnalogActionHandle_t(ulong value) => m_ControllerAnalogActionHandle = value;

        public int CompareTo(ControllerAnalogActionHandle_t other) => m_ControllerAnalogActionHandle.CompareTo(other.m_ControllerAnalogActionHandle);

        public bool Equals(ControllerAnalogActionHandle_t other) => m_ControllerAnalogActionHandle == other.m_ControllerAnalogActionHandle;

        public override string ToString() => m_ControllerAnalogActionHandle.ToString();

        public override bool Equals(object other) => other is ControllerAnalogActionHandle_t && this == (ControllerAnalogActionHandle_t)other;

        public override int GetHashCode() => m_ControllerAnalogActionHandle.GetHashCode();

        public static bool operator ==(ControllerAnalogActionHandle_t x, ControllerAnalogActionHandle_t y) => x.m_ControllerAnalogActionHandle == y.m_ControllerAnalogActionHandle;

        public static bool operator !=(ControllerAnalogActionHandle_t x, ControllerAnalogActionHandle_t y) => !(x == y);

        public static explicit operator ControllerAnalogActionHandle_t(ulong value) => new(value);

        public static explicit operator ulong(ControllerAnalogActionHandle_t that) => that.m_ControllerAnalogActionHandle;
    }
}