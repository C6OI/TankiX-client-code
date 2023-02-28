using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragAndDropCell : MonoBehaviour, IDropHandler, IEventSystemHandler {
    public IDropController dropController;

    public void OnDrop(PointerEventData data) {
        if (DragAndDropItem.draggedItemContentCopy != null && DragAndDropItem.draggedItemContentCopy.activeSelf) {
            DragAndDropItem draggedItem = DragAndDropItem.draggedItem;
            DragAndDropCell sourceCell = DragAndDropItem.sourceCell;
            dropController.OnDrop(sourceCell, this, draggedItem);
        }
    }

    public void RemoveItem() {
        DragAndDropItem[] componentsInChildren = GetComponentsInChildren<DragAndDropItem>();

        foreach (DragAndDropItem dragAndDropItem in componentsInChildren) {
            Destroy(dragAndDropItem.gameObject);
        }
    }

    public void PlaceItem(DragAndDropItem item) {
        if (item != null) {
            item.transform.SetParent(transform, false);
            item.transform.localPosition = Vector3.zero;
        }

        gameObject.SendMessageUpwards("OnItemPlace", item, SendMessageOptions.DontRequireReceiver);
    }

    public DragAndDropItem GetItem() => GetComponentInChildren<DragAndDropItem>();

    public void SwapItems(DragAndDropCell sourceCell, DragAndDropItem item) {
        DragAndDropItem item2 = GetItem();
        PlaceItem(item);

        if (item2 != null) {
            sourceCell.PlaceItem(item2);
        }
    }
}