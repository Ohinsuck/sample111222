using UnityEngine;
using CardGameTutorial;

public class AllyDeckManager : DeckBase
{
    void Start() { resourcesPath = "Cards/Ally"; LoadAll(); }
}
