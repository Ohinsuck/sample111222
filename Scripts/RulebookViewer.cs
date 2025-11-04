// Assets/Scripts/UI/RulebookViewer.cs  (교체 아님, 확인)
using UnityEngine;
using UnityEngine.UI;

public class RulebookViewer : MonoBehaviour
{
    public GameObject panel;   // 풀스크린 패널(비활성 시작)
    public Image image;        // 패널 안의 Image
    public Sprite rulebookSprite;

    void Awake() { if (panel) panel.SetActive(false); }

    public void Open()
    {
        if (!panel || !image || !rulebookSprite) return;
        image.sprite = rulebookSprite;
        panel.SetActive(true);

        // 배경 클릭으로 닫기
        var btn = panel.GetComponent<Button>();
        if (!btn) btn = panel.AddComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => panel.SetActive(false));
    }

    public void Close() { if (panel) panel.SetActive(false); }
}
