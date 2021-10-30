using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public int maxEnemyCnt;

    public GameObject enemyPrefab;

    Queue<EnemyController> enemyPoolQueue = new Queue<EnemyController>();

    private void Awake()
    {
        Instance = this;

        EnemyInit(maxEnemyCnt);
    }

    private void EnemyInit(int initCnt)
    {
        for (int idx = 0; idx < initCnt; idx++)
        {
            enemyPoolQueue.Enqueue(CreateNewEnemy());
        }
    }

    private EnemyController CreateNewEnemy()
    {
        var newGo = Instantiate(enemyPrefab).GetComponent<EnemyController>();
        newGo.gameObject.SetActive(false);
        newGo.transform.SetParent(transform);
        return newGo;
    }

    public static EnemyController GetEnemy()
    {
        if(Instance.enemyPoolQueue.Count > 0)
        {
            var obj = Instance.enemyPoolQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newGo = Instance.CreateNewEnemy();
            newGo.gameObject.SetActive(true);
            newGo.transform.SetParent(null);
            return newGo;
        }
    }
    
    public static void ReturnObject(EnemyController obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.enemyPoolQueue.Enqueue(obj);
    }
}
