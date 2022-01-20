using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BootStrapper : MonoBehaviour
{
    private EntityManager _em;

    private void Awake()
    {
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Start()
    {
        _em.CreateEntity(typeof(InputComponentData));
    }
}
