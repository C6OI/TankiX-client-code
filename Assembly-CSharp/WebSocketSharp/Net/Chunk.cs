using System;

namespace WebSocketSharp.Net {
    internal class Chunk {
        readonly byte[] _data;

        int _offset;

        public Chunk(byte[] data) => _data = data;

        public int ReadLeft => _data.Length - _offset;

        public int Read(byte[] buffer, int offset, int count) {
            int num = _data.Length - _offset;

            if (num == 0) {
                return num;
            }

            if (count > num) {
                count = num;
            }

            Buffer.BlockCopy(_data, _offset, buffer, offset, count);
            _offset += count;
            return count;
        }
    }
}