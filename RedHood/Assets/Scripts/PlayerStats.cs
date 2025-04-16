using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("플레이어 능력치")]
    public int maxHp = 100;
    public int currentHp;
    public int damage = 10;
    public int maxMp = 3;
    public int currentMp;
    public int kill = 0;
    public int minKill = 5;

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
        currentHp = maxHp;
        currentMp = 0;
    }

    void Start()
    {
        GameManager.Instance.LoadPlayerState(this);
    }


    public void TakeDamage(int amount)
    {
        SoundManager.Instance.PlaySFX(SFXType.PlayerDamage);
        currentHp -= amount;
        if(currentHp < 0)
        {
            //죽음
        }
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }

    public void Die()
    {
        //GameOver창 시작
    }

    public int Getdamage()
    {
        return damage;
    }

    public int Getkill()
    {
        return kill;
    }

    public void UpgradeDamage(int amount)
    {
        damage += amount;
        GameManager.Instance.SavePlayerStats(this);
    }

    public void UpgradeHp(int amount)
    {
        maxHp += amount;
        GameManager.Instance.SavePlayerStats(this);
    }

    public void UpgradeKill(int amount)
    {
        minKill += amount;
        GameManager.Instance.SavePlayerStats(this);
    }
}
