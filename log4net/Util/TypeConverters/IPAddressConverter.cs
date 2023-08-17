using System;
using System.Net;

namespace log4net.Util.TypeConverters {
    class IPAddressConverter : IConvertFrom {
        static readonly char[] validIpAddressChars = new char[27] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D',
            'E', 'F', 'x', 'X', '.', ':', '%'
        };

        public bool CanConvertFrom(Type sourceType) => sourceType == typeof(string);

        public object ConvertFrom(object source) {
            string text = source as string;

            if (text != null && text.Length > 0) {
                try {
                    if (text.Trim(validIpAddressChars).Length == 0) {
                        try {
                            return IPAddress.Parse(text);
                        } catch (FormatException) { }
                    }

                    IPHostEntry hostByName = Dns.GetHostByName(text);

                    if (hostByName != null &&
                        hostByName.AddressList != null &&
                        hostByName.AddressList.Length > 0 &&
                        hostByName.AddressList[0] != null) {
                        return hostByName.AddressList[0];
                    }
                } catch (Exception innerException) {
                    throw ConversionNotSupportedException.Create(typeof(IPAddress), source, innerException);
                }
            }

            throw ConversionNotSupportedException.Create(typeof(IPAddress), source);
        }
    }
}