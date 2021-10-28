using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedEnemyController : MonoBehaviour
{
    public EnemyStats stats;

    private void Start()
    {
        transform.localScale *= stats.shapeSize;
    }
}
