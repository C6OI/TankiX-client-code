using System;

namespace Tanks.Lobby.ClientControls.API {
    public class KeyUid : IComparable<KeyUid> {
        public string key;

        public string uid;

        public int CompareTo(KeyUid other) => string.Compare(key, other.key, StringComparison.Ordinal);

        public bool Equals(KeyUid other) => string.Equals(key, other.key) && string.Equals(uid, other.uid);

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            return obj is KeyUid && Equals((KeyUid)obj);
        }

        public override int GetHashCode() => (key != null ? key.GetHashCode() : 0) * 397 ^ (uid != null ? uid.GetHashCode() : 0);
    }
}