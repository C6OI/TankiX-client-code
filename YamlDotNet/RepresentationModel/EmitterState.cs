using System.Collections.Generic;

namespace YamlDotNet.RepresentationModel {
    class EmitterState {
        public HashSet<string> EmittedAnchors { get; } = new();
    }
}