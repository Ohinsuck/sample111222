using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnOnDrag :
    MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("꺼내올 실제 UI 프리팹 (RectTransform 루트, Image/TMP, CanvasGroup, SummonDragHandler 포함)")]
    public GameObject prefab;

    [Tooltip("없으면 자동으로 루트 Canvas를 잡습니다.")]
    public RectTransform parentForSpawn;

    GameObject _clone;
    RectTransform _dragLayer;
    Canvas _rootCanvas;

    void CacheCanvas()
    {
        if (_rootCanvas) return;
        _rootCanvas = GetComponentInParent<Canvas>()?.rootCanvas;
        if (!_rootCanvas) _rootCanvas = Object.FindFirstObjectByType<Canvas>(FindObjectsInactive.Include);
        if (_rootCanvas) _dragLayer = _rootCanvas.transform.Find("DragLayer") as RectTransform;
        if (!parentForSpawn && _rootCanvas) parentForSpawn = _rootCanvas.transform as RectTransform;
    }

    static bool IsValidUIPrefab(GameObject go)
    {
        return go && go.GetComponent<RectTransform>() && go.GetComponentInChildren<Graphic>(true);
    }

    public void OnPointerDown(PointerEventData e)
    {
        // 클릭 즉시 복제 + 드래그 시작 위임
        TrySpawnAndBeginDrag(e);
    }

    public void OnBeginDrag(PointerEventData e)
    {
        // 드래그에서 시작한 케이스도 동일하게 처리
        TrySpawnAndBeginDrag(e);
    }

    void TrySpawnAndBeginDrag(PointerEventData e)
    {
        CacheCanvas();
        if (_clone || !prefab || !IsValidUIPrefab(prefab) || !parentForSpawn) return;

        // 복제: 좌표는 지정 안 함(튕김 방지). 드래그에서 현재 포인터로 자리 잡음.
        var parent = _dragLayer ? _dragLayer : parentForSpawn;
        _clone = Object.Instantiate(prefab, parent);
        _clone.name = prefab.name + "_Clone";
        _clone.transform.localScale = Vector3.one;
        _clone.transform.SetAsLastSibling();

        // 즉시 드래그 위임
        e.pointerDrag = _clone;
        ExecuteEvents.Execute(_clone, e, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData e)
    {
        if (_clone) ExecuteEvents.Execute(_clone, e, ExecuteEvents.dragHandler);
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (_clone)
        {
            ExecuteEvents.Execute(_clone, e, ExecuteEvents.endDragHandler);
            _clone = null;
        }
    }
}
