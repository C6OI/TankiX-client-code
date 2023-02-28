using System;
using System.Collections;
using System.Collections.Generic;

namespace WebSocketSharp {
    internal class PayloadData : IEnumerable<byte>, IEnumerable {
        public static readonly PayloadData Empty;

        public static readonly ulong MaxLength;
        readonly byte[] _data;

        readonly long _length;

        static PayloadData() {
            Empty = new PayloadData();
            MaxLength = 9223372036854775807uL;
        }

        internal PayloadData() => _data = WebSocket.EmptyBytes;

        internal PayloadData(byte[] data)
            : this(data, data.LongLength) { }

        internal PayloadData(byte[] data, long length) {
            _data = data;
            _length = length;
        }

        internal long ExtensionDataLength { get; set; }

        internal bool IncludesReservedCloseStatusCode => _length > 1 && _data.SubArray(0, 2).ToUInt16(ByteOrder.Big).IsReserved();

        public byte[] ApplicationData => ExtensionDataLength <= 0 ? _data : _data.SubArray(ExtensionDataLength, _length - ExtensionDataLength);

        public byte[] ExtensionData => ExtensionDataLength <= 0 ? WebSocket.EmptyBytes : _data.SubArray(0L, ExtensionDataLength);

        public ulong Length => (ulong)_length;

        public IEnumerator<byte> GetEnumerator() {
            byte[] data = _data;

            for (int i = 0; i < data.Length; i++) {
                yield return data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal void Mask(byte[] key) {
            for (long num = 0L; num < _length; num++) {
                _data[num] = (byte)(_data[num] ^ key[num % 4]);
            }
        }

        public byte[] ToArray() => _data;

        public override string ToString() => BitConverter.ToString(_data);
    }
}