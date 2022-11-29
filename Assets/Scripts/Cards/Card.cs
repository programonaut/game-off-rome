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

    [Title("Suspicion")]
    public GameObject suspicionIncrease;
    public GameObject[] suspicionIncreaseIcons;
    public GameObject suspicionDecrease;
    public GameObject[] suspicionDecreaseIcons;

    [Title("Slowdown")]
    public GameObject buildTimeIncrease;
    public GameObject[] buildTimeIncreaseIcons;
    public GameObject buildTimeDecrease;
    public GameObject[] slowdownDecreaseIcons;

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

        SetSuspicion(card.suspicionIncrease, card.goodCard);
        SetSlowdown(card.slowdownAmountType, card.goodCard);

        // set card button action
        cardButton.onClick.AddListener(card.Execute);
        cardButton.onClick.AddListener(() => {CardSystem.Instance.RemoveCard(card);});
    }

    private void SetSuspicion(SuspicousnessAmountType suspicon, bool goodCard) {
        if (goodCard) {
            suspicionIncrease.SetActive(false);
            suspicionDecrease.SetActive(true);
            for (int i = 0; i < suspicionDecreaseIcons.Length; i++) {
                suspicionDecreaseIcons[i].SetActive(false);
                if (i == (int)suspicon)
                    suspicionDecreaseIcons[i].SetActive(true);
            }
        } else {
            suspicionIncrease.SetActive(true);
            suspicionDecrease.SetActive(false);
            for (int i = 0; i < suspicionIncreaseIcons.Length; i++) {
                suspicionIncreaseIcons[i].SetActive(false);
                if (i == (int)suspicon)
                    suspicionIncreaseIcons[i].SetActive(true);
            }
        }
    }

    private void SetSlowdown(SlowAmountType slowdown, bool goodCard) {
        if (goodCard) {
            buildTimeIncrease.SetActive(false);
            buildTimeDecrease.SetActive(true);
            for (int i = 0; i < slowdownDecreaseIcons.Length; i++) {
                slowdownDecreaseIcons[i].SetActive(false);
                if (i == (int)slowdown)
                    slowdownDecreaseIcons[i].SetActive(true);
            }
        } else {
            buildTimeIncrease.SetActive(true);
            buildTimeDecrease.SetActive(false);
            for (int i = 0; i < buildTimeIncreaseIcons.Length; i++) {
                buildTimeIncreaseIcons[i].SetActive(false);
                if (i == (int)slowdown)
                    buildTimeIncreaseIcons[i].SetActive(true);
            }
        }
    }
}
