using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRagdoll : MonoBehaviour
{
    public GameObject ragdollPrefab;
    public List<GameObject> ragdollList = new List<GameObject>();
    public void AddForceRagdoll()
    {
        var ragdollCnt = GameManager.Instance.player.ragdollObject.transform.childCount;

        for(int idx = 0; idx < ragdollCnt; idx++)
        {
            var randomX = Random.Range(-1f, 1f);
            var randomZ = Random.Range(-1f, 1f);
            var newPos = transform.position + new Vector3(randomX, 4f, randomZ);

            var newGo = ObjectPool.GetObject(PoolName.FinishRagdoll);
            ragdollList.Add(newGo);
            newGo.transform.position = newPos;

            var dir = newGo.transform.position - transform.position;
            var ragdollMgr = newGo.GetComponent<RagdollManager>();
            var rigid = ragdollMgr.rigid;
            rigid.WakeUp();
            rigid.AddForce(dir * 80f, ForceMode.Impulse);
            Debug.Log(newGo.transform.position);
        }
    }

    public void ReturnRagdoll()
    {
        foreach (var ragdoll in ragdollList)
        {
            var ragdollMgr = ragdoll.GetComponent<RagdollManager>();
            var rigid = ragdollMgr.rigid;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            rigid.Sleep();
            ObjectPool.ReturnObject(PoolName.FinishRagdoll, ragdoll);
        }
        ragdollList.Clear();
    }
}
