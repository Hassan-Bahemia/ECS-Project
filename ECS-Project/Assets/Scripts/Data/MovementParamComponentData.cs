using Unity.Entities;

[GenerateAuthoringComponent]
public struct MovementParamComponentData : IComponentData
{
    public float _linearVelocity;
    public float _maxLinearVelocity;
    public float _angularVelocity;
    public float _maxAngularVelocity;
}
