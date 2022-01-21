
using Unity.Entities;
using UnityEngine;


public class InputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref InputComponentData _input) =>
        {
            _input.m_inputLeft = Input.GetKey(KeyCode.A);
            _input.m_inputRight = Input.GetKey(KeyCode.D);
            _input.m_inputForward = Input.GetKey(KeyCode.W);
            _input.m_inputShoot = Input.GetMouseButton(0);

        }).Run();
    }
}