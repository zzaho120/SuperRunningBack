using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRagdoll : MonoBehaviour
{
    public GameObject ragdollPrefab;
    public void AddForceRagdoll()
    {
        var ragdollCnt = GameManager.Instance.player.ragdollObject.transform.childCount;

        for(int idx = 0; idx < ragdollCnt; idx++)
        {
            var randomX = Random.Range(-1f, 1f);
            var randomZ = Random.Range(-1f, 1f);
            var newPos = transform.position + new Vector3(randomX, 5f, randomZ);

            var newGo = Instantiate(ragdollPrefab, newPos, Quaternion.identity);

            var dir = newGo.transform.position - transform.position;
            var ragdollMgr = newGo.GetComponent<RagdollManager>();
            var rigid = ragdollMgr.rigid;
            rigid.AddForce(dir * 10f, ForceMode.Impulse);
        }
    }
}
