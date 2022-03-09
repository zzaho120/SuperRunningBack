using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStatsByCollision : MonoBehaviour, ICollisable
{
    public void onActionByCollision(GameObject other)
    {
        GameManager.Instance.GetItemMsg();
    }
}