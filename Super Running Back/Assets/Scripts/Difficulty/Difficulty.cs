using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty.asset", menuName = "Difficulty/Area Difficulty")]
public class Difficulty : ScriptableObject
{
    public int difficulty;
    public int maxCost;
    public int minEnemyLevel;
    public int maxEnemyLevel;
    public int minEnemyNumber;
    public int maxEnemyNumber;
    public int minErrorRange;
}
