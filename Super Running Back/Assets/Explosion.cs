using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Transform explosionTr;
    public float range;
    public float force;
    public void OnExplosionEffect()
    {
        var frags = Physics.OverlapSphere(explosionTr.position, range);
        foreach (var obj in frags)
        {
            var rigid = obj.GetComponent<Rigidbody>();

            if(rigid != null)
            {
                var dir = obj.transform.position - explosionTr.position;
                dir.y += Random.Range(10f, 15f);

                if (rigid.isKinematic)
                    rigid.isKinematic = false;
                rigid.AddForce(dir.normalized * force, ForceMode.Impulse);
            }
        }
    }

    public void OnExplosionActiveOff()
    {
        gameObject.SetActive(false);
    }
}
