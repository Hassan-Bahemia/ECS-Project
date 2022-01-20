using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerSetMoveDirectionToUpSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithAll<MovingUpDirectionComponentData>().ForEach((ref MovementCommandsComponentData _mccd, in Rotation _rot) =>
        {
            var direction = math.mul(_rot.Value, math.up());
            _mccd._curDirectionOfMovement = direction;
        }).Schedule();
    }
}
