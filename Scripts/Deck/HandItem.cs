// Assets/Scripts/HandItem.cs
using UnityEngine;

public class HandItem : MonoBehaviour
{
    public HandManager owner;

    void OnDestroy()
    {
        if (owner) owner.NotifyItemDestroyed(gameObject);
    }
}
