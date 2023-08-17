using System;
using Platform.Library.ClientLocale.API;
using UnityEngine;

namespace Lobby.ClientControls.API {
    public static class IntegerExtensions {
        public static string ToStringSeparatedByThousands(this int value) => value.ToString("N0", LocaleUtils.GetCulture());

        public static string ToStringSeparatedByThousands(this long value) => value.ToString("N0", LocaleUtils.GetCulture());

        public static string ToStringSeparatedByThousands(this double value) {
            if (Math.Abs(value - (int)value) < Mathf.Epsilon) {
                return value.ToString("N0", LocaleUtils.GetCulture());
            }

            return value.ToString("N2", LocaleUtils.GetCulture());
        }
    }
}