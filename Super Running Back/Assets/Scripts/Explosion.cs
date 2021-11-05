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
    public int ragdollCountTime = 1;
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
        var ragdollCnt = GameManager.Instance.player.stats.currentWeight;

        for (int idx = 0; idx < ragdollCnt; idx++)
        {
            var randomValueX = Random.Range(-5f, 5f);
            var randomValueZ = Random.Range(-5f, 5f);
            var position = transform.position + new Vector3(randomValueX, 3f, randomValueZ);
            var obj = Instantiate(ragdoll, position, Quaternion.identity);
            
            Destroy(obj, 5f);
            var ragdollMgr = obj.GetComponent<RagdollManager>();
            ragdollMgr.touchDownStats();

            var rigid = ragdollMgr.rigid;
            Debug.Log(rigid.gameObject.name);
            var dir = obj.transform.position - player.transform.position;

            rigid.AddForce(dir.normalized * force * 6, ForceMode.Impulse);
        }
    }
}
