using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientGarage.API {
    public class BuildBattleResultTankEvent : Event {
        public string HullGuid { get; set; }

        public string WeaponGuid { get; set; }

        public string PaintGuid { get; set; }

        public string CoverGuid { get; set; }

        public bool BestPlayerScreen { get; set; }

        public RenderTexture tankPreviewRenderTexture { get; set; }
    }
}