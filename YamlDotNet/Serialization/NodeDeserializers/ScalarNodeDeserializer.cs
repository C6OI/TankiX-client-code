using System;
using System.Globalization;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers {
    public sealed class ScalarNodeDeserializer : INodeDeserializer {
        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType,
            Func<EventReader, Type, object> nestedObjectDeserializer, out object value) {
            Scalar scalar = reader.Allow<Scalar>();

            if (scalar == null) {
                value = null;
                return false;
            }

            if (expectedType.IsEnum()) {
                value = Enum.Parse(expectedType, scalar.Value);
            } else {
                TypeCode typeCode = expectedType.GetTypeCode();

                switch (typeCode) {
                    case TypeCode.Boolean:
                        value = bool.Parse(scalar.Value);
                        break;

                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        value = DeserializeIntegerHelper(typeCode, scalar.Value, YamlFormatter.NumberFormat);
                        break;

                    case TypeCode.Single:
                        value = float.Parse(scalar.Value, YamlFormatter.NumberFormat);
                        break;

                    case TypeCode.Double:
                        value = double.Parse(scalar.Value, YamlFormatter.NumberFormat);
                        break;

                    case TypeCode.Decimal:
                        value = decimal.Parse(scalar.Value, YamlFormatter.NumberFormat);
                        break;

                    case TypeCode.String:
                        value = scalar.Value;
                        break;

                    case TypeCode.Char:
                        value = scalar.Value[0];
                        break;

                    case TypeCode.DateTime:
                        value = DateTime.Parse(scalar.Value, CultureInfo.InvariantCulture);
                        break;

                    default:
                        if (expectedType == typeof(object)) {
                            value = scalar.Value;
                        } else {
                            value = TypeConverter.ChangeType(scalar.Value, expectedType);
                        }

                        break;
                }
            }

            return true;
        }

        object DeserializeIntegerHelper(TypeCode typeCode, string value, IFormatProvider formatProvider) {
            StringBuilder stringBuilder = new();
            int i = 0;
            bool flag = false;
            int num = 0;
            long num2 = 0L;

            if (value[0] == '-') {
                i++;
                flag = true;
            } else if (value[0] == '+') {
                i++;
            }

            if (value[i] == '0') {
                if (i == value.Length - 1) {
                    num = 10;
                    num2 = 0L;
                } else {
                    i++;

                    if (value[i] == 'b') {
                        num = 2;
                        i++;
                    } else if (value[i] == 'x') {
                        num = 16;
                        i++;
                    } else {
                        num = 8;
                    }
                }

                for (; i < value.Length; i++) {
                    if (value[i] != '_') {
                        stringBuilder.Append(value[i]);
                    }
                }

                switch (num) {
                    case 2:
                    case 8:
                        num2 = Convert.ToInt64(stringBuilder.ToString(), num);
                        break;

                    case 16:
                        num2 = long.Parse(stringBuilder.ToString(), NumberStyles.HexNumber, YamlFormatter.NumberFormat);
                        break;
                }
            } else {
                string[] array = value.Substring(i).Split(':');
                num2 = 0L;

                for (int j = 0; j < array.Length; j++) {
                    num2 *= 60;
                    num2 += long.Parse(array[j].Replace("_", string.Empty));
                }
            }

            if (flag) {
                num2 = -num2;
            }

            switch (typeCode) {
                case TypeCode.Byte:
                    return (byte)num2;

                case TypeCode.Int16:
                    return (short)num2;

                case TypeCode.Int32:
                    return (int)num2;

                case TypeCode.Int64:
                    return num2;

                case TypeCode.SByte:
                    return (sbyte)num2;

                case TypeCode.UInt16:
                    return (ushort)num2;

                case TypeCode.UInt32:
                    return (uint)num2;

                case TypeCode.UInt64:
                    return (ulong)num2;

                default:
                    return num2;
            }
        }
    }
}