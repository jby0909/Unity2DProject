using Unity.Cinemachine;
using UnityEngine;

public class CinemachineBoundingShape : MonoBehaviour
{
    public PolygonCollider2D[] colliders;
    CinemachineConfiner2D CinemachineConfiner2D;
    void Start()
    {
        CinemachineConfiner2D = gameObject.GetComponent<CinemachineConfiner2D>();
        SceneManagerController.Instance.sceneOnLoad += ChangeCollider;
    }

    
    public void ChangeCollider()
    {
        CinemachineConfiner2D.BoundingShape2D = colliders[SceneManagerController.Instance.sceneNamesIndex];
    }
}
