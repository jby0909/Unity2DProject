using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
    public GameObject ArrowKeyObj;
    public GameObject SpaceKeyObj;
    public GameObject LeftMouseObj;
    public GameObject RightMouseObj;

    private void Start()
    {
        PlayerController.Instance.attack.onPlayerMpTutorial += OnPlayerMpFullTutorialUI;
    }
    public void OnPlayerMpFullTutorialUI()
    {
        RightMouseObj?.SetActive(true);
        PlayerController.Instance.attack.onPlayerMpTutorial -= OnPlayerMpFullTutorialUI;
        PlayerController.Instance.attack.onPlayerMpTutorial += OnPlayerMpUseTutorialUI;
    }

    public void OnPlayerMpUseTutorialUI()
    {
        RightMouseObj?.SetActive(false);
        PlayerController.Instance.attack.onPlayerMpTutorial -= OnPlayerMpUseTutorialUI;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "TutorialEvent1")
        {
            ArrowKeyObj?.SetActive(true);
        }
        else if(collision.name == "TutorialEvent2")
        {
            SpaceKeyObj?.SetActive(true);
        }
        else if(collision.name == "TutorialEvent3")
        {
            LeftMouseObj?.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name == "TutorialEvent1")
        {
            ArrowKeyObj?.SetActive(false);
        }
        else if (collision.name == "TutorialEvent2")
        {
            SpaceKeyObj?.SetActive(false);
        }
        else if (collision.name == "TutorialEvent3")
        {
            LeftMouseObj?.SetActive(false);
        }
    }
}
