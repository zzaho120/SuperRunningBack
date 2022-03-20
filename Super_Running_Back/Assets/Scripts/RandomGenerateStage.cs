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
    public bool isGenerateLarge;

    public List<(Vector3, int)> enemyInfoList = new List<(Vector3, int)>();
    public List<(Vector3, int)> fixedEnemyInfoList = new List<(Vector3, int)>();
    public List<Vector3> itemInfoList = new List<Vector3>();

    public void Generate() 
    {
        var gameMgr = GameManager.Instance;
        
        if (gameMgr.isGenerate)
        {
            var enemyInfoListCount = enemyInfoList.Count;
            for (var idx = 0; idx < enemyInfoListCount; ++idx)
            {
                var newGo = ObjectPool.GetObject(PoolName.Enemy);

                var enemy = newGo.GetComponent<EnemyController>();
                enemy.Init(enemyInfoList[idx].Item2);
                newGo.transform.SetParent(enemys);
                newGo.transform.position = enemyInfoList[idx].Item1;
            }

            var fixedEnemyInfoListCount = fixedEnemyInfoList.Count;
            for (var idx = 0; idx < fixedEnemyInfoListCount; ++idx)
            {
                var newGo = ObjectPool.GetObject(PoolName.FixedEnemy);
                var enemy = newGo.GetComponent<FixedEnemyController>();
                enemy.Init(fixedEnemyInfoList[idx].Item2); 
                newGo.transform.SetParent(fixedEnemys);
                newGo.transform.position = fixedEnemyInfoList[idx].Item1;
            }

            var itemInfoListCount = itemInfoList.Count;
            for (var idx = 0; idx < itemInfoListCount; ++idx)
            {
                var newGo = ObjectPool.GetObject(PoolName.Item);
                newGo.transform.SetParent(items);
                newGo.transform.position = itemInfoList[idx];
            }
        }
        else
        {
            enemyInfoList.Clear();
            fixedEnemyInfoList.Clear();
            itemInfoList.Clear();

            currentStageInfo = stageInfos[DataManager.CurrentStageIdx];
            currentStageInfo.Init();
            // 해당 스테이지의 칸 레벨을 섞는다.
            //currentStageInfo.RandomSortLevelArray();
            var maxAreaCnt = (int)(currentStageInfo.yard * 0.1f) + 1;
            //고정 수비수 칸을 정한다.
            while (randomFixedEnemyIndex.Count < currentStageInfo.fixedEnemyAreaCnt)
            {
                var isOverlapValue = false;
                var randomValue = Random.Range(1, maxAreaCnt - 2);

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

            //// 각 야드 영역마다 난이도 설정을 하고 랜덤 생성을 수행한다.
            for (int idx = 0; idx < maxAreaCnt; idx++)
            {
                var difficultyIndex = currentStageInfo.stageLevelArray[idx];
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
                        if (idx == maxAreaCnt - 1)
                            areas[idx].SetGenerateItem(true, isGenerateLarge);
                        else
                            areas[idx].SetGenerateItem();
                    }
                }

                areas[idx].Generate();
            }
        }
    }
}