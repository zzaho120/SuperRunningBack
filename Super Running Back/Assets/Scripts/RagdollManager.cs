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
        transform.localScale = new Vector3(stats.shapeSize, stats.shapeSize, stats.shapeSize);
    }

    public void touchDownStats()
    {
        transform.localScale = new Vector3(stats.shapeSize, stats.shapeSize, stats.shapeSize);
    }

    public void ApplyForce(Vector3 vector)
    {
        rigid.AddForce(vector, ForceMode.Impulse);
    }
}