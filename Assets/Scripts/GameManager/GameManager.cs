using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player Properties")]
    public GameObject playerReference;
    public GameObject playerPrefab;

    [Header("Spawn Properties")]
    [SerializeField] private Transform initialSpawnPoint;

    [Header("Enemies Spawn Properties")]
    public List<GameObject> weakEnemies;
    public List<GameObject> strongEnemies;
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private float spawnInterval = 5f; // Intervalo de tiempo para spawnear enemigos
    private float nextSpawnTime = 0f;
    [SerializeField] private Transform finalBossSpawnPoint;
    [SerializeField] private GameObject finalBossPrefab;
    bool finalBossSpawned = false;

    [Header("Kill Properties")]
    public int killCounter;

    [Header("Time Properties")]
    public float timeTimer;
    public bool startCountdown;

    [Header("On Finish Properties")]
    [SerializeField] private GameObject passGO;
    [SerializeField] private GameObject doorColliderGO;

    [Header("On Player Death Properties")]
    [SerializeField] private GameObject gameOverCanvas;

    [SerializeField] bool isLevelLoaded;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameLobby") return;

        InitializeLevel();
        isLevelLoaded = true;
    }

    private void InitializeLevel()
    {
        initialSpawnPoint = GameObject.FindGameObjectWithTag("InitialSpawnPoint").transform;
        timeTimer = 60;
        startCountdown = true;

        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint").ToList();
        passGO = GameObject.FindGameObjectWithTag("LayerCollider").gameObject.GetComponent<LayerCollider>().passGO;
        doorColliderGO = GameObject.FindGameObjectWithTag("LayerCollider").gameObject.GetComponent<LayerCollider>().doorColliderGO;
        finalBossSpawnPoint = GameObject.FindGameObjectWithTag("FinalBossSpawnPoint").transform;

        GameObject go = Instantiate(playerPrefab, initialSpawnPoint);
        playerReference = go;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountdown)
            StartTimer();

        // Verifica si es tiempo de spawnear enemigos
        if (Time.time >= nextSpawnTime)
        {
            EnemySpawner();
            nextSpawnTime = Time.time + spawnInterval;
        }

        if (playerReference != null)
        {
            if (playerReference.transform.position.y >= 4.91f)
            {
                passGO.SetActive(true);
                doorColliderGO.SetActive(true);

                if (!finalBossSpawned)
                {
                    finalBossSpawned = true;
                    GameObject go = Instantiate(finalBossPrefab, finalBossSpawnPoint);
                    Enemy enemy = go.GetComponent<Enemy>();

                    enemy.enemyHealth = playerReference.GetComponent<PlayerCombat>().health;
                    enemy.damage = playerReference.GetComponent<PlayerCombat>().damage;
                    enemy.movementSpeed = playerReference.GetComponent<PlayerMovement>().movementSpeed;
                }

                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 9.58f, Camera.main.transform.position.z);
            }
        }
    }

    private void StartTimer()
    {
        timeTimer -= Time.deltaTime;

        if (timeTimer > 0) return;

        if (timeTimer < 0)
        {
            timeTimer = 0;
        }

        passGO.SetActive(false);
        doorColliderGO.SetActive(false);
    }

    private void EnemySpawner()
    {
        if (timeTimer <= 0)
        {
            // Dejar de spawnear
            return;
        }
        else if (timeTimer > 0 && timeTimer <= 10)
        {
            // Dejar de spawnear
            return;
        }
        else if (timeTimer > 10 && timeTimer <= 30)
        {
            // Alternarse entre spawnear enemigo fuerte y enemigo débil cada 5 segundos, con mayor probabilidad de enemigos fuertes
            float chance = Random.Range(0f, 1f);
            if (chance < 0.7f)
            {
                SpawnEnemy(strongEnemies);
            }
            else
            {
                SpawnEnemy(weakEnemies);
            }
        }
        else if (timeTimer > 30 && timeTimer <= 40)
        {
            spawnInterval = 2;

            // Alternarse entre spawnear enemigo fuerte y enemigo débil cada 5 segundos
            if (Random.Range(0, 2) == 0)
            {
                SpawnEnemy(strongEnemies);
            }
            else
            {
                SpawnEnemy(weakEnemies);
            }
        }
        else if (timeTimer > 40 && timeTimer <= 60)
        {
            // Spawnear enemigo débil cada 5 segundos
            SpawnEnemy(weakEnemies);
        }
    }

    private void SpawnEnemy(List<GameObject> enemyList)
    {
        // Seleccionar un prefab de enemigo aleatorio
        GameObject randomEnemyPrefab = enemyList[Random.Range(0, enemyList.Count)];

        // Seleccionar un punto de aparición aleatorio
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;

        // Instanciar el enemigo en el punto de aparición seleccionado
        Instantiate(randomEnemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
    }

    public void OnPlayerDeath()
    {
        gameOverCanvas.SetActive(true);
    }
}
