using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    public static CardSystem Instance;

    public List<CardObject> cards = new List<CardObject>();
    public List<Card> cardInstances = new List<Card>();

    public List<CardObject> playedCards = new List<CardObject>();
    public GameObject cardPrefab;

    private int maxCards = 3;
    private int[] cardOffsets = {0, -375, 375, -750, 750};

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        StartCoroutine(Test());
    }

    public void SpawnCards() {
        RemoveCards();
        List<CardObject> cardsToSpawn = new List<CardObject>();
        for (int i = 0; i < cards.Count; i++) {
            CardObject card = cards[i];
            if (card.action.CheckIfBuilt()) {
                cardsToSpawn.Add(card);
            }

            if (cardsToSpawn.Count == maxCards)
                break;
        }

        Debug.Log("Spawning " + cardsToSpawn.Count + " cards");

        for (int i = 0; i < cardsToSpawn.Count; i++) {
            // index is always 0 because we remove the card from the list after we spawn it
            SpawnCard(cardsToSpawn[i], cardOffsets[i]);
        }
    }

    private void SpawnCard(CardObject cardObject, int offset) {
        Transform cardHolder = UIHandler.Instance.cardHolder;
        GameObject card = Instantiate(cardPrefab, cardHolder);
        card.transform.localPosition = new Vector3(offset, 0, 0);
        Card cardComponent = card.GetComponent<Card>();
        cardComponent.card = cardObject;
        cardComponent.SetupCard();
        cardInstances.Add(cardComponent);

        playedCards.Add(cardObject);
        cards.Remove(cardObject);
    }

    private void RemoveCards() {
        foreach (Card card in cardInstances) {
            Destroy(card.gameObject);
        }
        cardInstances = new List<Card>();
    }

    private void RemoveCard(Card card) {
        playedCards.Add(card.card);
        cards.Remove(card.card);
        cardInstances.Remove(card);
        Destroy(card.gameObject);
    }

    IEnumerator Test() {
        yield return new WaitForSeconds(1);
        SpawnCards();
    }
}
