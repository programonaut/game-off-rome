using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class Card : MonoBehaviour
{
    public CardObject card;

    [Title("UI Elements")]
    public Image cardBackground;
    public Image cardFrame;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDescriptionText;
    public TextMeshProUGUI suspicionIncreaseText;
    public TextMeshProUGUI slowdownAmountText;
    public Button cardButton;


    public void SetupCard() {
        // set card image
        if (card.cardBackground != null) {
            cardBackground.sprite = card.cardBackground;
        }
        if (card.cardFrame != null) {
            cardFrame.sprite = card.cardFrame;
        }
        // set card text
        cardNameText.text = card.cardName;
        cardDescriptionText.text = card.cardDescription;
        suspicionIncreaseText.text = card.suspicionIncrease.ToString();
        slowdownAmountText.text = card.slowdownAmountType.ToString();

        // set card button action
        cardButton.onClick.AddListener(card.Execute);
    }
}
