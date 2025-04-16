using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public void Restart()
    {
        string currentScene = SceneManagerController.Instance.GetActiveScene();
        SceneManagerController.Instance.StartSceneTransition(currentScene);
    }

    public void GoToMenuScene()
    {
        SceneManagerController.Instance.StartSceneTransition("Menu");
    }
}
