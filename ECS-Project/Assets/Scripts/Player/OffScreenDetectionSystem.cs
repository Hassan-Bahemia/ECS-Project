using Unity.Entities;
using Unity.Transforms;

public class OffScreenDetectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var screenData = GetSingleton<ScreenInfoComponentData>();

        Entities.ForEach(
            (Entity _entity, ref OffScreenWrapperComponentData _offScreen,
                in MovementCommandsComponentData _moveComponent, in Translation _translation) =>
            {
                var previousPosition = _moveComponent.m_previousPosition;
                var currentPosition = _translation.Value;

                var isMovingLeft = currentPosition.x - previousPosition.x < 0;
                var isMovingRight = currentPosition.x - previousPosition.x > 0;
                var isMovingUp = currentPosition.y - previousPosition.y > 0;
                var isMovingDown = currentPosition.y - previousPosition.y < 0;

                _offScreen.m_isOffScreenLeft = _translation.Value.x < -(screenData.m_width + _offScreen.m_bounds) * .5f && isMovingLeft;
                _offScreen.m_isOffScreenRight = _translation.Value.x > (screenData.m_width + _offScreen.m_bounds) * .5f && isMovingRight;

                _offScreen.m_isOffScreenUp = _translation.Value.y > (screenData.m_height + _offScreen.m_bounds) * .5f && isMovingUp;
                _offScreen.m_isOffScreenDown = _translation.Value.y < -(screenData.m_height + _offScreen.m_bounds) * .5f && isMovingDown;
                
            }).ScheduleParallel();
    }
}
