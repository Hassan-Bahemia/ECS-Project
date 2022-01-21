using Unity.Entities;
using Unity.Transforms;


public class WeaponSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem m_endSimulationEntityCommandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_endSimulationEntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var ecb = m_endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        Entities.ForEach((Entity _entity, int entityInQueryIndex, ref WeaponComponentData _weaponComponent,
            in Translation _translation,
            in Rotation _rotation) =>
        {
            _weaponComponent.m_timer += deltaTime;

            if (!_weaponComponent.m_isFiring) return;
            if (!(_weaponComponent.m_timer > _weaponComponent.m_fireRate)) return;

            _weaponComponent.m_timer = 0;
            var newProjectile = ecb.Instantiate(entityInQueryIndex, _weaponComponent.m_projectilePrefab);

            ecb.SetComponent(entityInQueryIndex, newProjectile, new Translation
            {
                Value = _translation.Value
            });

            ecb.SetComponent(entityInQueryIndex, newProjectile, new Rotation()
            {
                Value = _rotation.Value
            });

            ecb.SetComponent(entityInQueryIndex, newProjectile, new MovementCommandsComponentData()
            {
                m_currentlinearCommand = 1,
            });

            ecb.AddComponent(entityInQueryIndex, newProjectile, new ParticleEffectLink()
            {
                value = 1
            });

        }).ScheduleParallel();
        m_endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}

public struct ParticleEffectLink : IComponentData
{
    public int value;
}


public struct EffectIDSystemState : IComponentData
{
}