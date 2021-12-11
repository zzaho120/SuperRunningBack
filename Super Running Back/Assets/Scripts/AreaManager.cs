using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public List<GameObject> parts;

    public Difficulty difficulty;

    private bool isGenerateFixedEnemy;
    private bool isGenerateItem;
    private bool isGenerateLargeItem;
    private RandomGenerateStage randomGenerateStage;

    // for ad
    private bool isCenterItem;

    private int generateCnt;
    private int generateCost;

    public void Generate()
    {
        int emptyOrItemPart = 1;

        if (!isCenterItem)
            emptyOrItemPart = Random.Range(0, parts.Count);

        var fixedEnemyIdx = Random.Range(1, parts.Count);
        randomGenerateStage = GameManager.Instance.randomGenerateStage;

        for (int idx = 0; idx < parts.Count; idx++)
        {
            generateCnt = 0;
            generateCost = 0;

            if(isGenerateItem)
            {
                if(emptyOrItemPart == idx)
                {
                    if (!isGenerateLargeItem)
                        GenerateItem(parts[idx]);
                    else
                        GenerateItem();
                }
                else
                    GenerateEnemy(parts[idx]);
            }
            else
            {
                //if (emptyOrItemPart != idx)
                    GenerateEnemy(parts[idx]);
            }

            if(isGenerateFixedEnemy && idx == fixedEnemyIdx)
            {
                GenerateFixedEnemy(parts[idx]);
                isGenerateFixedEnemy = false;
            }
        }
    }

    private void GenerateEnemy(GameObject part)
    {
        var totalCost = 0;
        var enemyCount = 0;
        var costCondition = totalCost < difficulty.maxCost - difficulty.minErrorRange;
        var numberCondition = enemyCount < difficulty.maxEnemyNumber;

        while (costCondition && numberCondition)
        {
            var originPosition = part.transform.position;
            var rangeCollider = part.GetComponent<BoxCollider>();

            var rangeX = rangeCollider.bounds.size.x;
            var rangeZ = rangeCollider.bounds.size.z;

            rangeX = Random.Range((rangeX * 0.5f) * -1, rangeX * 0.5f);
            rangeZ = Random.Range((rangeZ * 0.5f) * -1, rangeZ * 0.5f);

            var randomPosition = new Vector3(rangeX, 0f, rangeZ);
            var respawnPosition = originPosition + randomPosition;

            var enemyLevel = Random.Range(difficulty.minEnemyLevel - 1, difficulty.maxEnemyLevel);
            var newGo = ObjectPool.GetObject(PoolName.Enemy);

            var enemy = newGo.GetComponent<EnemyController>();
            enemy.Init(enemyLevel);

            newGo.transform.SetParent(randomGenerateStage.enemys);
            newGo.transform.position = respawnPosition;

            totalCost += (enemyLevel + 1) * 10;
            enemyCount++;

            costCondition = totalCost < difficulty.maxCost - difficulty.minErrorRange;
            numberCondition = enemyCount < difficulty.maxEnemyNumber;
        }

        generateCnt = enemyCount;
        generateCost = totalCost;
    }

    private void GenerateItem(GameObject part)
    {
        for(int i = 0; i < 3; i++)
        {
            var newGo = ObjectPool.GetObject(PoolName.Item);

            newGo.transform.SetParent(randomGenerateStage.items);

            var pos = part.transform.position + new Vector3(0f, 1f, -10f);
            newGo.transform.position = pos + new Vector3(0f, 0f, 10f * i);
        }
    }

    private void GenerateItem()
    {
        foreach(var part in parts)
        {
            for(int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    var newGo = ObjectPool.GetObject(PoolName.Item);
                    newGo.transform.SetParent(randomGenerateStage.items);
                    var pos = part.transform.position + new Vector3(-9f, 1f, -10f);
                    newGo.transform.position = pos + new Vector3(3f * i, 0f, 2.5f * j);
                }
            }
        }
    }

    private void GenerateFixedEnemy(GameObject part)
    {
        var originPosition = part.transform.position;
        var rangeCollider = part.GetComponent<BoxCollider>();

        var rangeX = rangeCollider.bounds.size.x;
        var rangeZ = rangeCollider.bounds.size.z;

        var stageInfo = randomGenerateStage.currentStageInfo;
        var maxEnemyCnt = stageInfo.fixedEnemyNumCnt;
        var distance = rangeX / maxEnemyCnt;
        var enemyLevel = stageInfo.fixedEnemyLevel - 1;
        for (int idx = 0; idx < maxEnemyCnt; idx++)
        {
            var position = new Vector3(-rangeX * 0.5f + distance * idx, 0f, -rangeZ * 0.5f);
            var respawnPosition = originPosition + position;
            var newGo = ObjectPool.GetObject(PoolName.FixedEnemy);
            var enemy = newGo.GetComponent<FixedEnemyController>();
            enemy.Init(enemyLevel);
            newGo.transform.SetParent(randomGenerateStage.fixedEnemys);
            newGo.transform.position = respawnPosition;
        }
    }

    public void SetGenerateFixedEnemy()
    {
        isGenerateFixedEnemy = true;
    }

    public void SetGenerateItem(bool isCenter = false, bool isLarge = false)
    {
        isGenerateItem = true;
        isCenterItem = isCenter;
        isGenerateLargeItem = isLarge;
    }
}