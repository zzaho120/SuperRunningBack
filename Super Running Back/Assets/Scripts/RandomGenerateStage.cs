using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerateStage : MonoBehaviour
{
    public AreaManager[] areas;
    public Stage currentStageInfo;
    public List<Stage> stageInfos;
    public List<Difficulty> difficulties;

    public List<int> randomFixedEnemyIndex;
    public List<int> itemIndex;
    public Transform enemys;
    public Transform items;
    public Transform fixedEnemys;

    private int generateFixedEnemyCnt;

    public void Generate() 
    {
        currentStageInfo = stageInfos[DataManager.CurrentStageIdx];
        currentStageInfo.Init();
        // �ش� ���������� ĭ ������ ���´�.
        currentStageInfo.RandomSortLevelArray();
        var maxAreaCnt = (int)(currentStageInfo.yard * 0.1f) + 1;
        //���� ����� ĭ�� ���Ѵ�.
        while (randomFixedEnemyIndex.Count < currentStageInfo.fixedEnemyAreaCnt)
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

        while (itemIndex.Count < currentStageInfo.itemAreaCnt)
        {
            var isOverlapValue = false;
            var randomValue = Random.Range(1, maxAreaCnt);

            foreach (var elem in itemIndex)
            {
                if (elem == randomValue)
                    isOverlapValue = true;
            }

            if (!isOverlapValue)
                itemIndex.Add(randomValue);
        }

        //// �� �ߵ� �������� ���̵� ������ �ϰ� ���� ������ �����Ѵ�.
        for (int idx = 0; idx < maxAreaCnt; idx++)
        {
            var difficultyIndex = currentStageInfo.stageLevelArray[idx] - 1;
            areas[idx].difficulty = difficulties[difficultyIndex];

            foreach (var elem in randomFixedEnemyIndex)
            {
                if (idx == elem)
                {
                    areas[idx].SetGenerateFixedEnemy();
                }
            }

            foreach (var elem in itemIndex)
            {
                if (idx == elem)
                {
                    areas[idx].SetGenerateItem();
                }
            }

            areas[idx].Generate();
        }
    }
}