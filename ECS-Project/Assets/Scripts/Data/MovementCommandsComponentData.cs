using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MovementCommandsComponentData : IComponentData
{
    public float3 _curDirectionOfMovement;
    public float3 _curAngularCommand;
    public float _curLinearCommand;
}
