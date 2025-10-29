using System.Collections;
using UnityEngine;
using Assets.Scripts.Feed;
namespace Assets.Scripts.Feed_Scene.newFeed
{
    public class EntityUI_Specs : MonoBehaviour
    {
        [SerializeField] GameObject EntitySpecs;

        [Header("ObjectRelatedComponents")]
        [SerializeField] Transform lookAt;
        [SerializeField] Vector3 offset;
        [SerializeField]CameraManager cameraManager;  

        public void MoveUIObjectWithMainObject()
        {
            Vector3 pos = cameraManager.GetComponent<Camera>().WorldToScreenPoint(lookAt.position + offset);
            EntitySpecs.transform.position = Vector3.Lerp(EntitySpecs.transform.position,
                pos, 0.5f * Time.fixedDeltaTime);
        }
    }
}