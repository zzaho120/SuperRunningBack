using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisable
{
    void OnActionByCollision(GameObject other);
}
