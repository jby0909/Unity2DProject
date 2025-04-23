using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    
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
