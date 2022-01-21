using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class OffScreenWrappingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var screenDataComponent = GetSingleton<ScreenInfoComponentData>();
        
        Entities.WithAll<OffScreenWrapperComponentData>().ForEach((
            Entity _entity, ref OffScreenWrapperComponentData _offScreenWrapperComponent, ref Translation _translation) => 
        {
            if (_offScreenWrapperComponent.m_isOffScreenLeft)
            {
                _translation.Value = SpawnOnRightSide(_translation.Value, _offScreenWrapperComponent.m_bounds, screenDataComponent);
            }
            else if (_offScreenWrapperComponent.m_isOffScreenRight)
            {
                _translation.Value = SpawnOnLeftSide(_translation.Value, _offScreenWrapperComponent.m_bounds, screenDataComponent);
            }
            else if (_offScreenWrapperComponent.m_isOffScreenUp)
            {
                _translation.Value = SpawnOnBottomSide(_translation.Value,_offScreenWrapperComponent.m_bounds, screenDataComponent);
            }
            else if (_offScreenWrapperComponent.m_isOffScreenDown)
            {
                _translation.Value = SpawnOnTopSide(_translation.Value,_offScreenWrapperComponent.m_bounds, screenDataComponent);
            }
            
            _offScreenWrapperComponent.m_isOffScreenDown = false;
            _offScreenWrapperComponent.m_isOffScreenRight = false;
            _offScreenWrapperComponent.m_isOffScreenUp = false;
            _offScreenWrapperComponent.m_isOffScreenLeft = false;
            
        }).ScheduleParallel();
    }

    private static float3 SpawnOnRightSide(float3 _position, float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return new float3((_bounds + _screenDataComponent.m_width)*.5f, _position.y, 0);;
    }
    private static float3 SpawnOnLeftSide(float3 _position,float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return new float3(-(_bounds + _screenDataComponent.m_width)*.5f,_position.y, 0);;
    }
    private static float3 SpawnOnTopSide(float3 _position,float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return  new float3(_position.x,(_bounds + _screenDataComponent.m_height)*.5f,0);
    }
    private static float3 SpawnOnBottomSide(float3 _position,float _bounds, ScreenInfoComponentData _screenDataComponent)
    {
        return new float3(_position.x, - (_bounds + _screenDataComponent.m_height)*.5f,0);
    }
}
