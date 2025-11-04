// Assets/Scripts/Board/DeckResetButton.cs
using UnityEngine;
using UnityEngine.UI;

public class DeckResetButton : MonoBehaviour
{
    public DeckManager deck;

    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn) btn.onClick.AddListener(() => { if (deck) deck.ResetDeck(); });
    }
}
