using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A fruit basket class that keeps track of what items are/should be in the basket.
/// This class implements the fruit basket behavior such as collecting fruits,
/// generating an item list, and checking if the player has collected all the items.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Basket : MonoBehaviour
{
    /// <summary>
    /// Represents the item list of the basket. Determines what fruits and their count
    /// that should be in the basket.
    /// </summary>
    public Dictionary<FruitType, int> ItemList { get; private set; }
    
    [Header("Item List Settings")]

    [Tooltip("The minimum number of fruits to be displayed on the item list.")]
    [Range(1, 10)]
    public int minItemsLength = 3;

    [Tooltip("The range of the number of fruits to be displayed on the item list.")]
    [Range(0, 10)]
    public int maxItemsRange = 2;

    [Space(5)]

    [Header("Other Settings")]

    [Tooltip("How long an item list can be displayed for before it disappears and is incompleted.")]
    [Range(0f, 60f)]
    public float timeLimit = 30f;

    [Tooltip("The amount of profit the player gets for completing the basket.")]
    [Range(0, 100)]
    public int profitReward = 10;

    [Tooltip("The amount of reputation the player gets for completing the basket.")]
    [Range(0, 100)]
    public int reputationReward = 10;

    [Tooltip("The amount of reputation the player loses for incompleting the basket.")]
    [Range(0, 100)]
    public int reputationPenalty = 10;

    private float remainingTime;

    public void GenerateItemList()
    {
        ItemList = new();

        int numItems = Random.Range(minItemsLength, minItemsLength + maxItemsRange);

        for (int i = 0; i < numItems; i++)
        {
            FruitType fruitType = (FruitType)Random.Range(0, System.Enum.GetValues(typeof(FruitType)).Length);

            if (ItemList.ContainsKey(fruitType))
            {
                ItemList[fruitType]++;
            }
            else
            {
                ItemList.Add(fruitType, 1);
            }
        }

        remainingTime = timeLimit;
    }

    private void Update()
    {
        if (remainingTime <= 0)
        {
            GameManager.Instance.IncompleteBasket(reputationPenalty);
        }
        else
        {
            remainingTime -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            Fruit fruit = collision.gameObject.GetComponent<Fruit>();
            CollectFruit(fruit);
            Destroy(collision.gameObject);
        }
    }

    private void CollectFruit(Fruit fruit)
    {
        FruitType fruitType = fruit.fruitType;
        if (ItemList.ContainsKey(fruitType))
        {
            ItemList[fruitType]--;

            if (ItemList[fruitType] <= 0)
            {
                ItemList.Remove(fruitType);
            }
        }

        if (ItemList.Count == 0)
        {
            GameManager.Instance.CompleteBasket(profitReward, reputationReward);
            GenerateItemList();
        }
    }
}
