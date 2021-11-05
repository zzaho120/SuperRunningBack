using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public void Init()
    {
        var destory = GetComponent<DestoryByCollision>();
        destory.Init();
    }
}
