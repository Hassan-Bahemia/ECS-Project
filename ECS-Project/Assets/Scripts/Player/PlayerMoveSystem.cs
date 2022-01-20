using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;


public class PlayerMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref MovementCommandsComponentData _mccd, ref PhysicsVelocity _pv,
            in MovementParamComponentData _mpcd, in PhysicsMass _pm) =>
        {
           PhysicsComponentExtensions.ApplyLinearImpulse(ref _pv, _pm, _mccd._curDirectionOfMovement * _mccd._curLinearCommand * _mpcd._linearVelocity);
           if (math.length(_pv.Linear) > _mpcd._maxLinearVelocity) {
               _pv.Linear = math.normalize(_pv.Linear) * _mpcd._maxLinearVelocity;
           }
        }).ScheduleParallel();
    }
}
