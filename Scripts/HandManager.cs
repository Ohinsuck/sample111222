using System.Collections.Generic;
using UnityEngine;
using CardGameTutorial;

public class HandManager : MonoBehaviour
{
    [Header("References")]
    public DeckManager deckManager;
    public GameObject cardPrefab;    // 반드시 RectTransform 루트 (PlayerPrefab_UI)
    public Transform handTransform;  // Canvas/HandPosition (RectTransform)

    [Header("Layout")]
    public float fanSpread = 9f;
    public float cardSpacing = 180f;
    public float verticalSpacing = 40f;
    public float handScale = 1.4f;

    readonly List<GameObject> cardsInHand = new();

    void Awake()
    {
        if (!handTransform) handTransform = transform;

        // 루트 Canvas 보정 + GraphicRaycaster 보장
        var cv = GetComponentInParent<Canvas>()?.rootCanvas;
        if (cv && !cv.GetComponent<UnityEngine.UI.GraphicRaycaster>())
            cv.gameObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // HandPosition을 최상단쪽으로(DragLayer 바로 아래 권장)
        if (handTransform && handTransform is RectTransform hrt)
        {
            hrt.localScale = Vector3.one;
            hrt.anchoredPosition3D = new Vector3(0, 80, 0);
            hrt.SetAsLastSibling();
            var drag = (cv ? cv.transform.Find("DragLayer") : null);
            if (drag) drag.SetAsLastSibling();
        }
    }

    public void AddCardToHand(Card cardData)
    {
        if (!cardData) return;
        if (!cardPrefab)
        {
            Debug.LogError("[HandManager] cardPrefab 비어있음 (PlayerPrefab_UI 할당 필요)");
            return;
        }
        var rtCheck = cardPrefab.GetComponent<RectTransform>();
        if (!rtCheck)
        {
            Debug.LogError("[HandManager] cardPrefab 루트는 RectTransform 이어야 함");
            return;
        }

        var go = Instantiate(cardPrefab, handTransform);
        go.name = $"{cardData.cardName}_InHand";
        go.transform.localScale = Vector3.one * handScale;

        var disp = go.GetComponent<CardDisplay>();
        if (disp) { disp.treatAsTokenOrUnit = false; disp.SetCard(cardData); }

        var drag = go.GetComponent<SummonDragHandler>();
        if (drag) drag.InitForHandOnly();

        var hi = go.GetComponent<HandItem>();
        if (!hi) hi = go.AddComponent<HandItem>();
        hi.owner = this;

        // CanvasGroup 보장(투명/차단 이슈 방지)
        var cg = go.GetComponent<CanvasGroup>();
        if (!cg) cg = go.AddComponent<CanvasGroup>();
        cg.alpha = 1f; cg.interactable = true; cg.blocksRaycasts = true;

        cardsInHand.Add(go);

        // 디버그 체크포인트
        Debug.Log($"[HandManager] Added '{cardData.cardName}'  parent={go.transform.parent?.name}  pos={go.GetComponent<RectTransform>().anchoredPosition}");

        UpdateHandVisuals(true);
    }

    public void RemoveFromHand(GameObject go)
    {
        if (!go) return;
        int idx = cardsInHand.IndexOf(go);
        if (idx >= 0) cardsInHand.RemoveAt(idx);
        Destroy(go);
        UpdateHandVisuals();
    }

    public void NotifyItemDestroyed(GameObject go)
    {
        int idx = cardsInHand.IndexOf(go);
        if (idx >= 0) { cardsInHand.RemoveAt(idx); UpdateHandVisuals(); }
    }

    void LateUpdate() => UpdateHandVisuals();

    public void UpdateHandVisuals(bool forceInstant = false)
    {
        for (int i = cardsInHand.Count - 1; i >= 0; --i)
            if (!cardsInHand[i]) cardsInHand.RemoveAt(i);

        var active = new List<RectTransform>(cardsInHand.Count);
        foreach (var go in cardsInHand)
        {
            if (!go) continue;
            var rt = go.GetComponent<RectTransform>();
            if (!rt) continue;
            if (rt.transform.parent != handTransform) continue; // 드래그 중 제외
            active.Add(rt);
        }

        int n = active.Count;
        if (n == 0) return;

        float mid = (n - 1) * 0.5f;
        for (int i = 0; i < n; i++)
        {
            var rt = active[i];
            float t = i - mid;
            var targetPos = new Vector2(t * cardSpacing, -Mathf.Abs(t) * verticalSpacing);
            var targetRot = Quaternion.Euler(0, 0, -t * fanSpread);

            if (forceInstant)
            {
                rt.anchoredPosition = targetPos;
                rt.localRotation = targetRot;
                rt.localScale = Vector3.one * handScale;
            }
            else
            {
                rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, targetPos, 0.25f);
                rt.localRotation = Quaternion.Lerp(rt.localRotation, targetRot, 0.25f);
                rt.localScale = Vector3.one * handScale;
            }
        }
    }

    // 손패 셔플(버튼에서 호출)
    public void ShuffleHand()
    {
        for (int i = cardsInHand.Count - 1; i > 0; --i)
        {
            int j = Random.Range(0, i + 1);
            (cardsInHand[i], cardsInHand[j]) = (cardsInHand[j], cardsInHand[i]);
        }
        UpdateHandVisuals(true);
        Debug.Log("[HandManager] Hand shuffled.");
    }
}
