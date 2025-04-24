using UnityEngine;


public enum EMonsterType
{
    Wolf,
    Undead,
    Slime
}

public class AIManager : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnRangeX = 10.0f;
    public float spawnRangeY = 5.0f;
    public int enemyCount = 2;
    public Transform[] spawnPoints;
    private float monsterSpeed = 1.0f;
    private float monsterHp = 1.0f;
    private float monsterDamage = 1.0f;
    private EMonsterType currentMonsterType = EMonsterType.Wolf;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for(int i = 0; i < enemyCount; i++)
        {
            if(spawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Vector2 spawnPosition = spawnPoints[randomIndex].position;
                Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
                MonsterSetState();
            }
            else
            {
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(-spawnRangeY, spawnRangeY);
                Vector2 randomPosition = new Vector2(randomX, randomY);
                Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
            }
        }
    }

    void MonsterSetState()
    {
        MonsterManager monster = monsterPrefab.GetComponent<MonsterManager>();
        float minSpeed = 1.0f;
        float maxSpeed = 10.0f;
        float minHp = 1f;
        float maxHp = 10f;
        float minDamage = 1f;
        float maxDamage = 10f;

        if(monster.monsterType == MonsterType.Wolf)
        {
            minSpeed = 1.0f;
            maxSpeed = 2.0f;
            minHp = 10;
            maxHp = 25;
            minDamage = 5;
            maxDamage = 10;
        }
        else if (monster.monsterType == MonsterType.Undead)
        {
            minSpeed = 2.0f;
            maxSpeed = 3.0f;
            minHp = 30;
            maxHp = 50;
            minDamage = 15;
            maxDamage = 25;
        }
        else if (monster.monsterType == MonsterType.None)
        {
            minSpeed = 0.5f;
            maxSpeed = 1.0f;
            minHp = 1;
            maxHp = 10;
            minDamage = 1;
            maxDamage = 5;
        }
        monsterSpeed = Random.Range(minSpeed, maxSpeed);
        monsterHp = Random.Range(minHp, maxHp);
        monsterDamage = Random.Range(minDamage, maxDamage);
        monster.speed = monsterSpeed;
        monster.monsterHp = monsterHp;
        monster.damage = monsterDamage;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector2.zero, new Vector2(spawnRangeX * 2, spawnRangeY * 2));
        Gizmos.color = Color.blue;
        if (spawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
            }
        }
    }
}
