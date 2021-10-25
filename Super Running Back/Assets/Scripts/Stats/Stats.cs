using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats.asset", menuName = "Stats/BasicStats")]
public class Stats : ScriptableObject
{
    public int level;
    public int moveSpeed;
    public float shapeSize;
}
