// Assets/Scripts/Deck/GraveDropZone.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class GraveDropZone : MonoBehaviour, IDropHandler
{
    public enum Mode { Auto, Delete, DiscardToDeck }
    public Mode mode = Mode.Auto;
    public DeckManager targetDeck; // Auto/DiscardToDeck에서 카드면 discard 할 대상

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData == null || eventData.pointerDrag == null) return;
        var go = eventData.pointerDrag;

        var disp = go.GetComponent<CardDisplay>();
        bool isCard = disp && !disp.treatAsTokenOrUnit;
        bool isToken = disp && disp.treatAsTokenOrUnit;

        void NotifyHand(GameObject g)
        {
            var hi = g.GetComponent<HandItem>();
            if (hi && hi.owner) hi.owner.NotifyItemDestroyed(g);
        }

        switch (mode)
        {
            case Mode.Auto:
                if (isToken)
                {
                    NotifyHand(go);
                    Destroy(go); // 토큰/유닛은 삭제
                }
                else if (isCard && targetDeck)
                {
                    if (disp.cardData) targetDeck.Discard(disp.cardData);
                    NotifyHand(go);
                    Destroy(go);
                }
                else
                {
                    NotifyHand(go);
                    Destroy(go);
                }
                break;

            case Mode.Delete:
                NotifyHand(go);
                Destroy(go);
                break;

            case Mode.DiscardToDeck:
                if (isCard && targetDeck && disp.cardData) targetDeck.Discard(disp.cardData);
                NotifyHand(go);
                Destroy(go);
                break;
        }
    }
}
