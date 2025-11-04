// Assets/Scripts/Board/LastFiveUI.cs
using TMPro;
using UnityEngine;

public class LastFiveUI : MonoBehaviour
{
    public DeckManager deck;
    public TMP_Text text;

    void OnEnable() { Refresh(); }
    void Update() { Refresh(); } // 간단히 매 프레임 갱신(원하면 이벤트화)

    void Refresh()
    {
        if (!text) return;
        if (deck == null) { text.text = "최근 5장: (없음)"; return; }
        var list = deck.GetLastFive();
        text.text = (list.Count == 0) ? "최근 5장: (없음)" : "최근 5장: " + string.Join(", ", list);
    }
}
