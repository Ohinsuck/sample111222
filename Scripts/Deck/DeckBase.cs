using System.Collections.Generic;
using UnityEngine;
using CardGameTutorial;

public abstract class DeckBase : MonoBehaviour
{
    [HideInInspector] public List<Card> allCards = new();
    [SerializeField] protected string resourcesPath = "";

    protected virtual void LoadAll()
    {
        allCards.Clear();
        if (!string.IsNullOrEmpty(resourcesPath))
            allCards.AddRange(Resources.LoadAll<Card>(resourcesPath));
    }

#if UNITY_EDITOR
    protected virtual void OnValidate() { LoadAll(); }
#endif
}
