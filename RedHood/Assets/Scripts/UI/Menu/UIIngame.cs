using UnityEngine;
using UnityEngine.UI;

public class UIIngame : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Text breadCountText;
    [SerializeField] private GameObject[] mpIconArr;

    public void SetActivePanel(bool active)
    {
        if(panel.activeSelf == !active)
        {
            panel.SetActive(active);
        }
    }

    public void UpdateHpBar(float value)
    {
        hpBar.value = value;
    }
    public void UpdateBreadCount(int breadCount)
    {
        breadCountText.text = breadCount.ToString();
    }

    public void UpdateMP(int mpCount)
    {
        for (int i = 0; i < mpCount; i++)
        {
            mpIconArr[i].SetActive(true);
        }
        for (int i = mpCount; i < mpIconArr.Length; i++)
        {
            mpIconArr[i].SetActive(false);
        }
    }
}
