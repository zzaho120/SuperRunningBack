using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public EnemyStats stats;
    public Rigidbody rigid;

    public void SetStats(EnemyStats stats)
    {
        this.stats = stats;
        transform.localScale *= stats.shapeSize;
    }

    public void ApplyForce(Vector3 vector)
    {
        rigid.AddForce(vector, ForceMode.Impulse);
    }
}