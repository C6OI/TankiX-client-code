using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SharpCompress.Compressor.Deflate {
    class ParallelDeflateOutputStream : Stream {
        static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

        readonly CompressionLevel _compressLevel;

        readonly TraceBits _DesiredTrace =
            TraceBits.Lifecycle | TraceBits.Session | TraceBits.Synch | TraceBits.WriterThread;

        readonly object _eLock = new();

        readonly bool _leaveOpen;

        readonly object _outputLock = new();

        readonly ManualResetEvent _sessionReset;

        readonly ManualResetEvent _writingDone;

        int _bufferSize = IO_BUFFER_SIZE_DEFAULT;

        bool _firstWriteDone;

        bool _isClosed;

        bool _isDisposed;

        int _nextToFill;

        int _nextToWrite;

        bool _noMoreInputForThisSegment;

        Stream _outStream;

        int _pc;

        volatile Exception _pendingException;

        List<WorkItem> _pool;

        public ParallelDeflateOutputStream(Stream stream)
            : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false) { }

        public ParallelDeflateOutputStream(Stream stream, CompressionLevel level)
            : this(stream, level, CompressionStrategy.Default, false) { }

        public ParallelDeflateOutputStream(Stream stream, bool leaveOpen)
            : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen) { }

        public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen)
            : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen) { }

        public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy,
            bool leaveOpen) {
            _compressLevel = level;
            _leaveOpen = leaveOpen;
            Strategy = strategy;
            BuffersPerCore = 4;
            _writingDone = new ManualResetEvent(false);
            _sessionReset = new ManualResetEvent(false);
            _outStream = stream;
        }

        public CompressionStrategy Strategy { get; }

        public int BuffersPerCore { get; set; }

        public int BufferSize {
            get => _bufferSize;
            set {
                if (value < 1024) {
                    throw new ArgumentException();
                }

                _bufferSize = value;
            }
        }

        public int Crc32 { get; private set; }

        public long BytesProcessed { get; private set; }

        public override bool CanSeek => false;

        public override bool CanRead => false;

        public override bool CanWrite => _outStream.CanWrite;

        public override long Length => throw new NotImplementedException();

        public override long Position {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        void _InitializePoolOfWorkItems() {
            _pool = new List<WorkItem>();

            for (int i = 0; i < BuffersPerCore * Environment.ProcessorCount; i++) {
                _pool.Add(new WorkItem(_bufferSize, _compressLevel, Strategy));
            }

            _pc = _pool.Count;

            for (int j = 0; j < _pc; j++) {
                _pool[j].index = j;
            }

            _nextToFill = _nextToWrite = 0;
        }

        void _KickoffWriter() {
            if (!ThreadPool.QueueUserWorkItem(_PerpetualWriterMethod)) {
                throw new Exception("Cannot enqueue writer thread.");
            }
        }

        public override void Write(byte[] buffer, int offset, int count) {
            if (_isClosed) {
                throw new NotSupportedException();
            }

            if (_pendingException != null) {
                throw _pendingException;
            }

            if (count == 0) {
                return;
            }

            if (!_firstWriteDone) {
                _InitializePoolOfWorkItems();
                _KickoffWriter();
                _sessionReset.Set();
                _firstWriteDone = true;
            }

            do {
                int index = _nextToFill % _pc;
                WorkItem workItem = _pool[index];

                lock (workItem) {
                    if (workItem.status == 0 || workItem.status == 6 || workItem.status == 1) {
                        workItem.status = 1;

                        int num = workItem.buffer.Length - workItem.inputBytesAvailable <= count
                                      ? workItem.buffer.Length - workItem.inputBytesAvailable : count;

                        Array.Copy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num);
                        count -= num;
                        offset += num;
                        workItem.inputBytesAvailable += num;

                        if (workItem.inputBytesAvailable == workItem.buffer.Length) {
                            workItem.status = 2;
                            _nextToFill++;

                            if (!ThreadPool.QueueUserWorkItem(_DeflateOne, workItem)) {
                                throw new Exception("Cannot enqueue workitem");
                            }
                        }

                        continue;
                    }

                    int num2 = 0;

                    while (workItem.status != 0 && workItem.status != 6 && workItem.status != 1) {
                        num2++;
                        Monitor.Pulse(workItem);
                        Monitor.Wait(workItem);

                        if (workItem.status != 0 && workItem.status != 6 && workItem.status != 1) { }
                    }
                }
            } while (count > 0);
        }

        public override void Flush() => _Flush(false);

        void _Flush(bool lastInput) {
            if (_isClosed) {
                throw new NotSupportedException();
            }

            WorkItem workItem = _pool[_nextToFill % _pc];

            lock (workItem) {
                if (workItem.status == 1) {
                    workItem.status = 2;
                    _nextToFill++;

                    if (lastInput) {
                        _noMoreInputForThisSegment = true;
                    }

                    if (!ThreadPool.QueueUserWorkItem(_DeflateOne, workItem)) {
                        throw new Exception("Cannot enqueue workitem");
                    }
                } else if (lastInput) {
                    _noMoreInputForThisSegment = true;
                }
            }
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (!disposing) {
                return;
            }

            _isDisposed = true;
            _pool = null;
            _sessionReset.Set();
            _writingDone.Close();
            _sessionReset.Close();

            if (!_isClosed) {
                _Flush(true);
                WorkItem workItem = _pool[_nextToFill % _pc];

                lock (workItem) {
                    Monitor.PulseAll(workItem);
                }

                _writingDone.WaitOne();

                if (!_leaveOpen) {
                    _outStream.Close();
                }

                _isClosed = true;
            }
        }

        public void Reset(Stream stream) {
            if (!_firstWriteDone) {
                return;
            }

            if (_noMoreInputForThisSegment) {
                _writingDone.WaitOne();

                foreach (WorkItem item in _pool) {
                    item.status = 0;
                }

                _noMoreInputForThisSegment = false;
                _nextToFill = _nextToWrite = 0;
                BytesProcessed = 0L;
                Crc32 = 0;
                _isClosed = false;
                _writingDone.Reset();
            }

            _outStream = stream;
            _sessionReset.Set();
        }

        void _PerpetualWriterMethod(object state) {
            try {
                ZlibCodec zlibCodec;

                while (true) {
                    _sessionReset.WaitOne();

                    if (_isDisposed) {
                        return;
                    }

                    _sessionReset.Reset();
                    WorkItem workItem = null;
                    CRC32 cRC = new();

                    do {
                        workItem = _pool[_nextToWrite % _pc];

                        lock (workItem) {
                            if (_noMoreInputForThisSegment) { }

                            do {
                                if (workItem.status == 4) {
                                    workItem.status = 5;
                                    _outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
                                    cRC.Combine(workItem.crc, workItem.inputBytesAvailable);
                                    BytesProcessed += workItem.inputBytesAvailable;
                                    _nextToWrite++;
                                    workItem.inputBytesAvailable = 0;
                                    workItem.status = 6;
                                    Monitor.Pulse(workItem);
                                    break;
                                }

                                int num = 0;

                                while (workItem.status != 4 &&
                                       (!_noMoreInputForThisSegment || _nextToWrite != _nextToFill)) {
                                    num++;
                                    Monitor.Pulse(workItem);
                                    Monitor.Wait(workItem);

                                    if (workItem.status != 4) { }
                                }
                            } while (!_noMoreInputForThisSegment || _nextToWrite != _nextToFill);
                        }

                        if (_noMoreInputForThisSegment) { }
                    } while (!_noMoreInputForThisSegment || _nextToWrite != _nextToFill);

                    byte[] array = new byte[128];
                    zlibCodec = new ZlibCodec();
                    int num2 = zlibCodec.InitializeDeflate(_compressLevel, false);
                    zlibCodec.InputBuffer = null;
                    zlibCodec.NextIn = 0;
                    zlibCodec.AvailableBytesIn = 0;
                    zlibCodec.OutputBuffer = array;
                    zlibCodec.NextOut = 0;
                    zlibCodec.AvailableBytesOut = array.Length;
                    num2 = zlibCodec.Deflate(FlushType.Finish);

                    if (num2 != 1 && num2 != 0) {
                        break;
                    }

                    if (array.Length - zlibCodec.AvailableBytesOut > 0) {
                        _outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
                    }

                    zlibCodec.EndDeflate();
                    Crc32 = cRC.Crc32Result;
                    _writingDone.Set();
                }

                throw new Exception("deflating: " + zlibCodec.Message);
            } catch (Exception pendingException) {
                lock (_eLock) {
                    if (_pendingException != null) {
                        _pendingException = pendingException;
                    }
                }
            }
        }

        void _DeflateOne(object wi) {
            WorkItem workItem = (WorkItem)wi;

            try {
                lock (workItem) {
                    if (workItem.status != 2) {
                        throw new InvalidOperationException();
                    }

                    CRC32 cRC = new();
                    cRC.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
                    DeflateOneSegment(workItem);
                    workItem.status = 4;
                    workItem.crc = cRC.Crc32Result;
                    Monitor.Pulse(workItem);
                }
            } catch (Exception pendingException) {
                lock (_eLock) {
                    if (_pendingException != null) {
                        _pendingException = pendingException;
                    }
                }
            }
        }

        bool DeflateOneSegment(WorkItem workitem) {
            ZlibCodec compressor = workitem.compressor;
            compressor.ResetDeflate();
            compressor.NextIn = 0;
            compressor.AvailableBytesIn = workitem.inputBytesAvailable;
            compressor.NextOut = 0;
            compressor.AvailableBytesOut = workitem.compressed.Length;

            do {
                compressor.Deflate(FlushType.None);
            } while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);

            compressor.Deflate(FlushType.Sync);
            workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
            return true;
        }

        [Conditional("Trace")]
        void TraceOutput(TraceBits bits, string format, params object[] varParams) {
            if ((bits & _DesiredTrace) != 0) {
                lock (_outputLock) {
                    int hashCode = Thread.CurrentThread.GetHashCode();
                    Console.Write("{0:000} PDOS ", hashCode);
                    Console.WriteLine(format, varParams);
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) => throw new NotImplementedException();

        [Flags]
        enum TraceBits {
            None = 0,
            Write = 1,
            WriteBegin = 2,
            WriteDone = 4,
            WriteWait = 8,
            Flush = 0x10,
            Compress = 0x20,
            Fill = 0x40,
            Lifecycle = 0x80,
            Session = 0x100,
            Synch = 0x200,
            WriterThread = 0x400
        }
    }
}