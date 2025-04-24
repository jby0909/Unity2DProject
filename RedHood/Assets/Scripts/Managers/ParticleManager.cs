using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }
    private Dictionary<ParticleType, GameObject> particlePrefabDic = new Dictionary<ParticleType, GameObject>();
    private Dictionary<ParticleType, Queue<GameObject>> particlePools = new Dictionary<ParticleType, Queue<GameObject>>();

    public GameObject playerAttackEffectPrefab;
    public GameObject playerDamageEffectPrefab;
    public GameObject playerHealEffectPrefab;
    public int poolSize = 5;

    private void Awake()
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

        //particlePrefabDic.Add(ParticleType.PlayerAttack, playerAttackEffectPrefab);
        //particlePrefabDic.Add(ParticleType.PlayerDamage, playerDamageEffectPrefab);
        particlePrefabDic.Add(ParticleType.PlayerHeal, playerHealEffectPrefab);

        foreach (var type in particlePrefabDic.Keys)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++) 
            {
                GameObject obj = Instantiate(particlePrefabDic[type]);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            particlePools.Add(type, pool);
        }
    }

    public void ParticlePlay(ParticleType type, GameObject position, Vector3 scale)
    {
        if(particlePools.ContainsKey(type))
        {
            GameObject particleObj = particlePools[type].Dequeue();

            if(particleObj != null)
            {
                particleObj.transform.SetParent(position.transform, false);
                particleObj.transform.localScale = scale;
                particleObj.SetActive(true);

                Animator animator = particleObj.GetComponent<Animator>();
                if(animator != null)
                {
                    animator.Play(0);
                    StartCoroutine(AnimationEndCoroutine(type, particleObj, animator));
                }
            }
        }
    }

    IEnumerator AnimationEndCoroutine(ParticleType type, GameObject obj, Animator animator)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        obj.SetActive(false);
        obj.transform.SetParent(null);
        particlePools[type].Enqueue(obj);
    }
    
}

public enum ParticleType
{
    PlayerAttack,
    PlayerDamage,
    PlayerHeal
}
