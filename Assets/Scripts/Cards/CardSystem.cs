using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
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

        var rng = new System.Random();

        CardObject[] cardArray = cards.ToArray();
        rng.Shuffle<CardObject>(cardArray);
        cards = new List<CardObject>(cardArray);
    }

    // resturn true if more than 0 cards were spawned
    public bool SpawnCards() {
        List<CardObject> cardsToSpawn = new List<CardObject>();
        for (int i = 0; i < cards.Count; i++) {
            CardObject card = cards[i];
            if (card.CheckIfBuilt()) {
                cardsToSpawn.Add(card);
            }

            if (cardsToSpawn.Count == maxCards)
                break;
        }

        for (int i = 0; i < cardsToSpawn.Count; i++) {
            // index is always 0 because we remove the card from the list after we spawn it
            SpawnCard(cardsToSpawn[i], cardOffsets[i]);
        }

        return cardsToSpawn.Count > 0;
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

    public void RemoveCards() {
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


    [SerializeField] private List<CardData> cardNames = new List<CardData>();
    [Button]
    private void CreatCardObjectsFromList() {
        int index = 1;
        foreach (var cardData in cardNames) {
            int amount = cardData.amount == 0 ? 1 : cardData.amount;
            
            for (int i = 0; i < amount; i++)
            {
                CardObject card = ScriptableObject.CreateInstance<CardObject>();
                card.cardName = cardData.name;
                if (cardData.id != 0)
                    card.needsToBeBuild = true;
                card.buildingId = cardData.id;
                
                string path = $"Assets/_Cards/{index}_{cardData.name}.asset";
                cards.Add(card);

                AssetDatabase.CreateAsset(card, path);
                index++;
            }
        }

    }
}

[Serializable]
struct CardData {
    public string name;
    public int id;
    public ActionType type;
    public int amount;
}