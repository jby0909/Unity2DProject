using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text damageText;
    public Text coinText;

    //기본 업그레이드 비용
    public int baseDamageCost = 25;
    public int baseMoveSpeedCost = 15;
    public int baseHpCost = 10;

    //업그레이드 수치
    public int damageUpgradeAmount = 5;
    public float moveSpeedUpgradeAmount = 0.3f;
    public int hpUpgradeAmount = 10;

    //업그레이드 횟수 추적
    private int damageUpgradeCount = 0;
    private int attackSpeedUpgradeCount = 0;
    private int moveSpeedUpgradeCount = 0;
    private int hpUpgradeCount = 0;

    //가격 상승 조건
    private const int increaseThreshold = 3; // 3회 이상일 때 가격 증가
    private const float priceIncreaseRate = 1.5f; // 비용 * 1.5

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = PlayerStats.Instance;
    }

    private int GetCost(int baseCost, int upgradeCount)
    {
        if(upgradeCount < increaseThreshold)
        {
            return baseCost;
        }

        return Mathf.FloorToInt(baseCost * priceIncreaseRate); // 가격증가 계산(소수점 버림)
    }

    public void UpgradeDamage()
    {
        UpgradeUI();
        SoundManager.Instance.PlaySFX(SFXType.UIBuy);
        int cost = GetCost(baseDamageCost, damageUpgradeCount);
        if(GameManager.Instance.UseCoin(cost))
        {
            playerStats.UpgradeDamage(damageUpgradeAmount);
            damageUpgradeCount++;
        }
    }

    public void UpgradeHp()
    {
        UpgradeUI();
        SoundManager.Instance.PlaySFX(SFXType.UIBuy);
        int cost = GetCost(baseHpCost, hpUpgradeCount);
        if (GameManager.Instance.UseCoin(cost))
        {
            playerStats.UpgradeHp(hpUpgradeAmount);
            hpUpgradeCount++;
        }
    }


    public void UpgradeUI()
    {
        damageText.text = playerStats.damage.ToString();
        coinText.text = GameManager.Instance.coinCount.ToString();
    }
}
