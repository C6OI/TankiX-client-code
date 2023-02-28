using UnityEngine;
using UnityEngine.EventSystems;

public class Clicker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
    static Clicker selected;

    void Start() { }

    void Update() { }

    public void OnPointerClick(PointerEventData eventData) {
        if (selected != null) {
            selected.GetComponent<Animator>().SetBool("selected", false);
        }

        if (selected != this) {
            selected = this;
            GetComponent<Animator>().SetBool("selected", true);
        } else {
            selected = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GetComponent<Animator>().SetBool("over", true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Animator>().SetBool("over", false);
    }
}