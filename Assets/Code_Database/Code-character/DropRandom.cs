using UnityEngine;
using UnityEngine.EventSystems;

public class DropRandom : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerDrag == null)
            return;

        Drag d = eventData.pointerDrag.GetComponent<Drag>();
        if (d != null) {
            d.placeHolderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerDrag == null)
            return;

        Drag d = eventData.pointerDrag.GetComponent<Drag>();
        if (d != null && d.placeHolderParent == this.transform) {
            d.placeHolderParent = d.parentToReTurnTo;
        }
    }

    public void OnDrop(PointerEventData eventData) {
        Drag d = eventData.pointerDrag.GetComponent<Drag>();
        if (d != null) {
            d.parentToReTurnTo = this.transform;
        }
    }
}