using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionByCollision : MonoBehaviour, ICollisable
{
    public GameObject ragdollPrefab;
    public float forcePower;
    public GameObject kickEffect;
    public GameObject holdEffect;

    public void onActionByCollision(GameObject other)
    {
        EnemyStats enemyStats = null;
        if(gameObject.CompareTag("Enemy"))
            enemyStats = GetComponent<EnemyController>().stats;
        else if (gameObject.CompareTag("FixedEnemy"))
            enemyStats = GetComponent<FixedEnemyController>().stats;

        var player = other.GetComponent<PlayerController>(); 
        var scoreManager = GameManager.Instance.scoreManager;

        var isKick = player.stats.currentLevel.level >= enemyStats.level &&
            Random.Range(0, 100) < enemyStats.kickRate;

        if (isKick)
        {
            var effectPos = transform.position + new Vector3(0f, 5f, 2f);
            var effect = ObjectPool.GetObject(PoolName.KickParticle);
            effect.transform.position = effectPos;
            effect.transform.localScale *= 0.2f;
            


            player.SoundPlay(PlayerSound.Kick);

            var ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            var ragdollMgr = ragdoll.GetComponent<RagdollManager>();
            
            ragdollMgr.SetStats(enemyStats);

            player.stats.KickScoreUp(enemyStats.level);
            scoreManager.AddKickEnemyNumber();

            var dir = ragdoll.transform.position - other.transform.position;

            dir.y = 0.5f;
            ragdollMgr.ApplyForce(dir * forcePower);

            Destroy(ragdoll, 2f);
        }
        else
        {
            var effectPos = transform.position + new Vector3(0f, 3f, 0f);
            var effect = Instantiate(holdEffect, effectPos, Quaternion.identity);
            effect.transform.localScale *= 0.2f;

            player.SoundPlay(PlayerSound.Hold);

            var playerStat = player.stats; 

            if (playerStat.currentRagdollCnt < playerStat.currentLevel.ragdollCount)
                playerStat.currentRagdollCnt++;

            playerStat.currentWeight += enemyStats.weight;
            
            player.SetActiveRagdoll(enemyStats);
            scoreManager.AddHoldEnemyNumber();
        }

        GameManager.Instance.InplayPrintScore();
    }
}