using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats.asset", menuName = "Stats/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float currentSpeed;
    public float currentScale;
    public float currentTotalScale;
    public float currentArmScale;

    public float initSpeed;
    public float initScale;
    public float initTotalScale;
    public float initArmScale;

    public float increaseSpeed;
    public float increaseTotalScale;
    public float increaseArmScale;

    public float decreaseSpeed;
    public float decreaseTotalScale;
    public float decreaseArmScale;

    public float gameoverSpeed;

    public void Init()
    {
        currentSpeed = initSpeed;
        currentScale = initScale;
        currentTotalScale = initTotalScale;
        currentArmScale = initArmScale;
    }

    public void MsgGetItem()
    {
        currentSpeed += increaseSpeed;
        currentScale += increaseTotalScale;
        currentTotalScale += increaseTotalScale;
        currentArmScale -= increaseArmScale;
    }
    public void MsgGetPenalty()
    {
        currentSpeed -= decreaseSpeed;
        currentScale -= decreaseTotalScale;
        currentTotalScale -= decreaseTotalScale;
        currentArmScale += decreaseArmScale;
    }
}
