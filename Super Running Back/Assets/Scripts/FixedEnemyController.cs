using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedEnemyController : MonoBehaviour
{
    public List<EnemyStats> statsLevels;
    public EnemyStats stats;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(0f, -180f, 0f);
    }
    private void Start()
    {
        transform.localScale *= stats.shapeSize;
    }
}
