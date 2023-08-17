using System;
using System.Collections.Generic;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers {
    public sealed class TagNodeTypeResolver : INodeTypeResolver {
        readonly IDictionary<string, Type> tagMappings;

        public TagNodeTypeResolver(IDictionary<string, Type> tagMappings) {
            if (tagMappings == null) {
                throw new ArgumentNullException("tagMappings");
            }

            this.tagMappings = tagMappings;
        }

        bool INodeTypeResolver.Resolve(NodeEvent nodeEvent, ref Type currentType) {
            Type value;

            if (!string.IsNullOrEmpty(nodeEvent.Tag) && tagMappings.TryGetValue(nodeEvent.Tag, out value)) {
                currentType = value;
                return true;
            }

            return false;
        }
    }
}