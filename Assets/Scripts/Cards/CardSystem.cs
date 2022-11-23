using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    public static CardSystem Instance;

    public List<CardObject> cards = new List<CardObject>();
    public GameObject cardPrefab;

    private int maxCards = 3;
    private int cardOffset = 375;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SpawnCard();
    }

    private void SpawnCard() {
        // check if card can be spawned

        // spawn card
        GameObject card = Instantiate(cardPrefab, UIHandler.Instance.cardHolder);
        Card cardComponent = card.GetComponent<Card>();
        cardComponent.card = cards[0];
        cardComponent.SetupCard();
    }
}
