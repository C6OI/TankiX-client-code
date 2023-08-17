using System.Collections.Generic;
using Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyboardNavigation : MonoBehaviour {
    readonly HashSet<Selectable> traversed = new();

    void Update() {
        Selectable selectable = null;
        GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

        if (currentSelectedGameObject == null) {
            return;
        }

        Selectable component = currentSelectedGameObject.GetComponent<Selectable>();

        if (component == null) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            traversed.Clear();

            selectable = !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) ? FindDown(component)
                             : FindUp(component);
        } else if (Input.GetKeyDown(KeyCode.Return)) {
            traversed.Clear();

            if (component is InputField && !HasCustomNavigation(component)) {
                selectable = FindDown(component);
            }
        }

        if (selectable != null) {
            selectable.Select();
        }
    }

    bool HasCustomNavigation(Selectable current) {
        InputFieldReturnSelector component = current.gameObject.GetComponent<InputFieldReturnSelector>();
        return component != null && component.CanNavigateToSelectable();
    }

    Selectable FindUp(Selectable current) {
        if (traversed.Contains(current)) {
            return null;
        }

        traversed.Add(current);
        Selectable selectable = current.FindSelectableOnUp();

        if (IsValidSelectable(selectable)) {
            return selectable;
        }

        return FindUp(selectable);
    }

    Selectable FindDown(Selectable current) {
        if (traversed.Contains(current)) {
            return null;
        }

        traversed.Add(current);
        Selectable selectable = current.FindSelectableOnDown();

        if (IsValidSelectable(selectable)) {
            return selectable;
        }

        return FindDown(selectable);
    }

    bool IsValidSelectable(Selectable selectable) =>
        selectable == null || selectable.interactable && selectable.gameObject.activeSelf;
}