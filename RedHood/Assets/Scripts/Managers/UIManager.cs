using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private UIMenu menu;
    [SerializeField] private UIIngame ingame;

    public UIMenu Menu => menu;
    public UIIngame Ingame => ingame;
    
    void Awake()
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

    public void OnMenu(bool active)
    {
        menu.SetActivePanel(active);
    }
    public void OnIngame(bool active)
    {
        ingame.SetActivePanel(active);
    }

}
