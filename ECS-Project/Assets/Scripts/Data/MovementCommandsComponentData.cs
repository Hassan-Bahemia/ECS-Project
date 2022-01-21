using Unity.Entities;
using Unity.Mathematics;


[GenerateAuthoringComponent]
public struct MovementCommandsComponentData : IComponentData
{
    public float3 m_currentDirectionOfMove;
    public float3 m_currentAngularCommand;
    public float m_currentlinearCommand;
    public float3 m_previousPosition;
}
