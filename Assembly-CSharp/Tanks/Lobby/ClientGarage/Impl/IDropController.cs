namespace Tanks.Lobby.ClientGarage.Impl {
    public interface IDropController {
        void OnDrop(DragAndDropCell cellFrom, DragAndDropCell cellTo, DragAndDropItem item);
    }
}