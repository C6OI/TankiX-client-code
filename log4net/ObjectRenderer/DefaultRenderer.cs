using System;
using System.Collections;
using System.IO;
using log4net.Util;

namespace log4net.ObjectRenderer {
    public sealed class DefaultRenderer : IObjectRenderer {
        public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer) {
            if (rendererMap == null) {
                throw new ArgumentNullException("rendererMap");
            }

            if (obj == null) {
                writer.Write(SystemInfo.NullText);
                return;
            }

            Array array = obj as Array;

            if (array != null) {
                RenderArray(rendererMap, array, writer);
                return;
            }

            IEnumerable enumerable = obj as IEnumerable;

            if (enumerable != null) {
                ICollection collection = obj as ICollection;

                if (collection != null && collection.Count == 0) {
                    writer.Write("{}");
                    return;
                }

                IDictionary dictionary = obj as IDictionary;

                if (dictionary != null) {
                    RenderEnumerator(rendererMap, dictionary.GetEnumerator(), writer);
                } else {
                    RenderEnumerator(rendererMap, enumerable.GetEnumerator(), writer);
                }
            } else {
                IEnumerator enumerator = obj as IEnumerator;

                if (enumerator != null) {
                    RenderEnumerator(rendererMap, enumerator, writer);
                    return;
                }

                if (obj is DictionaryEntry) {
                    RenderDictionaryEntry(rendererMap, (DictionaryEntry)obj, writer);
                    return;
                }

                string text = obj.ToString();
                writer.Write(text != null ? text : SystemInfo.NullText);
            }
        }

        void RenderArray(RendererMap rendererMap, Array array, TextWriter writer) {
            if (array.Rank != 1) {
                writer.Write(array.ToString());
                return;
            }

            writer.Write(array.GetType().Name + " {");
            int length = array.Length;

            if (length > 0) {
                rendererMap.FindAndRender(array.GetValue(0), writer);

                for (int i = 1; i < length; i++) {
                    writer.Write(", ");
                    rendererMap.FindAndRender(array.GetValue(i), writer);
                }
            }

            writer.Write("}");
        }

        void RenderEnumerator(RendererMap rendererMap, IEnumerator enumerator, TextWriter writer) {
            writer.Write("{");

            if (enumerator != null && enumerator.MoveNext()) {
                rendererMap.FindAndRender(enumerator.Current, writer);

                while (enumerator.MoveNext()) {
                    writer.Write(", ");
                    rendererMap.FindAndRender(enumerator.Current, writer);
                }
            }

            writer.Write("}");
        }

        void RenderDictionaryEntry(RendererMap rendererMap, DictionaryEntry entry, TextWriter writer) {
            rendererMap.FindAndRender(entry.Key, writer);
            writer.Write("=");
            rendererMap.FindAndRender(entry.Value, writer);
        }
    }
}