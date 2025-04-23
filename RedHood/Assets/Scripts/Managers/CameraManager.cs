using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    
}
