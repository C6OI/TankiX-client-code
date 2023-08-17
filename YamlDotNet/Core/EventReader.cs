using System.Globalization;
using System.IO;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core {
    public class EventReader {
        bool endOfStream;

        public EventReader(IParser parser) {
            Parser = parser;
            MoveNext();
        }

        public IParser Parser { get; }

        public T Expect<T>() where T : ParsingEvent {
            T val = Allow<T>();

            if (val == null) {
                ParsingEvent current = Parser.Current;

                throw new YamlException(current.Start,
                    current.End,
                    string.Format(CultureInfo.InvariantCulture,
                        "Expected '{0}', got '{1}' (at {2}).",
                        typeof(T).Name,
                        current.GetType().Name,
                        current.Start));
            }

            return val;
        }

        public bool Accept<T>() where T : ParsingEvent {
            ThrowIfAtEndOfStream();
            return Parser.Current is T;
        }

        void ThrowIfAtEndOfStream() {
            if (endOfStream) {
                throw new EndOfStreamException();
            }
        }

        public T Allow<T>() where T : ParsingEvent {
            if (!Accept<T>()) {
                return null;
            }

            T result = (T)Parser.Current;
            MoveNext();
            return result;
        }

        public T Peek<T>() where T : ParsingEvent {
            if (!Accept<T>()) {
                return null;
            }

            return (T)Parser.Current;
        }

        public void SkipThisAndNestedEvents() {
            int num = 0;

            do {
                num += Peek<ParsingEvent>().NestingIncrease;
                MoveNext();
            } while (num > 0);
        }

        void MoveNext() => endOfStream = !Parser.MoveNext();
    }
}