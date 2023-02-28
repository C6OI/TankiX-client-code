namespace Tanks.Lobby.ClientControls.API {
    public interface SizeController {
        void RegisterSpriteRequest(SpriteRequest request);

        void UnregisterSpriteRequest(SpriteRequest request);
    }
}