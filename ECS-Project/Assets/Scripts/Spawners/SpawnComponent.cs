using Unity.Entities;

[GenerateAuthoringComponent]
public struct SpawnComponent : IComponentData
{
    public Entity spawnPrefab;
    public int multiplier;
}
