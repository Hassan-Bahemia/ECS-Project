using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

public class MoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((
            ref MovementCommandsComponentData _movementCommandsComponentData, ref PhysicsVelocity _velocity,
            in MovementParametersComponentData _movementParametersComponentData,
            in PhysicsMass _physicsMass, in Translation _translation) =>
        {
            _movementCommandsComponentData.m_previousPosition = _translation.Value;

            PhysicsComponentExtensions.ApplyLinearImpulse(
                ref _velocity,
                _physicsMass,
                _movementCommandsComponentData.m_currentDirectionOfMove *
                _movementCommandsComponentData.m_currentlinearCommand *
                _movementParametersComponentData.m_linearVelocity
            );

            if (math.length(_velocity.Linear) > _movementParametersComponentData.m_maxLinearVelocity)
                _velocity.Linear = math.normalize(_velocity.Linear) *
                                   _movementParametersComponentData.m_maxLinearVelocity;

        }).ScheduleParallel();

    }
}