using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coinCount = 0;

    public int breadCount = 0;

    public Text coinText;
    public Text breadText;

    private const string COIN_KEY = "CoinCount";
    private const string DAMAGE_KEY = "PlayerDamage";
    private const string MOVE_SPEED_KEY = "PlayerMoveSpeed";
    private const string HP_KEY = "PlayerHp";
    private const string MP_KEY = "PlayerMp";
    private const string KILL_KEY = "PlayerKill";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        coinText.text = coinCount.ToString();
        SaveCoin();
        SoundManager.Instance.PlaySFX(SFXType.PlayerGetItem2);
        
    }

    public void AddBread()
    {
        breadCount++;
        breadText.text = breadCount.ToString();
        SoundManager.Instance.PlaySFX(SFXType.PlayerGetItem2);
    }

    public bool UseCoin(int amount)
    {
        if(coinCount >= amount)
        {
            coinCount -= amount;
            SaveCoin();
            return true;
        }
        Debug.Log("코인이 부족합니다.");
        return false;
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    private void SaveCoin()
    {
        PlayerPrefs.SetInt(COIN_KEY, coinCount);
        PlayerPrefs.Save();
    }

    private void LoadCoin()
    {
        coinCount = PlayerPrefs.GetInt(COIN_KEY, 0);
    }

    public void SavePlayerStats(PlayerStats stats)
    {
        PlayerPrefs.SetInt(DAMAGE_KEY, stats.damage);
        PlayerPrefs.SetFloat(HP_KEY, stats.maxHp);
        PlayerPrefs.SetInt(KILL_KEY, stats.minKill);
    }

    public void LoadPlayerState(PlayerStats stats)
    {
        if(PlayerPrefs.HasKey(DAMAGE_KEY))
        {
            stats.damage = PlayerPrefs.GetInt(DAMAGE_KEY);
        }
        if (PlayerPrefs.HasKey(HP_KEY))
        {
            stats.maxHp = PlayerPrefs.GetInt(HP_KEY);
        }
        if(PlayerPrefs.HasKey(KILL_KEY))
        {
            stats.minKill = PlayerPrefs.GetInt(KILL_KEY);
        }
        
    }

    public void ResetCoin()
    {
        coinCount = 0;
        coinText.text = coinCount.ToString();
        //PlayerPrefs.SetInt("Coin", coinCount);
    }
}
