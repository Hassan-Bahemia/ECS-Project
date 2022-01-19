using System;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EnemySpawner : MonoBehaviour
{
    [Header("Public Variables for Enemy")]
    public GameObject _enemyPrefabToSpawn;
    public static Entity _convertedEnemyEntity;
    public int _maxEnemyEntitiesToSpawn;
    public int _enemyEntitiesPerInterval;
    public float _interval;
    public float _minEnemySpeed;
    public float _maxEnemySpeed;
    public float minSpawnPos;
    public float maxSpawnPos;

    [Header("Private Variables for Enemy")]
    [SerializeField] private int _spawnedEnemyEntities;
    [SerializeField] private float3 _position;
    [SerializeField] private float _elapsedTime;
    [SerializeField] private EntityManager _em;
    [SerializeField] private BlobAssetStore _bas;

    private void Start()
    {
        _elapsedTime = 0;
        _spawnedEnemyEntities = 0;
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;
        _bas = new BlobAssetStore();
        GameObjectConversionSettings gocs = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _bas);
        _convertedEnemyEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(_enemyPrefabToSpawn, gocs);
        _em.AddComponent<EnemyComponentData>(_convertedEnemyEntity);
        _em.SetComponentData(_convertedEnemyEntity, new EnemyComponentData
        {
            _speed = UnityEngine.Random.Range(_minEnemySpeed, _maxEnemySpeed)
        });
    }

    private void Update()
    {
        if (_spawnedEnemyEntities < _maxEnemyEntitiesToSpawn)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _interval)
            {
                _elapsedTime = 0;
                for (int i = 0; i <= _enemyEntitiesPerInterval; i++)
                {
                    if (_spawnedEnemyEntities >= _maxEnemyEntitiesToSpawn)
                    {
                        break;
                    }
                    _position = (float3) transform.position + new float3(UnityEngine.Random.Range(minSpawnPos, maxSpawnPos) * i, 0, UnityEngine.Random.Range(minSpawnPos, maxSpawnPos));
                    _em.Instantiate(_convertedEnemyEntity);
                    _em.AddComponent<EnemyComponentData>(_convertedEnemyEntity);
                    _em.SetComponentData(_convertedEnemyEntity, new Translation {Value = _position});
                    _em.SetComponentData(_convertedEnemyEntity, new EnemyComponentData
                    {
                        _speed = UnityEngine.Random.Range(_minEnemySpeed, _maxEnemySpeed)
                    });
                    _spawnedEnemyEntities++;
                }
            }
        }
    }

    private void OnDestroy()
    {
        _bas.Dispose();
    }
}
