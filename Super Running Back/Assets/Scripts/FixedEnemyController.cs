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
        transform.localScale = new Vector3(stats.shapeSize, stats.shapeSize, stats.shapeSize);

        var destroy = GetComponent<DestoryByCollision>();
        var action = GetComponent<ActionByCollision>();
        destroy.Init();
        action.Init();
    }

    public void Init(int level)
    {
        stats = statsLevels[level];
        Init();
    }
}
