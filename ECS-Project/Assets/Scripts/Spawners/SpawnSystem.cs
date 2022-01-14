using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class SpawnSystem : SystemBase
{
    protected override void OnStartRunning()
    {
        Entity prefab = GetSingleton<SpawnComponent>().spawnPrefab;
        int multiplier = GetSingleton<SpawnComponent>().multiplier;

        for (int i = 0; i < multiplier; i++)
        {
          Entity entity = EntityManager.Instantiate(prefab);
          EntityManager.SetComponentData(entity, new Translation
          {
              Value = new float3(
                  UnityEngine.Random.Range(-8f, 8f),
                  UnityEngine.Random.Range(-5f, 5f),
                  UnityEngine.Random.Range(0f, 10f))
          });
          EntityManager.SetComponentData(entity, new MoveComponent()
          {
              speedValue = UnityEngine.Random.Range(2f, 10f)
          });
        }
    }

    protected override void OnUpdate()
    {
        
    }
}
