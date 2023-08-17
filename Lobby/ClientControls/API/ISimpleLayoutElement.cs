namespace Lobby.ClientControls.API {
    public interface ISimpleLayoutElement {
        float flexibleWidth { get; }

        float minWidth { get; }

        float maxWidth { get; }

        float flexibleHeight { get; }

        float minHeight { get; }

        float maxHeight { get; }

        int layoutPriority { get; }
    }
}