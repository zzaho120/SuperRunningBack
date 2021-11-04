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
        currentStageInfo = stageInfos[DataManager.CurrentStageIndex];
        currentStageInfo.Init();
        // 해당 스테이지의 칸 레벨을 섞는다.
        currentStageInfo.RandomSortLevelArray();
        var maxAreaCnt = (int)(currentStageInfo.yard * 0.1f) + 1;
        // 고정 수비수 칸을 정한다.
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

        while(itemIndex.Count < currentStageInfo.itemAreaCnt)
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

        // 각 야드 영역마다 난이도 설정을 하고 랜덤 생성을 수행한다.
        for(int idx = 0; idx < maxAreaCnt; idx++)
        {
            var difficultyIndex = currentStageInfo.stageLevelArray[idx] - 1;
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

            areas[idx].Generate();
        }
    }
}