using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class ConditionDescription {
        public string format { get; set; }

        public Dictionary<CaseType, string> cases { get; set; }
    }
}