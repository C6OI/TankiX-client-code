namespace Tanks.Lobby.ClientControls.API {
    public interface IUIList {
        void AddItem(object data);

        void RemoveItem(object data);

        void ClearItems();
    }
}