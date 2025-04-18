using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text damageText;
    public Text coinText;

    //�⺻ ���׷��̵� ���
    public int baseDamageCost = 25;
    public int baseMoveSpeedCost = 15;
    public int baseHpCost = 10;

    //���׷��̵� ��ġ
    public int damageUpgradeAmount = 5;
    public float moveSpeedUpgradeAmount = 0.3f;
    public int hpUpgradeAmount = 10;

    //���׷��̵� Ƚ�� ����
    private int damageUpgradeCount = 0;
    private int attackSpeedUpgradeCount = 0;
    private int moveSpeedUpgradeCount = 0;
    private int hpUpgradeCount = 0;

    //���� ��� ����
    private const int increaseThreshold = 3; // 3ȸ �̻��� �� ���� ����
    private const float priceIncreaseRate = 1.5f; // ��� * 1.5

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

        return Mathf.FloorToInt(baseCost * priceIncreaseRate); // �������� ���(�Ҽ��� ����)
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
