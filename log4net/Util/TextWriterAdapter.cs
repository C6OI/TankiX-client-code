using System;
using System.IO;
using System.Text;

namespace log4net.Util {
    public abstract class TextWriterAdapter : TextWriter {
        protected TextWriterAdapter(TextWriter writer) => Writer = writer;

        protected TextWriter Writer { get; set; }

        public override Encoding Encoding => Writer.Encoding;

        public override IFormatProvider FormatProvider => Writer.FormatProvider;

        public override string NewLine {
            get => Writer.NewLine;
            set => Writer.NewLine = value;
        }

        public override void Close() => Writer.Close();

        protected override void Dispose(bool disposing) {
            if (disposing) {
                ((IDisposable)Writer).Dispose();
            }
        }

        public override void Flush() => Writer.Flush();

        public override void Write(char value) => Writer.Write(value);

        public override void Write(char[] buffer, int index, int count) => Writer.Write(buffer, index, count);

        public override void Write(string value) => Writer.Write(value);
    }
}