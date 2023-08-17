using System.Collections.Generic;
using System.Globalization;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Utilities {
    sealed class ObjectAnchorCollection {
        readonly IDictionary<object, string> anchorsByObject = new Dictionary<object, string>();
        readonly IDictionary<string, object> objectsByAnchor = new Dictionary<string, object>();

        public object this[string anchor] {
            get {
                object value;

                if (objectsByAnchor.TryGetValue(anchor, out value)) {
                    return value;
                }

                throw new AnchorNotFoundException(string.Format(CultureInfo.InvariantCulture,
                    "The anchor '{0}' does not exists",
                    anchor));
            }
        }

        public void Add(string anchor, object @object) {
            objectsByAnchor.Add(anchor, @object);

            if (@object != null) {
                anchorsByObject.Add(@object, anchor);
            }
        }

        public bool TryGetAnchor(object @object, out string anchor) => anchorsByObject.TryGetValue(@object, out anchor);
    }
}