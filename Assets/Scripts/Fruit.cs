using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType
{
    Apple,
    //Banana,
    Blueberry,
    Lemon,
    Orange,
    //Pear,
    //Pineapple,
    //Strawberry,
    //Watermelon
}

/// <summary>
/// Keeps track of what type of fruit it is.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Fruit : MonoBehaviour
{
    public FruitType fruitType { get; private set; }

    private void OnEnable()
    {
        fruitType = (FruitType)Random.Range(0, System.Enum.GetValues(typeof(FruitType)).Length);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Basket"))
        {
            Destroy(gameObject);
        }
    }
}
