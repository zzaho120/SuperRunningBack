using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    public List<GameObject> forceObject;
    public Transform explosionTr;
    public float forcePower;

    public void fenceAddForce()
    {
        foreach(var elem in forceObject)
        {
            var dir = elem.transform.position - explosionTr.position;
            dir.y = Random.Range(1f, 4f);
            var rigid = elem.GetComponent<Rigidbody>();

            rigid.AddForce(dir * forcePower, ForceMode.Impulse);
        }
    }
}
