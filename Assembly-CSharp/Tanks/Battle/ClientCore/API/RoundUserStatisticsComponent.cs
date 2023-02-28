using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(6921761768819133913L)]
    public class RoundUserStatisticsComponent : Component, IComparable<RoundUserStatisticsComponent> {
        public int Place { get; set; }

        public int ScoreWithoutBonuses { get; set; }

        public int Kills { get; set; }

        public int KillAssists { get; set; }

        public int Deaths { get; set; }

        public int CompareTo(RoundUserStatisticsComponent other) => Place.CompareTo(other.Place);
    }
}