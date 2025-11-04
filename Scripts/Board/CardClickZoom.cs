using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CardClickZoom : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    public float zoomScale = 2f;
    public float speed = 15f;
    public bool dimBackground = true;

    RectTransform rt;
    Vector3 baseScale;
    Vector3 basePos;
    Transform baseParent;
    Canvas rootCanvas;
    RectTransform dragLayer;
    CanvasGroup cg;

    bool hovering;
    bool zoomed;
    GameObject dimObj;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        baseScale = rt.localScale;
        basePos = rt.anchoredPosition;
        baseParent = rt.parent;

        cg = GetComponent<CanvasGroup>();
        if (!cg) cg = gameObject.AddComponent<CanvasGroup>();

        rootCanvas = GetComponentInParent<Canvas>()?.rootCanvas;
        if (rootCanvas)
        {
            var t = rootCanvas.transform.Find("DragLayer");
            if (t) dragLayer = t as RectTransform;
        }
    }

    void Update()
    {
        // ESC 로 닫기
        if (zoomed && Input.GetKeyDown(KeyCode.Escape))
            Restore();
    }

    public void OnPointerEnter(PointerEventData e) => hovering = true;
    public void OnPointerExit(PointerEventData e) => hovering = false;

    public void OnPointerClick(PointerEventData e)
    {
        // 드래그 중이면 무시(드래그 핸들러가 blocksRaycasts 끄는 동안 클릭 이벤트 튀는 것 방지)
        if (!cg || !cg.blocksRaycasts) return;

        if (!zoomed) ZoomIn();
        else Restore();
    }

    void ZoomIn()
    {
        if (!rootCanvas) return;

        // 최상단으로 올리고 확대
        baseParent = rt.parent;
        basePos = rt.anchoredPosition;
        baseScale = rt.localScale;

        Transform targetParent = dragLayer ? (Transform)dragLayer : rootCanvas.transform;
        rt.SetParent(targetParent, false);
        rt.SetAsLastSibling();

        if (dimBackground) CreateDim();

        rt.localScale = baseScale * zoomScale;
        rt.anchoredPosition = Vector2.zero;

        zoomed = true;
    }

    void Restore()
    {
        if (!baseParent) return;

        rt.SetParent(baseParent, false);
        rt.localScale = baseScale;
        rt.anchoredPosition = basePos;

        if (dimObj) Destroy(dimObj);
        zoomed = false;
    }

    void CreateDim()
    {
        if (!rootCanvas) return;

        dimObj = new GameObject("Dim", typeof(RectTransform), typeof(Image));
        var dimRT = dimObj.GetComponent<RectTransform>();
        dimObj.transform.SetParent(rootCanvas.transform, false);
        dimRT.anchorMin = Vector2.zero; dimRT.anchorMax = Vector2.one;
        dimRT.offsetMin = Vector2.zero; dimRT.offsetMax = Vector2.zero;
        var img = dimObj.GetComponent<Image>();
        img.color = new Color(0, 0, 0, 0.55f);
        dimObj.transform.SetAsLastSibling(); // Dim 위로 카드가 올라오게 아래서 다시 한 번
        rt.SetAsLastSibling();
    }
}
