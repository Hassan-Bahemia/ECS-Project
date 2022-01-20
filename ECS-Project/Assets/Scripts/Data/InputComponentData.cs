using Unity.Entities;

public struct InputComponentData : IComponentData
{
    public bool _inputLeft;
    public bool _inputRight;
    public bool _inputForward;
}
