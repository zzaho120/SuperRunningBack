using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public List<GameObject> parts;

    public Difficulty difficulty;

    private bool isGenerateFixedEnemy;
    private bool isGenerateItem;
    private RandomGenerateStage randomGenerateStage;

    private int generateCnt;
    private int generateCost;

    private void Start()
    {
        randomGenerateStage = GameManager.Instance.randomGenerateStage;
    }

    public void Generate()
    {
        var emptyOrItemPart = Random.Range(0, parts.Count);
        var fixedEnemyIdx = Random.Range(0, parts.Count);
        randomGenerateStage = GameObject.FindWithTag("GameManager").GetComponent<RandomGenerateStage>();

        for (int idx = 0; idx < parts.Count; idx++)
        {
            generateCnt = 0;
            generateCost = 0;

            if(isGenerateItem)
            {
                if(emptyOrItemPart == idx)
                {
                    GenerateItem(parts[idx]);
                }
                else
                    GenerateEnemy(parts[idx]);
            }
            else
            {
                if (emptyOrItemPart != idx)
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

            var randomEnemy = Random.Range(difficulty.minEnemyLevel - 1, difficulty.maxEnemyLevel);

            var enemy = randomGenerateStage.enemysByLevel[randomEnemy];

            var newGo = //ObjectPool.GetObject(PoolName.Enemy);
                Instantiate(enemy, respawnPosition, Quaternion.identity);

            newGo.transform.SetParent(randomGenerateStage.enemys);
            newGo.transform.position = respawnPosition;

            totalCost += (randomEnemy + 1) * 10;
            enemyCount++;

            costCondition = totalCost < difficulty.maxCost - difficulty.minErrorRange;
            numberCondition = enemyCount < difficulty.maxEnemyNumber;
        }

        generateCnt = enemyCount;
        generateCost = totalCost;
    }

    private void GenerateItem(GameObject part)
    {
        var newGo = Instantiate(randomGenerateStage.itemSet, part.transform.position, Quaternion.identity);

        newGo.transform.SetParent(randomGenerateStage.items);
    }

    private void GenerateFixedEnemy(GameObject part)
    {
        var originPosition = part.transform.position;
        var rangeCollider = part.GetComponent<BoxCollider>();

        var rangeX = rangeCollider.bounds.size.x;
        var rangeZ = rangeCollider.bounds.size.z;


        var stageInfo = randomGenerateStage.stageInfo;
        var maxEnemyCnt = stageInfo.fixedEnemyNumCnt;
        var distance = rangeX / maxEnemyCnt;
        var enemyLevel = stageInfo.fixedEnemyLevel - 1;
        var enemy = randomGenerateStage.fixedEnemyByLevel[enemyLevel];
        for (int idx = 0; idx < maxEnemyCnt; idx++)
        {
            var position = new Vector3(-rangeX * 0.5f + distance * idx, 0f, -rangeZ * 0.5f);
            var respawnPosition = originPosition + position;
            var newGo = Instantiate(enemy, respawnPosition, Quaternion.identity);

            newGo.transform.SetParent(randomGenerateStage.fixedEnemys);
        }
    }

    public void SetGenerateFixedEnemy()
    {
        isGenerateFixedEnemy = true;
    }

    public void SetGenerateItem()
    {
        isGenerateItem = true;
    }
}