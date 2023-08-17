using System;
using System.IO;

namespace log4net.Util.PatternStringConverters {
    class LiteralPatternConverter : PatternConverter {
        public override PatternConverter SetNext(PatternConverter pc) {
            LiteralPatternConverter literalPatternConverter = pc as LiteralPatternConverter;

            if (literalPatternConverter != null) {
                Option += literalPatternConverter.Option;
                return this;
            }

            return base.SetNext(pc);
        }

        public override void Format(TextWriter writer, object state) => writer.Write(Option);

        protected override void Convert(TextWriter writer, object state) =>
            throw new InvalidOperationException("Should never get here because of the overridden Format method");
    }
}