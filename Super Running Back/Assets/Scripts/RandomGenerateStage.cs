using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerateStage : MonoBehaviour
{
    public AreaManager[] areas;
    public Stage stageInfo;
    public List<Difficulty> difficulties;

    public List<GameObject> enemysByLevel;
    public List<GameObject> fixedEnemyByLevel;
    public List<int> randomFixedEnemyIndex;
    public List<int> itemIndex;
    public Transform enemys;
    public Transform items;
    public Transform fixedEnemys;

    public GameObject itemSet;

    private int generateFixedEnemyCnt;

    public void Generate() 
    {
        stageInfo.Init();
        // �ش� ���������� ĭ ������ ���´�.
        stageInfo.RandomSortLevelArray();
        var maxAreaCnt = (int)(stageInfo.yard * 0.1f) + 1;
        // ���� ����� ĭ�� ���Ѵ�.
        while (randomFixedEnemyIndex.Count < stageInfo.fixedEnemyAreaCnt)
        {
            var isOverlapValue = false;
            var randomValue = Random.Range(0, maxAreaCnt);

            foreach (var elem in itemIndex)
            {
                if (elem == randomValue)
                    isOverlapValue = true;
            }

            if (!isOverlapValue)
                randomFixedEnemyIndex.Add(randomValue);
        }

        while(itemIndex.Count < stageInfo.itemAreaCnt)
        {
            var isOverlapValue = false;
            var randomValue = Random.Range(1, maxAreaCnt);

            foreach(var elem in itemIndex)
            {
                if (elem == randomValue)
                    isOverlapValue = true;
            }

            if(!isOverlapValue)
                itemIndex.Add(randomValue);
        }

        // �� �ߵ� �������� ���̵� ������ �ϰ� ���� ������ �����Ѵ�.
        for(int idx = 0; idx < maxAreaCnt; idx++)
        {
            var difficultyIndex = stageInfo.stageLevelArray[idx] - 1;
            areas[idx].difficulty = difficulties[difficultyIndex];

            foreach(var elem in randomFixedEnemyIndex)
            {
                if (idx == elem)
                {
                    areas[idx].SetGenerateFixedEnemy();
                }
            }
            
            foreach(var elem in itemIndex)
            {
                if(idx == elem)
                {
                    areas[idx].SetGenerateItem();
                }
            }

            Debug.Log($"idx : {idx}, difficulty : {areas[idx].difficulty}");

            areas[idx].Generate();
        }
    }
}