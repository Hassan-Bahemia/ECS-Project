using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public class PlayerControlSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var query = GetEntityQuery(typeof(InputComponentData));
        var array = query.ToComponentDataArray<InputComponentData>(Allocator.TempJob);

        var inputData = array[0];

        Entities.WithAll<PlayerTagComponent>().ForEach((ref MovementCommandsComponentData _mccd) =>
        {
            var turningLeft = inputData._inputLeft ? 1 : 0;
            var turningRight = inputData._inputRight ? 1 : 0;

            var rotDir = turningLeft - turningRight;
            
            _mccd._curAngularCommand = new float3(0,0,rotDir);
            _mccd._curLinearCommand = inputData._inputForward ? 1 : 0;
        }).ScheduleParallel();

        array.Dispose();
    }
}
