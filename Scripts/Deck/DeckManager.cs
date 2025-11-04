// Assets/Scripts/Deck/DeckManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using CardGameTutorial;

public class DeckManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private string resourcesPath = "Cards/Player";

    [Header("State")]
    public List<Card> allCards = new();   // 현재 덱(남은 카드)
    public List<Card> discard = new();    // 묘지(사용 카드)

    // 최근 5장 기록(드로우 기준)
    readonly List<Card> lastFive = new();
    public IReadOnlyList<Card> LastFive => lastFive;

    [Header("Options")]
    public bool recycleDiscard = true; // true: 덱이 비면 discard에서 리필

    public event Action OnDeckChanged; // UI 등이 구독

    void Start()
    {
        LoadAllCards();
        Shuffle(allCards);
        NotifyChanged();
    }

    // === 외부 API ===

    /// <summary>손패로 한 장 드로우</summary>
    public void DrawCard(HandManager hand)
    {
        if (!hand) return;

        if (allCards.Count == 0)
        {
            if (recycleDiscard) RefillFromDiscard();
        }
        if (allCards.Count == 0) return; // 여전히 비었으면 종료

        var c = allCards[0];
        allCards.RemoveAt(0);

        // 손패 UI 생성
        hand.AddCardToHand(c);

        // 최근 5장 기록(드로우된 카드)
        PushLastFive(c);

        NotifyChanged();
    }

    /// <summary>카드 사용(버림)</summary>
    public void Discard(Card c)
    {
        if (!c) return;
        discard.Add(c);
        NotifyChanged();
    }

    /// <summary>묘지 → 덱으로 리필(셔플 포함)</summary>
    public void ShuffleFromGrave() => RefillFromDiscard();

    /// <summary>현재 덱만 셔플(상태 유지)</summary>
    public void ShuffleDeckOnly()
    {
        Shuffle(allCards);
        NotifyChanged();
    }

    /// <summary>덱/묘지 완전 초기화 후 리소스 재로딩 + 셔플</summary>
    public void ResetDeck()
    {
        allCards.Clear();
        discard.Clear();
        LoadAllCards();
        Shuffle(allCards);
        lastFive.Clear();
        NotifyChanged();
    }

    // 최근 5장 이름(Discard 말고, 드로우 기록 기준)
    public List<string> GetLastFive()
    {
        var list = new List<string>();
        for (int i = Mathf.Max(0, lastFive.Count - 5); i < lastFive.Count; i++)
            if (lastFive[i]) list.Add(lastFive[i].cardName);
        return list;
    }

    // === 내부 ===

    void LoadAllCards()
    {
        allCards.Clear();
        discard.Clear();
        lastFive.Clear();

        // Resources 폴더에서 카드 ScriptableObject 로드
        allCards.AddRange(Resources.LoadAll<Card>(resourcesPath));
    }

    void RefillFromDiscard()
    {
        if (discard.Count == 0) return;
        allCards.AddRange(discard);
        discard.Clear();
        Shuffle(allCards);
    }

    void Shuffle(List<Card> list)
    {
        for (int i = list.Count - 1; i > 0; --i)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    void PushLastFive(Card c)
    {
        if (!c) return;
        lastFive.Add(c);
        if (lastFive.Count > 5) lastFive.RemoveAt(0);
    }

    void NotifyChanged() => OnDeckChanged?.Invoke();
}
