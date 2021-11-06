using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Transform explosionTr;
    public Transform ragdollTr;
    public float range;
    public float force;
    public GameObject ragdoll;
    public GameObject player;
    public int ragdollCountTime = 1;
    public float ragdollHeight;
    public float minGenerateRange;
    public float maxGenerateRange;
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
        
    }

    public void GenerateRagdoll()
    {
        var ragdollCnt = GameManager.Instance.player.stats.currentWeight;
        for (int idx = 0; idx < ragdollCnt; idx++)
        {
            var randomValueX = Random.Range(minGenerateRange, maxGenerateRange);
            var randomValueZ = Random.Range(minGenerateRange, maxGenerateRange);
            var position = ragdollTr.position + new Vector3(randomValueX, ragdollHeight, randomValueZ);
            var obj = Instantiate(ragdoll, position, Quaternion.identity);

            Destroy(obj, 5f);
            var ragdollMgr = obj.GetComponent<RagdollManager>();
            ragdollMgr.touchDownStats();

            var rigid = ragdollMgr.rigid;
            var dir = obj.transform.position - ragdollTr.transform.position;

            rigid.AddForce(dir.normalized * force * 6, ForceMode.Impulse);
        }
    }
}

