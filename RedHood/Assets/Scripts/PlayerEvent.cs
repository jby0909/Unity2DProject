using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
    public GameObject ArrowKeyObj;
    public GameObject SpaceKeyObj;
    public GameObject LeftMouseObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "TutorialEvent1")
        {
            ArrowKeyObj.SetActive(true);
        }
        else if(collision.name == "TutorialEvent2")
        {
            SpaceKeyObj.SetActive(true);
        }
        else if(collision.name == "TutorialEvent3")
        {
            LeftMouseObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name == "TutorialEvent1")
        {
            ArrowKeyObj.SetActive(false);
        }
        else if (collision.name == "TutorialEvent2")
        {
            SpaceKeyObj.SetActive(false);
        }
        else if (collision.name == "TutorialEvent3")
        {
            LeftMouseObj.SetActive(false);
        }
    }
}
