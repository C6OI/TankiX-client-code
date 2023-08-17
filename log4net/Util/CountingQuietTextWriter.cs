using System;
using System.IO;
using log4net.Core;

namespace log4net.Util {
    public class CountingQuietTextWriter : QuietTextWriter {
        public CountingQuietTextWriter(TextWriter writer, IErrorHandler errorHandler)
            : base(writer, errorHandler) => Count = 0L;

        public long Count { get; set; }

        public override void Write(char value) {
            try {
                base.Write(value);
                Count += Encoding.GetByteCount(new char[1] { value });
            } catch (Exception e) {
                ErrorHandler.Error("Failed to write [" + value + "].", e, ErrorCode.WriteFailure);
            }
        }

        public override void Write(char[] buffer, int index, int count) {
            if (count > 0) {
                try {
                    base.Write(buffer, index, count);
                    Count += Encoding.GetByteCount(buffer, index, count);
                } catch (Exception e) {
                    ErrorHandler.Error("Failed to write buffer.", e, ErrorCode.WriteFailure);
                }
            }
        }

        public override void Write(string str) {
            if (str != null && str.Length > 0) {
                try {
                    base.Write(str);
                    Count += Encoding.GetByteCount(str);
                } catch (Exception e) {
                    ErrorHandler.Error("Failed to write [" + str + "].", e, ErrorCode.WriteFailure);
                }
            }
        }
    }
}