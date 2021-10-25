using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "EnemyStats.asset", menuName = "Stats/EnemyStats")]
public class EnemyStats : Stats
{
    public int weight;

    public float kickRate;
    public float divingRate;

    public int holdPoint;
    public int kickPoint;
}