using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Transform explosionTr;
    public float range;
    public float force;
    public GameObject ragdoll;
    public GameObject player;
    public void OnExplosionEffect()
    {
        var colliders = Physics.OverlapSphere(explosionTr.position, range);
        foreach (var obj in colliders)
        {
            var rigid = obj.GetComponent<Rigidbody>();

            if (rigid != null)
            {
                var dir = obj.transform.position - explosionTr.position;
                dir.y += Random.Range(10f, 15f);

                if (rigid.isKinematic)
                    rigid.isKinematic = false;
                rigid.AddForce(dir.normalized * force, ForceMode.Impulse);
            }
        }
        var ragdollCnt = GameManager.Instance.player.stats.currentRagdollCnt;

        for (int idx = 0; idx < ragdollCnt; idx++)
        {
            var randomValue = Random.Range(-3f, 3f);
            var position = transform.position + new Vector3(randomValue, 3f, randomValue);
            var obj = Instantiate(ragdoll, position, Quaternion.identity);
            Destroy(obj, 3f);
            var rigid = obj.GetComponent<RagdollManager>().rigid;

            Debug.Log(rigid.gameObject.name);
            var dir = obj.transform.position - player.transform.position;

            rigid.AddForce(dir.normalized * force * 6, ForceMode.Impulse);

        }
    }
    public void OnGenerateRagdoll()
    {
        
    }
}
