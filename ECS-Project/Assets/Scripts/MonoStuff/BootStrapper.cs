using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BootStrapper : MonoBehaviour
{
    public Entity m_asteroidLibrary;
    public Entity m_playerEntity;
    private EntityManager m_entityManager;

    public Transform[] m_asteroidsSpawnPositions;
    private Vector3[] m_spawnPositionsVectors;

    private float m_currentTimer;
    public float m_asteroidSpawnFrequency;
    public float m_increaseSpawnValue;

    public int m_entitiesSpawned;
    public int m_maxEntitiesCanBeSpawned;
    public int m_score;
    public int m_lives;

    private ValidateSpawnPosJob m_validateSpawnPosJob;
    private JobHandle m_jobHandle;

    public static BootStrapper m_bootstrapperInstance;

    public Text m_curFrequencyText;
    public Text m_entitiesSpawnedText;
    public Text m_livesText;
    public Text m_scoreText;
    public Text m_finalScoreText;

    public GameObject m_howToPlay;
    public GameObject m_GameOver;

    private void Awake()
    {
        m_bootstrapperInstance = this;
        
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        m_spawnPositionsVectors = new Vector3[m_asteroidsSpawnPositions.Length];
        for (int i = 0; i < m_spawnPositionsVectors.Length; i++)
        {
            m_spawnPositionsVectors[i] = m_asteroidsSpawnPositions[i].position;
        }
    }


    void Start()
    {
        m_jobHandle = new JobHandle();
        m_validateSpawnPosJob = new ValidateSpawnPosJob();
        m_entityManager.CreateEntity(typeof(InputComponentData));
        m_score = 0;
        m_lives = 3;
        m_GameOver.SetActive(false);
        m_howToPlay.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(SpawnPlayerAtPosition(Vector3.zero));
    }

    private void SpawnAsteroid()
    {
        var buffer = m_entityManager.GetBuffer<EntityBufferElement>(m_asteroidLibrary);
        var lengthOfBuffer = buffer.Length;
        var randomAsteroidIndex = UnityEngine.Random.Range(0, lengthOfBuffer);
        var newAsteroid = m_entityManager.Instantiate(buffer[randomAsteroidIndex].m_entity);

        var randomSpawnPositionIndex = UnityEngine.Random.Range(0, m_spawnPositionsVectors.Length);
        var spawnPosition = m_spawnPositionsVectors[randomSpawnPositionIndex];

        m_entityManager.SetComponentData(newAsteroid, new Translation()
        {
            Value = spawnPosition
        });

        var randomMoveDirection = math.normalize(new float3(UnityEngine.Random.Range(-.8f, .8f), UnityEngine.Random.Range(-.8f, .8f), 0));
        var randomRotation = math.normalize(new float3(UnityEngine.Random.value, UnityEngine.Random.value, 0));

        m_entityManager.SetComponentData(newAsteroid, new MovementCommandsComponentData()
        {
            m_previousPosition = spawnPosition,
            m_currentDirectionOfMove = randomMoveDirection,
            m_currentlinearCommand = 1,
            m_currentAngularCommand = randomRotation
        });
        m_entitiesSpawned++;
    }

    private void Update()
    {
        m_currentTimer += Time.deltaTime;

        m_curFrequencyText.text = "Asteroid Frequency Spawn Rate: " + m_asteroidSpawnFrequency;
        m_entitiesSpawnedText.text = "Asteroid Entities Spawned: " + m_entitiesSpawned;
        m_scoreText.text = "Score: " + m_score;
        m_livesText.text = "Lives: " + m_lives;

        if (m_howToPlay && Input.GetMouseButtonDown(1))
        {
            m_howToPlay.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            
            m_asteroidSpawnFrequency -= m_increaseSpawnValue;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            m_asteroidSpawnFrequency += m_increaseSpawnValue;
        }

        if (m_asteroidSpawnFrequency <= 0.12f)
        {
            m_asteroidSpawnFrequency = 0.12f;
        }
        

        if (m_currentTimer > m_asteroidSpawnFrequency && m_entitiesSpawned != m_maxEntitiesCanBeSpawned)
        {
            m_currentTimer = 0;
            SpawnAsteroid();
        }


        if (m_entitiesSpawned == m_maxEntitiesCanBeSpawned)
        {
            m_entitiesSpawned = m_maxEntitiesCanBeSpawned;
        }
        
        if (m_lives <= 0)
        {
            m_lives = 0;
            m_GameOver.SetActive(true);
            m_finalScoreText.text = "Final Score: " + m_score;
            Time.timeScale = 0f;
            if (Input.GetKeyDown(KeyCode.R))
            {
                m_lives = 3;
                m_score = 0;
                m_GameOver.SetActive(false);
                m_howToPlay.SetActive(true);
            }
        }
    }

    public void LookForPlayerSpawnPos()
    {
        var stillLookingForPos = true;
        var screenInfoQuery = m_entityManager.CreateEntityQuery(typeof(ScreenInfoComponentData));
        var screenInfoEntity = screenInfoQuery.GetSingletonEntity();
        var screenInfoComponent = m_entityManager.GetComponentData<ScreenInfoComponentData>(screenInfoEntity);

        var screenHalfWidth = screenInfoComponent.m_width * .5f;
        var screenHalfHeight = screenInfoComponent.m_height * .5f;

        var asteroidQuery = m_entityManager.CreateEntityQuery(typeof(AsteroidTagComponent), ComponentType.ReadOnly<Translation>());
        var translationComponentOfAllAsteroids = asteroidQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
        
        var isSpawnPosValid = new NativeArray<bool>(1, Allocator.TempJob);
        
        var possibleSpawnPos = new Vector3(UnityEngine.Random.Range(-screenHalfWidth, screenHalfWidth), UnityEngine.Random.Range(-screenHalfHeight, screenHalfHeight), 0);

        while (stillLookingForPos)
        {
            m_validateSpawnPosJob.m_translations = translationComponentOfAllAsteroids;
            m_validateSpawnPosJob.m_possibleSpawnPos = possibleSpawnPos;
            m_validateSpawnPosJob.m_minimalSpawnDist = 5f;
            m_validateSpawnPosJob.m_result = isSpawnPosValid;

            m_jobHandle = m_validateSpawnPosJob.Schedule();
            m_jobHandle.Complete();

            if (isSpawnPosValid[0]) {
                stillLookingForPos = false;
            } 
            else 
            {
              possibleSpawnPos = new Vector3(UnityEngine.Random.Range(-screenHalfWidth, screenHalfWidth), UnityEngine.Random.Range(-screenHalfHeight, screenHalfHeight), 0);  
            }
        }

        isSpawnPosValid.Dispose();
        translationComponentOfAllAsteroids.Dispose();
        StartCoroutine(SpawnPlayerAtPosition(possibleSpawnPos));
    }

    private IEnumerator SpawnPlayerAtPosition(Vector3 _spawnPos)
    {
        yield return new WaitForSeconds(1f);
        var playerLibraryRef = m_entityManager.GetComponentData<PlayerLibraryElementComponentData>(m_playerEntity);
        var playerPrefab = m_entityManager.Instantiate(playerLibraryRef.m_player);
        m_entityManager.SetComponentData(playerPrefab, new Translation
        {
            Value = _spawnPos
        });
    }
    
    public struct ValidateSpawnPosJob : IJob
    {
        public NativeArray<Translation> m_translations;
        public float3 m_possibleSpawnPos;
        public float m_minimalSpawnDist;
        public NativeArray<bool> m_result;
        
        public void Execute()
        {
            var result = true;
            foreach (var translation in m_translations)
            {
                if (math.distancesq(translation.Value, m_possibleSpawnPos) < m_minimalSpawnDist * m_minimalSpawnDist) {
                    result = false;
                    break;
                }
            }

            m_result[0] = result;
        }
    }
}
