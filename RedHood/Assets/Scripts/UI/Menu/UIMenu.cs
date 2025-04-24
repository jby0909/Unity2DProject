using UnityEngine;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void SetActivePanel(bool active)
    {
        if(panel.activeSelf == !active)
        {
            panel.SetActive(active);
        }
    }
}
