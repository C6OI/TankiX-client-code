using System;
using System.Globalization;

namespace YamlDotNet.Serialization {
    static class YamlFormatter {
        public static readonly NumberFormatInfo NumberFormat = new() {
            CurrencyDecimalSeparator = ".",
            CurrencyGroupSeparator = "_",
            CurrencyGroupSizes = new int[1] { 3 },
            CurrencySymbol = string.Empty,
            CurrencyDecimalDigits = 99,
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = "_",
            NumberGroupSizes = new int[1] { 3 },
            NumberDecimalDigits = 99,
            NaNSymbol = ".nan",
            PositiveInfinitySymbol = ".inf",
            NegativeInfinitySymbol = "-.inf"
        };

        public static string FormatNumber(object number) => Convert.ToString(number, NumberFormat);

        public static string FormatBoolean(object boolean) => !boolean.Equals(true) ? "false" : "true";

        public static string FormatDateTime(object dateTime) =>
            ((DateTime)dateTime).ToString("o", CultureInfo.InvariantCulture);

        public static string FormatTimeSpan(object timeSpan) => ((TimeSpan)timeSpan).ToString();
    }
}