using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats.asset", menuName = "Stats/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public List<PlayerLevel> playerLevels;

    public PlayerLevel currentLevel;
    public int currentItemCnt;
    public int currentWeight;
    public int currentKickScore;
    public int currentRagdollCnt;

    private Vector3 initScale;
    public int initLevel;

    public float CurrentWeightByPower
    {
        get 
        {
            return currentWeight / (float)currentLevel.power;
        }
    }

    public float CurrentItemByNextLevel
    {
        get
        {
            if (currentLevel.level < playerLevels.Count)
                return (currentItemCnt - playerLevels[currentLevel.level - 1].itemCount) /
                    (float)(playerLevels[currentLevel.level].itemCount - playerLevels[currentLevel.level - 1].itemCount);
            else
                return 1.0f;
        }
    }

    public void CheckItem()
    {
        bool check = false;
        if(playerLevels.Count > currentLevel.level)
            check = currentItemCnt >= playerLevels[currentLevel.level].itemCount;

        if(check)
        {
            currentLevel = playerLevels[currentLevel.level];
            GameManager.Instance.PlayerLevelUpMsg();
        }
    }

    public void Init()
    {
        currentItemCnt = 0;
        currentWeight = 0;
        currentKickScore = 0;
        currentRagdollCnt = 0;
        initScale = GameManager.Instance.player.transform.localScale;

        if(currentLevel == null)
            currentLevel = playerLevels[0];

        else
        {
            if(initLevel == 0)
            {
                currentLevel = playerLevels[0];
            }
            else if(playerLevels.Count >= initLevel)
                currentLevel = playerLevels[initLevel - 1];
            else
            {
                Debug.Log($"플레이어 레벨 배열은 {playerLevels.Count}개인데 참조하려는" +
                    $"인덱스는 {initLevel - 1}이다.");
            }
        }
    }

    public void KickScoreUp(int level)
    {
        currentKickScore += level * 100;
    }
}
