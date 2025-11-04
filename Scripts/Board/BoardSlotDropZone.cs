// Assets/Scripts/Board/BoardSlotDropZone.cs
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class BoardSlotDropZone : MonoBehaviour, IDropHandler
{
    public bool acceptCards = true;
    public bool acceptTokens = true;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        var go = eventData.pointerDrag;

        var disp = go.GetComponent<CardDisplay>();
        bool isCard = disp && !disp.treatAsTokenOrUnit;
        bool isToken = disp && disp.treatAsTokenOrUnit;

        if ((isCard && !acceptCards) || (isToken && !acceptTokens)) return;

        var rt = go.GetComponent<RectTransform>();
        if (!rt) return;

        rt.SetParent(transform, false);
        rt.anchoredPosition = Vector2.zero; // ½½·Ô Áß¾Ó ½º³À
    }
}
