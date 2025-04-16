using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource; // 배경음 재생용 AudioSource
    public AudioSource sfxSource; // 효과음 재생용 AudioSource

    public Dictionary<BGMType, AudioClip> bgmDic = new Dictionary<BGMType, AudioClip>(); 
    public Dictionary<SFXType, AudioClip> sfxDic = new Dictionary<SFXType, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    //게임 시작 시 자동으로 실행되는 초기화 함수
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitSoundManager()
    {
        GameObject obj = new GameObject("SoundManger");
        Instance = obj.AddComponent<SoundManager>();
        DontDestroyOnLoad(obj);

        //BGM 설정
        GameObject bgmObj = new GameObject("BGM");
        SoundManager.Instance.bgmSource = bgmObj.AddComponent<AudioSource>();
        bgmObj.transform.SetParent(obj.transform);
        SoundManager.Instance.bgmSource.loop = true; //BGM은 무한반복
        SoundManager.Instance.bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);

        //SFX 설정
        GameObject sfxObj = new GameObject("SFX");
        SoundManager.Instance.sfxSource = sfxObj.AddComponent<AudioSource>();
        SoundManager.Instance.sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        sfxObj.transform.SetParent(obj.transform);

        //BGM 리소스 로드
        //오디오클립이 많으면 로드할 때 오래걸리는 단점이 있음
        AudioClip[] bgmClips = Resources.LoadAll<AudioClip>("Sound/BGM");

        foreach(var clip in bgmClips)
        {
            try
            {
                BGMType type = (BGMType)Enum.Parse(typeof(BGMType), clip.name);
                SoundManager.Instance.bgmDic.Add(type, clip);
            }
            catch
            {
                Debug.LogWarning("BGM Enum 필요 : " + clip.name);
            }
        }

        //SFX 리소스 로드
        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("Sound/SFX");
        foreach(var clip in sfxClips)
        {
            try
            {
                SFXType type = (SFXType)Enum.Parse(typeof(SFXType), clip.name);
                SoundManager.Instance.sfxDic.Add(type, clip);
            }
            catch
            {
                Debug.LogWarning("SFX Enum 필요 : " + clip.name);
            }
        }

        // 씬 로드시마다 OnSceneLoadCompleted 호출
        SceneManager.sceneLoaded += SoundManager.Instance.OnSceneLoadCompleted;
    }

    // 씬 전환 완료 시 자동 호출되는 함수
    public void OnSceneLoadCompleted(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Tutorial")
        {
            PlayBGM(BGMType.Tutorial, 1f);
        }
        else if(scene.name == "Boss")
        {
            PlayBGM(BGMType.Boss, 1f);
        }
        else if(scene.name == "Menu")
        {
            PlayBGM(BGMType.Menu, 1f);
        }
    }

    public void PlaySFX(SFXType type) //효과음 재생
    {
        sfxSource.PlayOneShot(sfxDic[type]);
    }

    public void PlayBGM(BGMType type, float fadeTime = 0) // 배경음 재생함수(페이드효과 포함)
    {
        if(bgmSource.clip != null)
        {
            if(bgmSource.clip.name == type.ToString())
            {
                return;
            }
            if(fadeTime == 0)
            {
                bgmSource.clip = bgmDic[type];
                bgmSource.Play();
            }
            else
            {
                StartCoroutine(FadeOutBGM(() =>
                {
                    bgmSource.clip = bgmDic[type];
                    bgmSource.Play();
                    StartCoroutine(FadeInBGM(fadeTime));
                }, fadeTime));
            }
        }
        else
        {
            if(fadeTime == 0)
            {
                bgmSource.clip = bgmDic[type];
                bgmSource.Play();
            }
            else
            {
                bgmSource.volume = 0;
                bgmSource.clip = bgmDic[type];
                bgmSource.Play();
                StartCoroutine(FadeInBGM(fadeTime));

            }
        }
    }

    //BGM볼륨을 천천히 줄이는 코루틴
    private IEnumerator FadeOutBGM(Action onComplete, float duration)
    {
        float startVolume = bgmSource.volume;
        float time = 0;

        while(time < duration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bgmSource.volume = 0f;
        onComplete?.Invoke(); // 페이드 아웃 후 콜백 진행
    }

    //BGM볼륨을 천천히 올리는 코루틴
    private IEnumerator FadeInBGM(float duration = 1.0f)
    {
        float targetVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        float time = 0f;

        while(time < duration)
        {
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    //BGM 볼륨 설정
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    //SFX 볼륨 설정
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }


}

//BGM 종류
public enum BGMType
{
    Tutorial,
    Boss,
    Menu
}


//SFX 종류
public enum SFXType
{
    PlayerAttack,
    PlayerDamage,
    PlayerJump,
    PlayerGetItem,
    PlayerGetItem2,
    PlayerFootStep,
    PlayerDead,
    EnemyAttack,
    EnemyDamage,
    UIPause,
    UIUnpause,
    UIBuy
}
