using UnityEngine;
using UnityEngine.UI;

public class GraveyardUI : MonoBehaviour
{
    public DeckManager playerDeck;
    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn) btn.onClick.AddListener(() =>
        {
            if (playerDeck) playerDeck.ShuffleFromGrave();
        });
    }
}
