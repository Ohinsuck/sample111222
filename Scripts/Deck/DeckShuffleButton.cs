// Assets/Scripts/Board/DeckShuffleButton.cs
using UnityEngine;
using UnityEngine.UI;

public class DeckShuffleButton : MonoBehaviour
{
    public DeckManager deck;

    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn) btn.onClick.AddListener(() => { if (deck) deck.ShuffleDeckOnly(); });
    }
}
