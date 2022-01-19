using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;

public struct ShooterComponentData : IComponentData
{
    public BlobAssetReference<Collider> _colliderCast;
    public Entity _projectileEntity;
    public float3 _projectileToSpawnPos;
    public float _elapsedTime;
    public float _firingProjectileInterval;
    public float _projectileSpeed;
    public float _projectileLifeTime;
}
