using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerController : MonoBehaviour
{
    public static SceneManagerController Instance { get; private set; }

    public Image panel;
    public float fadeDuration = 1.0f;
    public string nextSceneName;
    public string currentSceneName;
    private bool isFading = false;

    private string[] nextSceneNames = {"Tutorial", "Stage1", "Stage2", "GameClear","Menu" };
    public int NextSceneNamesIndex = 0;

    public bool isTutorial = false;

    
    public GameObject menuUI;
    public GameObject inGameUI;
    public GameObject mainCamera;
    public GameObject cinemachineCamera;
    public GameObject player;

    public event Action sceneOnLoad;

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

    public void StartSceneTransition(string sceneName)
    {
        if(!isFading)
        {
            nextSceneName = sceneName;
            currentSceneName = GetActiveScene();
            StartCoroutine(FadeInAndLoadScene());
           
            
            
        }
    }

    IEnumerator FadeInAndLoadScene()
    {
        isFading = true;
        
        panel.gameObject.SetActive(true);

        yield return StartCoroutine(FadeImage(0, 1, fadeDuration));

        yield return StartCoroutine(LoadLoadingAndNextScene());

        if (nextSceneName.CompareTo("Menu") == 0)
        {
            isTutorial = false;
            NextSceneNamesIndex = 0;
            GameManager.Instance.currentStageLevel = 0;

            PlayerController.Instance.health.ResetHp();
            PlayerController.Instance.ResetDead();
            PlayerController.Instance.attack.ResetMp();
            PlayerController.Instance.animation.ResetAnimation();
            PlayerController.Instance.movement.ResetPosition();
            GameManager.Instance.ResetBread();
            UIManager.Instance.OnMenu(true);
            UIManager.Instance.OnIngame(false);
            cinemachineCamera?.SetActive(false);
            player?.SetActive(true);
            //모든 값 리셋
            

        }
        else
        {
            if (nextSceneName.CompareTo("Tutorial") == 0)
            {
                isTutorial = true;
            }
            else
            {
                isTutorial = false;
            }

            if (nextSceneName.CompareTo(currentSceneName) != 0)
            {
                GameManager.Instance.currentStageLevel++;
                NextSceneNamesIndex = (NextSceneNamesIndex + 1) % nextSceneNames.Length;

            }
            else
            {
                //만약 restart 라면(다음 씬과 현재 씬이 같다면)
                //모든 값 리셋
                PlayerController.Instance.health.ResetHp();
                PlayerController.Instance.ResetDead();
                PlayerController.Instance.attack.ResetMp();
                PlayerController.Instance.animation.ResetAnimation();
                if(GameManager.Instance.currentStageLevel - 2 >= 0)
                {
                    GameManager.Instance.ResetBread(GameManager.Instance.stageDataDict[GameManager.Instance.currentStageLevel - 2].breadCount);
                }
                else
                {
                    GameManager.Instance.ResetBread();
                }
                

            }

            UIManager.Instance.OnMenu(false);
            UIManager.Instance.OnIngame(true);
            cinemachineCamera.SetActive(true);
            player.SetActive(true);
            //플레이어 위치 시작부분으로 리셋
            PlayerController.Instance.movement.ResetPosition();
        }
        nextSceneName = nextSceneNames[NextSceneNamesIndex];

        if(sceneOnLoad != null)
        {
            sceneOnLoad.Invoke();
        }
        
        yield return StartCoroutine(FadeImage(1, 0, fadeDuration));

        isFading = false;
        //패널이 켜져있으면 유아이 클릭이 막히기 때문에 페이드아웃 후 패널을 비활성화함
        panel.gameObject.SetActive(false);


        
    }

    IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color panelColor = panel.color;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            panelColor.a = newAlpha;
            panel.color = panelColor;
            yield return null;
        }

        panelColor.a = endAlpha;
        panel.color = panelColor;
    }

    IEnumerator LoadLoadingAndNextScene()
    {
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        loadingSceneOp.allowSceneActivation = false;

        while(!loadingSceneOp.isDone)
        {
            if(loadingSceneOp.progress >= 0.9f)
            {
                loadingSceneOp.allowSceneActivation = true;
            }
            yield return null;
        }

        Slider loadingSlider = null;
        GameObject sliderObj = GameObject.Find("LoadingSlider");
        if (sliderObj != null)
        {
            loadingSlider = sliderObj.GetComponent<Slider>();
        }

        AsyncOperation nextSceneOp = SceneManager.LoadSceneAsync(nextSceneName);
        while(!nextSceneOp.isDone)
        {
            if(loadingSlider != null)
            {
                loadingSlider.value = nextSceneOp.progress;
            }
            yield return null;

        }
        if(SceneManager.GetSceneByName("LoadingScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("LoadingScene");
        }
        else
        {
            Debug.Log("로딩씬 not Load");
        }
        

    }

    public string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void ExitScene()
    {
        Application.Quit();
    }

    public void StartGameMenu()
    {
        StartSceneTransition("Tutorial");
    }

}
