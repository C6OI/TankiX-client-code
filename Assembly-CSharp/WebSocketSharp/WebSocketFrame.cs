using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebSocketSharp {
    internal class WebSocketFrame : IEnumerable<byte>, IEnumerable {
        internal static readonly byte[] EmptyPingBytes;

        static WebSocketFrame() => EmptyPingBytes = CreatePingFrame(false).ToArray();

        WebSocketFrame() { }

        internal WebSocketFrame(Opcode opcode, PayloadData payloadData, bool mask)
            : this(Fin.Final, opcode, payloadData, false, mask) { }

        internal WebSocketFrame(Fin fin, Opcode opcode, byte[] data, bool compressed, bool mask)
            : this(fin, opcode, new PayloadData(data), compressed, mask) { }

        internal WebSocketFrame(Fin fin, Opcode opcode, PayloadData payloadData, bool compressed, bool mask) {
            Fin = fin;
            Rsv1 = opcode.IsData() && compressed ? Rsv.On : Rsv.Off;
            Rsv2 = Rsv.Off;
            Rsv3 = Rsv.Off;
            Opcode = opcode;
            ulong length = payloadData.Length;

            if (length < 126) {
                PayloadLength = (byte)length;
                ExtendedPayloadLength = WebSocket.EmptyBytes;
            } else if (length < 65536) {
                PayloadLength = 126;
                ExtendedPayloadLength = ((ushort)length).InternalToByteArray(ByteOrder.Big);
            } else {
                PayloadLength = 127;
                ExtendedPayloadLength = length.InternalToByteArray(ByteOrder.Big);
            }

            if (mask) {
                Mask = Mask.On;
                MaskingKey = createMaskingKey();
                payloadData.Mask(MaskingKey);
            } else {
                Mask = Mask.Off;
                MaskingKey = WebSocket.EmptyBytes;
            }

            PayloadData = payloadData;
        }

        internal int ExtendedPayloadLengthCount => PayloadLength >= 126 ? PayloadLength != 126 ? 8 : 2 : 0;

        internal ulong FullPayloadLength => PayloadLength < 126 ? PayloadLength :
                                            PayloadLength != 126 ? ExtendedPayloadLength.ToUInt64(ByteOrder.Big) : ExtendedPayloadLength.ToUInt16(ByteOrder.Big);

        public byte[] ExtendedPayloadLength { get; private set; }

        public Fin Fin { get; private set; }

        public bool IsBinary => Opcode == Opcode.Binary;

        public bool IsClose => Opcode == Opcode.Close;

        public bool IsCompressed => Rsv1 == Rsv.On;

        public bool IsContinuation => Opcode == Opcode.Cont;

        public bool IsControl => (int)Opcode >= 8;

        public bool IsData => Opcode == Opcode.Text || Opcode == Opcode.Binary;

        public bool IsFinal => Fin == Fin.Final;

        public bool IsFragment => Fin == Fin.More || Opcode == Opcode.Cont;

        public bool IsMasked => Mask == Mask.On;

        public bool IsPing => Opcode == Opcode.Ping;

        public bool IsPong => Opcode == Opcode.Pong;

        public bool IsText => Opcode == Opcode.Text;

        public ulong Length => (ulong)(2L + (ExtendedPayloadLength.Length + MaskingKey.Length)) + PayloadData.Length;

        public Mask Mask { get; private set; }

        public byte[] MaskingKey { get; private set; }

        public Opcode Opcode { get; private set; }

        public PayloadData PayloadData { get; private set; }

        public byte PayloadLength { get; private set; }

        public Rsv Rsv1 { get; private set; }

        public Rsv Rsv2 { get; private set; }

        public Rsv Rsv3 { get; private set; }

        public IEnumerator<byte> GetEnumerator() {
            byte[] array = ToArray();

            for (int i = 0; i < array.Length; i++) {
                yield return array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        static byte[] createMaskingKey() {
            byte[] array = new byte[4];
            WebSocket.RandomNumber.GetBytes(array);
            return array;
        }

        static string dump(WebSocketFrame frame) {
            ulong length = frame.Length;
            long num = (long)(length / 4uL);
            int num2 = (int)(length % 4uL);
            int num3;
            string arg5;

            if (num < 10000) {
                num3 = 4;
                arg5 = "{0,4}";
            } else if (num < 65536) {
                num3 = 4;
                arg5 = "{0,4:X}";
            } else if (num < 4294967296L) {
                num3 = 8;
                arg5 = "{0,8:X}";
            } else {
                num3 = 16;
                arg5 = "{0,16:X}";
            }

            string arg6 = string.Format("{{0,{0}}}", num3);
            string format = string.Format("\n{0} 01234567 89ABCDEF 01234567 89ABCDEF\n{0}+--------+--------+--------+--------+\\n", arg6);
            string lineFmt = string.Format("{0}|{{1,8}} {{2,8}} {{3,8}} {{4,8}}|\n", arg5);
            string format2 = string.Format("{0}+--------+--------+--------+--------+", arg6);
            StringBuilder output = new(64);

            Func<Action<string, string, string, string>> func = delegate {
                long lineCnt = 0L;

                return delegate(string arg1, string arg2, string arg3, string arg4) {
                    output.AppendFormat(lineFmt, ++lineCnt, arg1, arg2, arg3, arg4);
                };
            };

            Action<string, string, string, string> action = func();
            output.AppendFormat(format, string.Empty);
            byte[] array = frame.ToArray();

            for (long num4 = 0L; num4 <= num; num4++) {
                long num5 = num4 * 4;

                if (num4 < num) {
                    action(Convert.ToString(array[num5], 2).PadLeft(8, '0'), Convert.ToString(array[num5 + 1], 2).PadLeft(8, '0'), Convert.ToString(array[num5 + 2], 2).PadLeft(8, '0'),
                        Convert.ToString(array[num5 + 3], 2).PadLeft(8, '0'));
                } else if (num2 > 0) {
                    action(Convert.ToString(array[num5], 2).PadLeft(8, '0'), num2 < 2 ? string.Empty : Convert.ToString(array[num5 + 1], 2).PadLeft(8, '0'),
                        num2 != 3 ? string.Empty : Convert.ToString(array[num5 + 2], 2).PadLeft(8, '0'), string.Empty);
                }
            }

            output.AppendFormat(format2, string.Empty);
            return output.ToString();
        }

        static string print(WebSocketFrame frame) {
            byte payloadLength = frame.PayloadLength;
            string text = payloadLength <= 125 ? string.Empty : frame.FullPayloadLength.ToString();
            string text2 = BitConverter.ToString(frame.MaskingKey);

            string text3 = payloadLength == 0 ? string.Empty :
                           payloadLength > 125 ? "---" :
                           !frame.IsText || frame.IsFragment || frame.IsMasked || frame.IsCompressed ? frame.PayloadData.ToString() : frame.PayloadData.ApplicationData.UTF8Decode();

            string format =
                "\n                    FIN: {0}\n                   RSV1: {1}\n                   RSV2: {2}\n                   RSV3: {3}\n                 Opcode: {4}\n                   MASK: {5}\n         Payload Length: {6}\nExtended Payload Length: {7}\n            Masking Key: {8}\n           Payload Data: {9}";

            return string.Format(format, frame.Fin, frame.Rsv1, frame.Rsv2, frame.Rsv3, frame.Opcode, frame.Mask, payloadLength, text, text2, text3);
        }

        static WebSocketFrame processHeader(byte[] header) {
            if (header.Length != 2) {
                throw new WebSocketException("The header of a frame cannot be read from the stream.");
            }

            Fin fin = (header[0] & 0x80) == 128 ? Fin.Final : Fin.More;
            Rsv rsv = (header[0] & 0x40) == 64 ? Rsv.On : Rsv.Off;
            Rsv rsv2 = (header[0] & 0x20) == 32 ? Rsv.On : Rsv.Off;
            Rsv rsv3 = (header[0] & 0x10) == 16 ? Rsv.On : Rsv.Off;
            byte opcode = (byte)(header[0] & 0xFu);
            Mask mask = (header[1] & 0x80) == 128 ? Mask.On : Mask.Off;
            byte b = (byte)(header[1] & 0x7Fu);

            string text = !opcode.IsSupported() ? "An unsupported opcode." :
                          !opcode.IsData() && rsv == Rsv.On ? "A non data frame is compressed." :
                          opcode.IsControl() && fin == Fin.More ? "A control frame is fragmented." :
                          !opcode.IsControl() || b <= 125 ? null : "A control frame has a long payload length.";

            if (text != null) {
                throw new WebSocketException(CloseStatusCode.ProtocolError, text);
            }

            WebSocketFrame webSocketFrame = new();
            webSocketFrame.Fin = fin;
            webSocketFrame.Rsv1 = rsv;
            webSocketFrame.Rsv2 = rsv2;
            webSocketFrame.Rsv3 = rsv3;
            webSocketFrame.Opcode = (Opcode)opcode;
            webSocketFrame.Mask = mask;
            webSocketFrame.PayloadLength = b;
            return webSocketFrame;
        }

        static WebSocketFrame readExtendedPayloadLength(Stream stream, WebSocketFrame frame) {
            int extendedPayloadLengthCount = frame.ExtendedPayloadLengthCount;

            if (extendedPayloadLengthCount == 0) {
                frame.ExtendedPayloadLength = WebSocket.EmptyBytes;
                return frame;
            }

            byte[] array = stream.ReadBytes(extendedPayloadLengthCount);

            if (array.Length != extendedPayloadLengthCount) {
                throw new WebSocketException("The extended payload length of a frame cannot be read from the stream.");
            }

            frame.ExtendedPayloadLength = array;
            return frame;
        }

        static void readExtendedPayloadLengthAsync(Stream stream, WebSocketFrame frame, Action<WebSocketFrame> completed, Action<Exception> error) {
            int len = frame.ExtendedPayloadLengthCount;

            if (len == 0) {
                frame.ExtendedPayloadLength = WebSocket.EmptyBytes;
                completed(frame);
                return;
            }

            stream.ReadBytesAsync(len, delegate(byte[] bytes) {
                if (bytes.Length != len) {
                    throw new WebSocketException("The extended payload length of a frame cannot be read from the stream.");
                }

                frame.ExtendedPayloadLength = bytes;
                completed(frame);
            }, error);
        }

        static WebSocketFrame readHeader(Stream stream) => processHeader(stream.ReadBytes(2));

        static void readHeaderAsync(Stream stream, Action<WebSocketFrame> completed, Action<Exception> error) {
            stream.ReadBytesAsync(2, delegate(byte[] bytes) {
                completed(processHeader(bytes));
            }, error);
        }

        static WebSocketFrame readMaskingKey(Stream stream, WebSocketFrame frame) {
            int num = frame.IsMasked ? 4 : 0;

            if (num == 0) {
                frame.MaskingKey = WebSocket.EmptyBytes;
                return frame;
            }

            byte[] array = stream.ReadBytes(num);

            if (array.Length != num) {
                throw new WebSocketException("The masking key of a frame cannot be read from the stream.");
            }

            frame.MaskingKey = array;
            return frame;
        }

        static void readMaskingKeyAsync(Stream stream, WebSocketFrame frame, Action<WebSocketFrame> completed, Action<Exception> error) {
            int len = frame.IsMasked ? 4 : 0;

            if (len == 0) {
                frame.MaskingKey = WebSocket.EmptyBytes;
                completed(frame);
                return;
            }

            stream.ReadBytesAsync(len, delegate(byte[] bytes) {
                if (bytes.Length != len) {
                    throw new WebSocketException("The masking key of a frame cannot be read from the stream.");
                }

                frame.MaskingKey = bytes;
                completed(frame);
            }, error);
        }

        static WebSocketFrame readPayloadData(Stream stream, WebSocketFrame frame) {
            ulong fullPayloadLength = frame.FullPayloadLength;

            if (fullPayloadLength == 0) {
                frame.PayloadData = PayloadData.Empty;
                return frame;
            }

            if (fullPayloadLength > PayloadData.MaxLength) {
                throw new WebSocketException(CloseStatusCode.TooBig, "A frame has a long payload length.");
            }

            long num = (long)fullPayloadLength;
            byte[] array = frame.PayloadLength >= 127 ? stream.ReadBytes(num, 1024) : stream.ReadBytes((int)fullPayloadLength);

            if (array.LongLength != num) {
                throw new WebSocketException("The payload data of a frame cannot be read from the stream.");
            }

            frame.PayloadData = new PayloadData(array, num);
            return frame;
        }

        static void readPayloadDataAsync(Stream stream, WebSocketFrame frame, Action<WebSocketFrame> completed, Action<Exception> error) {
            ulong fullPayloadLength = frame.FullPayloadLength;

            if (fullPayloadLength == 0) {
                frame.PayloadData = PayloadData.Empty;
                completed(frame);
                return;
            }

            if (fullPayloadLength > PayloadData.MaxLength) {
                throw new WebSocketException(CloseStatusCode.TooBig, "A frame has a long payload length.");
            }

            long llen = (long)fullPayloadLength;

            Action<byte[]> completed2 = delegate(byte[] bytes) {
                if (bytes.LongLength != llen) {
                    throw new WebSocketException("The payload data of a frame cannot be read from the stream.");
                }

                frame.PayloadData = new PayloadData(bytes, llen);
                completed(frame);
            };

            if (frame.PayloadLength < 127) {
                stream.ReadBytesAsync((int)fullPayloadLength, completed2, error);
            } else {
                stream.ReadBytesAsync(llen, 1024, completed2, error);
            }
        }

        internal static WebSocketFrame CreateCloseFrame(PayloadData payloadData, bool mask) => new(Fin.Final, Opcode.Close, payloadData, false, mask);

        internal static WebSocketFrame CreatePingFrame(bool mask) => new(Fin.Final, Opcode.Ping, PayloadData.Empty, false, mask);

        internal static WebSocketFrame CreatePingFrame(byte[] data, bool mask) => new(Fin.Final, Opcode.Ping, new PayloadData(data), false, mask);

        internal static WebSocketFrame ReadFrame(Stream stream, bool unmask) {
            WebSocketFrame webSocketFrame = readHeader(stream);
            readExtendedPayloadLength(stream, webSocketFrame);
            readMaskingKey(stream, webSocketFrame);
            readPayloadData(stream, webSocketFrame);

            if (unmask) {
                webSocketFrame.Unmask();
            }

            return webSocketFrame;
        }

        internal static void ReadFrameAsync(Stream stream, bool unmask, Action<WebSocketFrame> completed, Action<Exception> error) {
            readHeaderAsync(stream, delegate(WebSocketFrame frame) {
                readExtendedPayloadLengthAsync(stream, frame, delegate(WebSocketFrame frame1) {
                    readMaskingKeyAsync(stream, frame1, delegate(WebSocketFrame frame2) {
                        readPayloadDataAsync(stream, frame2, delegate(WebSocketFrame frame3) {
                            if (unmask) {
                                frame3.Unmask();
                            }

                            completed(frame3);
                        }, error);
                    }, error);
                }, error);
            }, error);
        }

        internal void Unmask() {
            if (Mask != 0) {
                Mask = Mask.Off;
                PayloadData.Mask(MaskingKey);
                MaskingKey = WebSocket.EmptyBytes;
            }
        }

        public void Print(bool dumped) {
            Console.WriteLine(!dumped ? print(this) : dump(this));
        }

        public string PrintToString(bool dumped) => !dumped ? print(this) : dump(this);

        public byte[] ToArray() {
            using (MemoryStream memoryStream = new()) {
                int fin = (int)Fin;
                fin = (fin << 1) + (int)Rsv1;
                fin = (fin << 1) + (int)Rsv2;
                fin = (fin << 1) + (int)Rsv3;
                fin = (fin << 4) + (int)Opcode;
                fin = (fin << 1) + (int)Mask;
                fin = (fin << 7) + PayloadLength;
                memoryStream.Write(((ushort)fin).InternalToByteArray(ByteOrder.Big), 0, 2);

                if (PayloadLength > 125) {
                    memoryStream.Write(ExtendedPayloadLength, 0, PayloadLength != 126 ? 8 : 2);
                }

                if (Mask == Mask.On) {
                    memoryStream.Write(MaskingKey, 0, 4);
                }

                if (PayloadLength > 0) {
                    byte[] array = PayloadData.ToArray();

                    if (PayloadLength < 127) {
                        memoryStream.Write(array, 0, array.Length);
                    } else {
                        memoryStream.WriteBytes(array, 1024);
                    }
                }

                memoryStream.Close();
                return memoryStream.ToArray();
            }
        }

        public override string ToString() => BitConverter.ToString(ToArray());
    }
}