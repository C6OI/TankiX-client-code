using System;
using System.IO;

namespace SharpCompress.Compressor.LZMA.LZ {
    class OutWindow {
        byte[] _buffer;

        int _pendingDist;

        int _pendingLen;

        int _pos;

        Stream _stream;

        int _streamPos;

        int _windowSize;

        public long Limit;

        public long Total;

        public bool HasSpace => _pos < _windowSize && Total < Limit;

        public bool HasPending => _pendingLen > 0;

        public int AvailableBytes => _pos - _streamPos;

        public void Create(int windowSize) {
            if (_windowSize != windowSize) {
                _buffer = new byte[windowSize];
            } else {
                _buffer[windowSize - 1] = 0;
            }

            _windowSize = windowSize;
            _pos = 0;
            _streamPos = 0;
            _pendingLen = 0;
            Total = 0L;
            Limit = 0L;
        }

        public void Reset() => Create(_windowSize);

        public void Init(Stream stream) {
            ReleaseStream();
            _stream = stream;
        }

        public void Train(Stream stream) {
            long length = stream.Length;
            int num = (int)(length >= _windowSize ? _windowSize : length);
            stream.Position = length - num;
            Total = 0L;
            Limit = num;
            _pos = _windowSize - num;
            CopyStream(stream, num);

            if (_pos == _windowSize) {
                _pos = 0;
            }

            _streamPos = _pos;
        }

        public void ReleaseStream() {
            Flush();
            _stream = null;
        }

        public void Flush() {
            if (_stream == null) {
                return;
            }

            int num = _pos - _streamPos;

            if (num != 0) {
                _stream.Write(_buffer, _streamPos, num);

                if (_pos >= _windowSize) {
                    _pos = 0;
                }

                _streamPos = _pos;
            }
        }

        public void CopyBlock(int distance, int len) {
            int num = len;
            int num2 = _pos - distance - 1;

            if (num2 < 0) {
                num2 += _windowSize;
            }

            while (num > 0 && _pos < _windowSize && Total < Limit) {
                if (num2 >= _windowSize) {
                    num2 = 0;
                }

                _buffer[_pos++] = _buffer[num2++];
                Total++;

                if (_pos >= _windowSize) {
                    Flush();
                }

                num--;
            }

            _pendingLen = num;
            _pendingDist = distance;
        }

        public void PutByte(byte b) {
            _buffer[_pos++] = b;
            Total++;

            if (_pos >= _windowSize) {
                Flush();
            }
        }

        public byte GetByte(int distance) {
            int num = _pos - distance - 1;

            if (num < 0) {
                num += _windowSize;
            }

            return _buffer[num];
        }

        public int CopyStream(Stream stream, int len) {
            int num = len;

            while (num > 0 && _pos < _windowSize && Total < Limit) {
                int num2 = _windowSize - _pos;

                if (num2 > Limit - Total) {
                    num2 = (int)(Limit - Total);
                }

                if (num2 > num) {
                    num2 = num;
                }

                int num3 = stream.Read(_buffer, _pos, num2);

                if (num3 == 0) {
                    throw new DataErrorException();
                }

                num -= num3;
                _pos += num3;
                Total += num3;

                if (_pos >= _windowSize) {
                    Flush();
                }
            }

            return len - num;
        }

        public void SetLimit(long size) => Limit = Total + size;

        public int Read(byte[] buffer, int offset, int count) {
            if (_streamPos >= _pos) {
                return 0;
            }

            int num = _pos - _streamPos;

            if (num > count) {
                num = count;
            }

            Buffer.BlockCopy(_buffer, _streamPos, buffer, offset, num);
            _streamPos += num;

            if (_streamPos >= _windowSize) {
                _pos = 0;
                _streamPos = 0;
            }

            return num;
        }

        public void CopyPending() {
            if (_pendingLen > 0) {
                CopyBlock(_pendingDist, _pendingLen);
            }
        }
    }
}