using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameTutorial
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public int health;
        public int damage;
        public int AP;
        public int Faith;
        public string effect;
        public float dragonGauge;

        public enum CardEffectType
        {
            DamageSingle,
            DamageMultiTarget,
            BuffFaith,
            BuffAP,
            Draw,
            SummonUnit
        }
    }

}