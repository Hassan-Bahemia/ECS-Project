using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerLibraryComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public BootStrapper m_bootStrapper;
    public GameObject m_playerPrefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new PlayerLibraryElementComponentData
        {
            m_player = conversionSystem.GetPrimaryEntity(m_playerPrefab)
        });
        m_bootStrapper.m_playerEntity = entity;
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(m_playerPrefab);
    }
}

public struct PlayerLibraryElementComponentData : IComponentData
{
    public Entity m_player { get; set; }
}
