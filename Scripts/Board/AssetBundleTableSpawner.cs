using System.Collections;
using UnityEngine;

public class AssetBundleTableSpawner : MonoBehaviour
{
    [Header("AssetBundle info")]
    public string bundlePath;      // 전체 경로(StreamingAssets 등) 또는 WWW/파일 경로
    public string prefabNameInBundle; // 번들 안 프리팹 이름

    [Header("Spawn")]
    public Transform parent;       // 보통 Canvas/BoardRoot 같은 곳
    public Vector3 position;
    public Vector3 eulerAngles = Vector3.zero;
    public Vector3 scale = Vector3.one;

    IEnumerator Start()
    {
        if (string.IsNullOrEmpty(bundlePath) || string.IsNullOrEmpty(prefabNameInBundle))
        {
            Debug.LogWarning("[AB Table] bundlePath/prefabNameInBundle 비어있음");
            yield break;
        }

        // 파일에서 동기 로드(간단 버전). 필요시 WWW/UnityWebRequest로 교체 가능.
        var bundle = AssetBundle.LoadFromFile(bundlePath);
        if (!bundle)
        {
            Debug.LogError($"[AB Table] 번들 로드 실패: {bundlePath}");
            yield break;
        }

        var prefab = bundle.LoadAsset<GameObject>(prefabNameInBundle);
        if (!prefab)
        {
            Debug.LogError($"[AB Table] 프리팹 로드 실패: {prefabNameInBundle}");
            bundle.Unload(false);
            yield break;
        }

        var go = Instantiate(prefab, parent ? parent : transform);
        go.transform.localPosition = position;
        go.transform.localEulerAngles = eulerAngles;
        go.transform.localScale = scale;

        // 필요하다면 번들을 유지하거나, 메모리 회수를 위해 언로드
        bundle.Unload(false);
        yield return null;
    }
}
