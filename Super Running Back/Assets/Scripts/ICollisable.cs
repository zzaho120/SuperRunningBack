using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisable
{
    void onActionByCollision(GameObject other);
}
