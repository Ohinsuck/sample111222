using UnityEngine;
using UnityEngine.UI;

public class HandShuffleButton : MonoBehaviour
{
    public HandManager hand;
    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn) btn.onClick.AddListener(() =>
        {
            if (hand) hand.ShuffleHand();
        });
    }
}
