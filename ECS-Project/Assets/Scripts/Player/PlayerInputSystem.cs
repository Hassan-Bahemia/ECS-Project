
using Unity.Entities;
using UnityEngine;

public class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref InputComponentData _inputData) =>
        {
            _inputData._inputLeft = Input.GetKey(KeyCode.A);
            _inputData._inputRight = Input.GetKey(KeyCode.D);
            _inputData._inputForward = Input.GetKey(KeyCode.W);
        }).Run(); 
    }
}
