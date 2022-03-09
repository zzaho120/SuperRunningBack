using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLevel.asset", menuName = "Stats/PlayerLevel")]
public class PlayerLevel : Stats
{
    public float armSize;
    public int ragdollCount;
    public int itemCount;
    public int power;
}