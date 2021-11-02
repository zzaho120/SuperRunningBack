using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    public Transform explosionTr;
    public float range;
    public float force;
    public void OnFragEffect()
    {
        var frags = Physics.OverlapSphere(explosionTr.position, range);
        foreach (var obj in frags)
        {
            obj.gameObject.SendMessage("Damage", 1000f, SendMessageOptions.DontRequireReceiver);
            var rigid = obj.GetComponentsInChildren<Rigidbody>();

            var dir =  obj.transform.position - explosionTr.position ;
            foreach(var elem in rigid)
            {
                if (elem.isKinematic)
                    elem.isKinematic = false;
                elem.AddForce(dir * force);
            }
            
        }
    }

    public void OnFragActiveOff()
    {
        gameObject.SetActive(false);
    }
}
