// Assets/Scripts/CardDisplay.cs
using TMPro;
using UnityEngine;
using CardGameTutorial;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;

    [Header("UI")]
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text apText;
    public TMP_Text faithText;
    public TMP_Text effectText;
    public TMP_Text dragonGaugeText;

    [Header("Behavior")]
    [Tooltip("true면 묘지 드롭 시 Discard 하지 않고 삭제(Delete)")]
    public bool treatAsTokenOrUnit = false;

    public void SetCard(Card data)
    {
        cardData = data;
        UpdateCardDisplay();
    }

    void Start() => UpdateCardDisplay();

    public void UpdateCardDisplay()
    {
        if (!cardData) return;
        if (nameText) nameText.text = cardData.cardName;
        if (healthText) healthText.text = cardData.health.ToString();
        if (damageText) damageText.text = cardData.damage.ToString();
        if (apText) apText.text = cardData.AP.ToString();
        if (faithText) faithText.text = cardData.Faith.ToString();
        if (effectText) effectText.text = string.IsNullOrWhiteSpace(cardData.effect) ? "" : cardData.effect;
        if (dragonGaugeText) dragonGaugeText.text = Mathf.RoundToInt(cardData.dragonGauge) + "%";
    }
}
