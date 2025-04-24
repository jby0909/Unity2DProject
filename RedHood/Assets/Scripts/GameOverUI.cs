using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverUI;

    public void Restart()
    {
        gameOverUI.SetActive(false);
        string currentScene = SceneManagerController.Instance.GetActiveScene();
        SceneManagerController.Instance.StartSceneTransition(currentScene);
    }

    public void GoToMenuScene()
    {
        gameOverUI.SetActive(false);
        SceneManagerController.Instance.StartSceneTransition("Menu");
    }
}
