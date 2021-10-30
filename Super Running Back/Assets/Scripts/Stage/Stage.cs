using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "stage.asset", menuName = "Stage/Stage Infomation")]
public class Stage : ScriptableObject
{
    public int yard;
    public int fixedEnemyLevel;
    public int fixedEnemyNumCnt;
    public int fixedEnemyAreaCnt;
    public int itemAreaCnt;


    public int[] stageLevelArray;

    private int randomTime = 100;

    public void Init()
    {
        yard = Mathf.Clamp(yard, 50, 100);
        itemAreaCnt = Mathf.Clamp(itemAreaCnt, 0, (int)(yard * 0.1f));
    }
    public void RandomSortLevelArray()
    {
        for(int idx = 0; idx < randomTime; idx++)
        {
            var arrayIdx1 = Random.Range(0, stageLevelArray.Length);
            var arrayIdx2 = Random.Range(0, stageLevelArray.Length);

            Swap(ref stageLevelArray[arrayIdx1], ref stageLevelArray[arrayIdx2]);
        }
    }

    private void Swap(ref int lhs, ref int rhs)
    {
        int temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
}
