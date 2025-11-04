using UnityEngine;
using CardGameTutorial;

public class EnemyDeckManager : DeckBase
{
    void Start() { resourcesPath = "Cards/Enemy"; LoadAll(); }
}
