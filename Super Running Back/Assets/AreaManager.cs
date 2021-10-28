using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public GameObject enemys;
    public List<GameObject> parts;
    public List<GameObject> enemysByLevel;
    public List<GameObject> fixedEnemyByLevel;

    public Difficulty difficulty;

    private int emptyOrItemPart;
    private bool isExistItemPart;

    public void Awake()
    {
        emptyOrItemPart = Random.Range(0, parts.Count);
        isExistItemPart = Random.Range(0, 1) == 1 ? true : false;
    }

    public void Generate()
    {
        for(int idx = 0; idx < parts.Count; idx++)
        {
            if (idx == emptyOrItemPart && isExistItemPart)
            {
                // 아이템 생성
            }
            else 
            {
                GeneratePart(parts[idx]);
            }
        }
    }

    private void GeneratePart(GameObject part)
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

            var randomEnemy = Random.Range(difficulty.minEnemyLevel, difficulty.maxEnemyLevel) - 1;

            var enemy = enemysByLevel[randomEnemy];

            var newGo = Instantiate(enemy, respawnPosition, Quaternion.identity);

            newGo.transform.SetParent(enemys.transform);

            totalCost += (randomEnemy + 1) * 10;
            enemyCount++;

            costCondition = totalCost < difficulty.maxCost - difficulty.minErrorRange;
            numberCondition = enemyCount < difficulty.maxEnemyNumber;
        }
    }
}