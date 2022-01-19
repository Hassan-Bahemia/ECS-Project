using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;

public class EnemySystem : SystemBase
{
    public float3 _shooterPos;

    protected override void OnCreate()
    {
        _shooterPos = float3.zero;
    }

    protected override void OnUpdate()
    {
        Entities.WithoutBurst().ForEach((in ShooterComponentData scd, in Translation trans) =>
        {
            _shooterPos = trans.Value;
        }).Run();

        float3 sh = _shooterPos;
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref EnemyComponentData ecd, ref Translation trans, ref Rotation rot) =>
        {
            float3 diff = math.normalize(sh - trans.Value);
            rot.Value = math.slerp(rot.Value, quaternion.LookRotation(diff, math.up()), deltaTime);
            trans.Value += diff * ecd._speed * deltaTime;
        }).ScheduleParallel();
    }
}
