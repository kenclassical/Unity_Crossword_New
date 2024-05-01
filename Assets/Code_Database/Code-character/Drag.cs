using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReTurnTo = null;
    public Transform placeHolderParent = null;
    public GameObject placeHolder = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        placeHolder = new GameObject();
        placeHolder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;
        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReTurnTo = this.transform.parent;
        placeHolderParent = parentToReTurnTo;
        this.transform.SetParent(this.transform.parent.parent);

        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;

        if (placeHolder.transform.parent != placeHolderParent)
            placeHolder.transform.SetParent(placeHolderParent);
        
        int newSiblingIndex = placeHolderParent.childCount;

        for (int i = 0; i < placeHolderParent.childCount; i++)
        {
            if (this.transform.position.x < placeHolderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;
                
                break;
            }
        }
        
        placeHolder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReTurnTo);
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeHolder);
    }
}
