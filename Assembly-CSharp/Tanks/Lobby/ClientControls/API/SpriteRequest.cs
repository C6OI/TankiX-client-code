using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public interface SpriteRequest {
        string Uid { get; }

        void Resolve(Sprite sprite);

        void Cancel();
    }
}