using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public struct MoveCommand {
        [ProtocolOptional] public Movement? Movement { get; set; }

        [ProtocolOptional] public float? WeaponRotation { get; set; }

        public DiscreteTankControl DiscreteTankControl { get; set; }

        public double ClientTime { get; set; }

        public override string ToString() =>
            string.Format("MoveCommand[Movement={0}, WeaponRotation={1}, DiscreteTankControl={2}]",
                Movement,
                WeaponRotation,
                DiscreteTankControl);
    }
}