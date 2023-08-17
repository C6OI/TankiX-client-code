using System;
using System.IO;
using System.Text;
using System.Threading;
using log4net.Core;
using log4net.Layout;
using log4net.Util;

namespace log4net.Appender {
    public class FileAppender : TextWriterAppender {
        static readonly Type declaringType = typeof(FileAppender);

        string m_fileName;

        LockingStream m_stream;

        public FileAppender() { }

        [Obsolete("Instead use the default constructor and set the Layout, File & AppendToFile properties")]
        public FileAppender(ILayout layout, string filename, bool append) {
            Layout = layout;
            File = filename;
            AppendToFile = append;
            ActivateOptions();
        }

        [Obsolete("Instead use the default constructor and set the Layout & File properties")]
        public FileAppender(ILayout layout, string filename)
            : this(layout, filename, true) { }

        public virtual string File {
            get => m_fileName;
            set => m_fileName = value;
        }

        public bool AppendToFile { get; set; } = true;

        public Encoding Encoding { get; set; } = Encoding.Default;

        public SecurityContext SecurityContext { get; set; }

        public LockingModelBase LockingModel { get; set; } = new ExclusiveLock();

        public override void ActivateOptions() {
            base.ActivateOptions();

            if (SecurityContext == null) {
                SecurityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }

            if (LockingModel == null) {
                LockingModel = new ExclusiveLock();
            }

            LockingModel.CurrentAppender = this;

            if (m_fileName != null) {
                using (SecurityContext.Impersonate(this)) {
                    m_fileName = ConvertToFullPath(m_fileName.Trim());
                }

                SafeOpenFile(m_fileName, AppendToFile);
            } else {
                LogLog.Warn(declaringType, "FileAppender: File option not set for appender [" + Name + "].");
                LogLog.Warn(declaringType, "FileAppender: Are you using FileAppender instead of ConsoleAppender?");
            }
        }

        protected override void Reset() {
            base.Reset();
            m_fileName = null;
        }

        protected override void PrepareWriter() => SafeOpenFile(m_fileName, AppendToFile);

        protected override void Append(LoggingEvent loggingEvent) {
            if (m_stream.AcquireLock()) {
                try {
                    base.Append(loggingEvent);
                } finally {
                    m_stream.ReleaseLock();
                }
            }
        }

        protected override void Append(LoggingEvent[] loggingEvents) {
            if (m_stream.AcquireLock()) {
                try {
                    base.Append(loggingEvents);
                } finally {
                    m_stream.ReleaseLock();
                }
            }
        }

        protected override void WriteFooter() {
            if (m_stream != null) {
                m_stream.AcquireLock();

                try {
                    base.WriteFooter();
                } finally {
                    m_stream.ReleaseLock();
                }
            }
        }

        protected override void WriteHeader() {
            if (m_stream != null && m_stream.AcquireLock()) {
                try {
                    base.WriteHeader();
                } finally {
                    m_stream.ReleaseLock();
                }
            }
        }

        protected override void CloseWriter() {
            if (m_stream != null) {
                m_stream.AcquireLock();

                try {
                    base.CloseWriter();
                } finally {
                    m_stream.ReleaseLock();
                }
            }
        }

        protected void CloseFile() => WriteFooterAndCloseWriter();

        protected virtual void SafeOpenFile(string fileName, bool append) {
            try {
                OpenFile(fileName, append);
            } catch (Exception e) {
                ErrorHandler.Error("OpenFile(" + fileName + "," + append + ") call failed.", e, ErrorCode.FileOpenFailure);
            }
        }

        protected virtual void OpenFile(string fileName, bool append) {
            if (LogLog.IsErrorEnabled) {
                bool flag = false;

                using (SecurityContext.Impersonate(this)) {
                    flag = Path.IsPathRooted(fileName);
                }

                if (!flag) {
                    LogLog.Error(declaringType,
                        "INTERNAL ERROR. OpenFile(" + fileName + "): File name is not fully qualified.");
                }
            }

            lock (this) {
                Reset();
                LogLog.Debug(declaringType, "Opening file for writing [" + fileName + "] append [" + append + "]");
                m_fileName = fileName;
                AppendToFile = append;
                LockingModel.CurrentAppender = this;
                LockingModel.OpenFile(fileName, append, Encoding);
                m_stream = new LockingStream(LockingModel);

                if (m_stream != null) {
                    m_stream.AcquireLock();

                    try {
                        SetQWForFiles(new StreamWriter(m_stream, Encoding));
                    } finally {
                        m_stream.ReleaseLock();
                    }
                }

                WriteHeader();
            }
        }

        protected virtual void SetQWForFiles(Stream fileStream) => SetQWForFiles(new StreamWriter(fileStream, Encoding));

        protected virtual void SetQWForFiles(TextWriter writer) => QuietWriter = new QuietTextWriter(writer, ErrorHandler);

        protected static string ConvertToFullPath(string path) => SystemInfo.ConvertToFullPath(path);

        sealed class LockingStream : Stream, IDisposable {
            readonly LockingModelBase m_lockingModel;

            int m_lockLevel;

            int m_readTotal = -1;

            Stream m_realStream;

            public LockingStream(LockingModelBase locking) {
                if (locking == null) {
                    throw new ArgumentException("Locking model may not be null", "locking");
                }

                m_lockingModel = locking;
            }

            public override bool CanRead => false;

            public override bool CanSeek {
                get {
                    AssertLocked();
                    return m_realStream.CanSeek;
                }
            }

            public override bool CanWrite {
                get {
                    AssertLocked();
                    return m_realStream.CanWrite;
                }
            }

            public override long Length {
                get {
                    AssertLocked();
                    return m_realStream.Length;
                }
            }

            public override long Position {
                get {
                    AssertLocked();
                    return m_realStream.Position;
                }
                set {
                    AssertLocked();
                    m_realStream.Position = value;
                }
            }

            void IDisposable.Dispose() => Close();

            public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback,
                object state) {
                AssertLocked();
                IAsyncResult asyncResult = m_realStream.BeginRead(buffer, offset, count, callback, state);
                m_readTotal = EndRead(asyncResult);
                return asyncResult;
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback,
                object state) {
                AssertLocked();
                IAsyncResult asyncResult = m_realStream.BeginWrite(buffer, offset, count, callback, state);
                EndWrite(asyncResult);
                return asyncResult;
            }

            public override void Close() => m_lockingModel.CloseFile();

            public override int EndRead(IAsyncResult asyncResult) {
                AssertLocked();
                return m_readTotal;
            }

            public override void EndWrite(IAsyncResult asyncResult) { }

            public override void Flush() {
                AssertLocked();
                m_realStream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count) => m_realStream.Read(buffer, offset, count);

            public override int ReadByte() => m_realStream.ReadByte();

            public override long Seek(long offset, SeekOrigin origin) {
                AssertLocked();
                return m_realStream.Seek(offset, origin);
            }

            public override void SetLength(long value) {
                AssertLocked();
                m_realStream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count) {
                AssertLocked();
                m_realStream.Write(buffer, offset, count);
            }

            public override void WriteByte(byte value) {
                AssertLocked();
                m_realStream.WriteByte(value);
            }

            void AssertLocked() {
                if (m_realStream == null) {
                    throw new LockStateException("The file is not currently locked");
                }
            }

            public bool AcquireLock() {
                bool result = false;

                lock (this) {
                    if (m_lockLevel == 0) {
                        m_realStream = m_lockingModel.AcquireLock();
                    }

                    if (m_realStream != null) {
                        m_lockLevel++;
                        result = true;
                    }
                }

                return result;
            }

            public void ReleaseLock() {
                lock (this) {
                    m_lockLevel--;

                    if (m_lockLevel == 0) {
                        m_lockingModel.ReleaseLock();
                        m_realStream = null;
                    }
                }
            }

            public sealed class LockStateException : LogException {
                public LockStateException(string message)
                    : base(message) { }
            }
        }

        public abstract class LockingModelBase {
            public FileAppender CurrentAppender { get; set; }

            public abstract void OpenFile(string filename, bool append, Encoding encoding);

            public abstract void CloseFile();

            public abstract Stream AcquireLock();

            public abstract void ReleaseLock();

            protected Stream CreateStream(string filename, bool append, FileShare fileShare) {
                using (CurrentAppender.SecurityContext.Impersonate(this)) {
                    string directoryName = Path.GetDirectoryName(filename);

                    if (!Directory.Exists(directoryName)) {
                        Directory.CreateDirectory(directoryName);
                    }

                    FileMode mode = !append ? FileMode.Create : FileMode.Append;
                    return new FileStream(filename, mode, FileAccess.Write, fileShare);
                }
            }

            protected void CloseStream(Stream stream) {
                using (CurrentAppender.SecurityContext.Impersonate(this)) {
                    stream.Close();
                }
            }
        }

        public class ExclusiveLock : LockingModelBase {
            Stream m_stream;

            public override void OpenFile(string filename, bool append, Encoding encoding) {
                try {
                    m_stream = CreateStream(filename, append, FileShare.Read);
                } catch (Exception ex) {
                    CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + filename + ". " + ex.Message);
                }
            }

            public override void CloseFile() {
                CloseStream(m_stream);
                m_stream = null;
            }

            public override Stream AcquireLock() => m_stream;

            public override void ReleaseLock() { }
        }

        public class MinimalLock : LockingModelBase {
            bool m_append;
            string m_filename;

            Stream m_stream;

            public override void OpenFile(string filename, bool append, Encoding encoding) {
                m_filename = filename;
                m_append = append;
            }

            public override void CloseFile() { }

            public override Stream AcquireLock() {
                if (m_stream == null) {
                    try {
                        m_stream = CreateStream(m_filename, m_append, FileShare.Read);
                        m_append = true;
                    } catch (Exception ex) {
                        CurrentAppender.ErrorHandler.Error(
                            "Unable to acquire lock on file " + m_filename + ". " + ex.Message);
                    }
                }

                return m_stream;
            }

            public override void ReleaseLock() {
                CloseStream(m_stream);
                m_stream = null;
            }
        }

        public class InterProcessLock : LockingModelBase {
            Mutex m_mutex;

            bool m_mutexClosed;

            Stream m_stream;

            public override void OpenFile(string filename, bool append, Encoding encoding) {
                try {
                    m_stream = CreateStream(filename, append, FileShare.ReadWrite);
                    string name = filename.Replace("\\", "_").Replace(":", "_").Replace("/", "_");
                    m_mutex = new Mutex(false, name);
                } catch (Exception ex) {
                    CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + filename + ". " + ex.Message);
                }
            }

            public override void CloseFile() {
                try {
                    CloseStream(m_stream);
                    m_stream = null;
                } finally {
                    m_mutex.ReleaseMutex();
                    m_mutex.Close();
                    m_mutexClosed = true;
                }
            }

            public override Stream AcquireLock() {
                if (m_mutex != null) {
                    m_mutex.WaitOne();

                    if (m_stream.CanSeek) {
                        m_stream.Seek(0L, SeekOrigin.End);
                    }
                }

                return m_stream;
            }

            public override void ReleaseLock() {
                if (!m_mutexClosed && m_mutex != null) {
                    m_mutex.ReleaseMutex();
                }
            }
        }
    }
}