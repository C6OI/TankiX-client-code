using System.Collections.Generic;

namespace Tanks.Battle.ClientCore.API {
    public class GravityTypeNames {
        public const string configPath = "localization/gravity_type";

        public Dictionary<GravityType, string> Names { get; set; }
    }
}