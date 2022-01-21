using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ScreenInfoComponentData_Authoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Camera m_camera;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var bottomLeftCornerPosition = m_camera.ViewportToWorldPoint(new Vector3(0, 0));
        var topRightCornerPosition = m_camera.ViewportToWorldPoint(new Vector3(1, 1));
        
        var heightOfScreen = topRightCornerPosition.y - bottomLeftCornerPosition.y;
        var widthOfScreen = topRightCornerPosition.x - bottomLeftCornerPosition.x;
        var size = new float2(widthOfScreen, heightOfScreen);
        
        dstManager.AddComponentData(entity, new ScreenInfoComponentData
        {
            m_height = size.y,
            m_width = size.x
        });
    }
}
