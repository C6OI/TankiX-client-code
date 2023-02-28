using System.Collections.Generic;
using System.Linq;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public static class StatsTool {
        public static long GetParameterValue<T>(Dictionary<T, long> dictionary, T key) {
            if (dictionary.ContainsKey(key)) {
                return dictionary[key];
            }

            return 0L;
        }

        public static long GetItemWithLagestValue(Dictionary<long, long> dictionary) {
            return dictionary.OrderBy(pair => pair.Value).LastOrDefault().Key;
        }
    }
}