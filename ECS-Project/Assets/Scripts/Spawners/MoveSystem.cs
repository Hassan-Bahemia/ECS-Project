using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Translation translation, in MoveComponent moveComponent) => {
                translation.Value.z -= moveComponent.speedValue * deltaTime;
                if (translation.Value.z < 0) translation.Value.z = 10f;
        }).Schedule();
    }
}
