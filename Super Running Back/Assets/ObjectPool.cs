using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public int maxEnemyCnt;
    public int maxitemCnt;
    public int maxfixedEnemyCnt;
    public int maxParticleCnt;
    public int maxSoundCnt;

    public List<GameObject> nonSettingPrefabs;

    public Dictionary<PoolName, Queue<GameObject>> pool = new Dictionary<PoolName, Queue<GameObject>>();
    public Dictionary<PoolName, GameObject> prefabs = new Dictionary<PoolName, GameObject>();

    private int poolNameCount = (int)PoolName.PoolNameMax;

    private void Awake()
    {
        Instance = this;

        for(int idx = 0; idx < poolNameCount; idx++)
        {
            pool.Add((PoolName)idx, new Queue<GameObject>());
            prefabs.Add((PoolName)idx, nonSettingPrefabs[idx]);
        }

        queueInit(maxEnemyCnt, prefabs[PoolName.Enemy], pool[PoolName.Enemy]);
        queueInit(maxitemCnt, prefabs[PoolName.FixedEnemy], pool[PoolName.FixedEnemy]);
        queueInit(maxfixedEnemyCnt, prefabs[PoolName.Item], pool[PoolName.Item]);

        queueInit(maxParticleCnt, prefabs[PoolName.KickParticle], pool[PoolName.KickParticle]);
        queueInit(maxSoundCnt, prefabs[PoolName.KickSound], pool[PoolName.KickSound]);

        queueInit(maxParticleCnt, prefabs[PoolName.HoldParticle], pool[PoolName.HoldParticle]);
        queueInit(maxSoundCnt, prefabs[PoolName.HoldSound], pool[PoolName.HoldSound]);

        queueInit(maxParticleCnt, prefabs[PoolName.LevelUpParticle], pool[PoolName.LevelUpParticle]);
        queueInit(maxSoundCnt, prefabs[PoolName.LevelUpSound], pool[PoolName.LevelUpSound]);
    }

    private void queueInit(int initCnt, GameObject obj, Queue<GameObject> queue)
    {
        for (int idx = 0; idx < initCnt; idx++)
        {
            queue.Enqueue(Create(obj));
        }
    }

    private GameObject Create(GameObject obj)
    {
        var newGo = Instantiate(obj);
        newGo.SetActive(false);
        newGo.transform.SetParent(transform);
        return newGo;
    }

    public static GameObject GetObject(PoolName poolName)
    {
        var queue = Instance.pool[poolName];
        if(queue.Count > 0)
        {
            var obj = queue.Dequeue();
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var newGo = Instance.Create(Instance.prefabs[poolName]);
            newGo.gameObject.SetActive(true);
            newGo.transform.SetParent(null);
            return newGo;
        }
    }
    
    public static void ReturnObject(PoolName poolName, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.pool[poolName].Enqueue(obj);
    }
}
