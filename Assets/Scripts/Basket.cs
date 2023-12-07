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

    [Tooltip("Potential candidates for the item list (must contain a Fruit Image component).")]
    public List<GameObject> availableFruits;

    [Space(5)]

    [Header("Other Settings")]

    [Tooltip("How long the basket is available for before it disappears and is incompleted.")]
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

    [Space(5)]

    [Header("References")]

    [SerializeField] private Transform itemListParent;

    [Tooltip("The points where the collected fruits will be placed. There should be as many points " +
        "as there are maximum possible items on the item list.")]
    [SerializeField] private Transform[] collectedPoints;

    private float remainingTime;
    private int collected;
    
    /// <summary>
    /// Initializes a new `ItemList` and instantiates the fruits on the item list.
    /// </summary>
    public void GenerateItemList()
    {
        // Clear the item list
        ItemList.Clear();
        for (int i = itemListParent.childCount - 1; i >= 0; i--)
        {
            Destroy(itemListParent.GetChild(i).gameObject);
        }


        foreach (Transform child in collectedPoints)
        {
            if (child.childCount == 0)
            {
                continue;
            }

            for (int i = child.childCount - 1; i >= 0; i--)
            {
                Destroy(child.GetChild(i).gameObject);
            }
        }   

        int numItems = Random.Range(minItemsLength, minItemsLength + maxItemsRange);

        // Create a list of items
        for (int i = 0; i < numItems; i++)
        {
            GameObject prefab = availableFruits[Random.Range(0, availableFruits.Count)];
            FruitImage fruitImage = Instantiate(prefab, itemListParent).GetComponent<FruitImage>();
            FruitType fruitType = fruitImage.fruitType;

            if (ItemList.ContainsKey(fruitType))
            {
                ItemList[fruitType]++;
            }
            else
            {
                ItemList.Add(fruitType, 1);
            }
        }

        Debug.Log("Item List: " + string.Join(", ", ItemList.Keys) + " " +  string.Join(", ", ItemList.Values));

        // Reset time and counter
        remainingTime = timeLimit;
        collected = 0;
    }

    private void Awake()
    {
        ItemList = new();
    }

    private void Start()
    {
        GenerateItemList();
    }

    private void Update()
    {
        // Update the remaining time
        if (remainingTime <= 0)
        {
            GameManager.Instance.IncompleteBasket(reputationPenalty);
            GenerateItemList();
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
        }
    }

    private void CollectFruit(Fruit fruit)
    {
        // Remove the fruit from the item list
        FruitType ft = fruit.fruitType;
        if (ItemList.ContainsKey(ft))
        {
            ItemList[ft]--;

            if (ItemList[ft] <= 0)
            {
                ItemList.Remove(ft);
            }
        }
        else
        {
            return;
        }

        // Place the fruit in the collected points
        if (collected < collectedPoints.Length)
        {
            Transform point = collectedPoints[collected];
            fruit.transform.SetPositionAndRotation(point.position, point.rotation);
            fruit.transform.parent = point;

            // Disable physics
            Rigidbody rb = fruit.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Collider collider = fruit.GetComponent<Collider>();
            collider.enabled = false;

            collected++;
        }

        // Reward when the item list is fulfilled
        if (ItemList.Count == 0)
        {
            GameManager.Instance.CompleteBasket(profitReward, reputationReward);
            GenerateItemList();
        }
    }
}
