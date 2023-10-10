using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector] public bool IsInPlay { get; private set; }
    [HideInInspector] public int Profit { get; private set; }
    [HideInInspector] public int Reputation { get; private set; }

    [Header("Fruit Settings")]

    [Tooltip("The minimum time between fruit spawns.")]
    [Range(0.1f, 5f)]
    [SerializeField] private float minSpawnTime = 1f;

    [Tooltip("Time interval between min and max spawn times.")]
    [Range(0f, 10f)]
    [SerializeField] private float spawnTimeRange = 2f;

    [SerializeField] private Transform fruitsParent;
    [SerializeField] private GameObject[] fruitPrefabs;
    [SerializeField] private Transform[] spawnPoints;


    [Space(5)]

    [Header("Input References")]

    [SerializeField] private InputActionProperty resetAction;

    [Space(5)]

    [Header("Other References")]

    [SerializeField] string mainMenuScene = "Main Menu";
    [SerializeField] string gameOverScene = "Game Over";
    
    public void CompleteBasket(int profit, int reputation)
    {
        Profit += profit;
        Reputation += reputation;
    }

    public void IncompleteBasket(int reputation)
    {
        Reputation += reputation;
    }

    public void GameOver()
    {
        IsInPlay = false;

        SceneManager.LoadScene(gameOverScene);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy this instance if another one already exists
        }

        IsInPlay = true;
    }

    private void Start()
    {
        StartCoroutine(SpawnFruit());
    }

    private void Update()
    {
        if (resetAction.action.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator SpawnFruit()
    {
        while (IsInPlay)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, minSpawnTime + spawnTimeRange));

            int spawnIndex = Random.Range(0, spawnPoints.Length);
            int fruitIndex = Random.Range(0, fruitPrefabs.Length);

            Instantiate(fruitPrefabs[fruitIndex], spawnPoints[spawnIndex].position, Quaternion.identity, fruitsParent);
        }
    }
}