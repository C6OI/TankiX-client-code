using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class MandatoryAssetsFirstLoadingComponent : Component {
        [Flags]
        public enum MandatoryRequestsState {
            HANGAR = 0,
            WEAPON_SKIN = 1,
            HULL_SKIN = 2,
            TANK_COLORING = 3,
            WEAPON_COLORING = 4,
            CONTAINER = 5
        }

        readonly List<AssetRequestComponent> assetsRequests = new();

        MandatoryRequestsState currentRequestsState;

        readonly MandatoryRequestsState finishRequestsState = MandatoryRequestsState.TANK_COLORING | MandatoryRequestsState.WEAPON_COLORING;

        public bool LoadingScreenShown { get; set; }

        public void MarkAsRequested(AssetRequestComponent assetRequest, MandatoryRequestsState mandatoryRequestsState) {
            currentRequestsState |= mandatoryRequestsState;
            assetsRequests.Add(assetRequest);
        }

        public bool AllMandatoryAssetsAreRequested() => currentRequestsState == finishRequestsState;

        public bool IsAssetRequestMandatory(AssetRequestComponent assetRequest) => assetsRequests.Contains(assetRequest);

        public bool IsContainerRequested() => (currentRequestsState & MandatoryRequestsState.CONTAINER) == MandatoryRequestsState.CONTAINER;
    }
}