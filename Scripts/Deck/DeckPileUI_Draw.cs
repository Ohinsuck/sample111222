using UnityEngine;
using UnityEngine.UI;

public class DeckPileUI_Draw : MonoBehaviour
{
    public DeckManager playerDeck;
    public HandManager hand;

    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn) btn.onClick.AddListener(() =>
        {
            if (playerDeck && hand) playerDeck.DrawCard(hand);
        });
    }
}
