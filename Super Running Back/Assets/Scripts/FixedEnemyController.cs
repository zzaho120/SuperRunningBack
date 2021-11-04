using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedEnemyController : MonoBehaviour
{
    public List<EnemyStats> statsLevels;
    public EnemyStats stats;
    public void Init()
    {
        transform.rotation = Quaternion.Euler(0f, -180f, 0f);
        transform.localScale *= stats.shapeSize;
    }

    public void Init(int level)
    {
        stats = statsLevels[level];
        Init();
    }
}
