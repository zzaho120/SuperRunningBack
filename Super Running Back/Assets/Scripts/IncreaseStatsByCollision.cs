using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStatsByCollision : MonoBehaviour, ICollisable
{
    public void onActionByCollision(GameObject other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.stats.currentItemCnt++;
            player.stats.CheckItem();
        }
    }
}