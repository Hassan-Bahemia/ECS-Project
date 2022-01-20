using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

public class PlayerRotateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PhysicsVelocity _pv, ref MovementCommandsComponentData _mccd, ref Rotation _rot,
            in MovementParamComponentData _mpcd, in PhysicsMass _pm) =>
        {
            PhysicsComponentExtensions.ApplyAngularImpulse(ref _pv, _pm, _mccd._curAngularCommand * _mpcd._angularVelocity);

            var curAngularSpeed = PhysicsComponentExtensions.GetAngularVelocityWorldSpace(in _pv, in _pm, in _rot);

            if (math.length(curAngularSpeed) > _mpcd._maxAngularVelocity) {
                PhysicsComponentExtensions.SetAngularVelocityWorldSpace(ref _pv, _pm, _rot, math.normalize(curAngularSpeed) * _mpcd._maxAngularVelocity);
            }
        }).Schedule();
    }
}
