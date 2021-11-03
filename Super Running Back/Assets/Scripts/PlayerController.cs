using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public PlayerStats stats;
    public UnityEvent onDieEvent;
    public List<Transform> variableTwoShape;
    public List<Transform> variableThreeShape;
    public List<Transform> armsShape;
    public Transform dumbbellUI;
    public GameObject levelUpEffect;
    public LevelUpText levelUptext;

    public List<AudioClip> playerSound;

    private Rigidbody rigid;
    private Animator animator;
    private AudioSource audioSource;
    private GameObject[] ragdolls;
    private float slideSpeed;
    private float runAniSpeed = 0.25f;
    private float animaitorNomalize = 10f;

    private int ragdollIndex;
    private float decreaseSpeed;
    public float minDecreaseSpeed;
    private bool isPlaying;
    private bool isDead;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ragdolls = GameObject.FindGameObjectsWithTag("EnemyRagdoll");
        animator.SetFloat("MoveX", 0.5f);

        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }


    public void Init()
    {
        foreach (var elem in ragdolls)
        {
            elem.SetActive(false);
        }
        stats.Init();
        SizeSetting();
    }

    private void FixedUpdate()
    {
        if(!isDead && isPlaying)
        {
            Move();
        }
    }

    private void Update()
    {
        CheckPlayerDie();
    }

    private void Move()
    {
        rigid.velocity = new Vector3(slideSpeed, rigid.velocity.y, stats.currentLevel.moveSpeed * decreaseSpeed);
    }

    public void horizontalMove(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Moved:
                slideSpeed = touch.deltaPosition.x;
                var aniValue = SetAnimationValue(touch.deltaPosition.x);
                //Math.Round(aniValue, 1);
                animator.SetFloat("MoveX", (float)Math.Round((double)aniValue, 1));
                break;
            case TouchPhase.Stationary:
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                slideSpeed = 0f;
                animator.SetFloat("MoveX", 0.5f);
                break;
        }
    }

    public void horizontalKeyboardMove(float horizontal)
    {
        slideSpeed = horizontal * speed;
        var aniValue = horizontal + 0.5f;
        animator.SetFloat("MoveX", aniValue);

        if (horizontal == 0)
            decreaseSpeed = 1f;
        else
            decreaseSpeed = minDecreaseSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isDead)
        {
            var collisable = other.GetComponents<ICollisable>();
            var colliEnemy = other.GetComponent<EnemyController>();
            if (colliEnemy != null)
                colliEnemy.State = EnemyController.STATE.PASSOVER;
            foreach (var elem in collisable)
            {
                elem.onActionByCollision(gameObject);
            }
        }
    }

    private void CheckPlayerDie()
    {
        // 충돌 시에 해볼 것 나중에 수정
        if (stats.currentWeight >= stats.currentLevel.power)
        {
            isDead = true;
            onDieEvent.Invoke();
        }
    }

    public void AnimationSpeedUp()
    {
        animator.SetFloat("Speed", runAniSpeed * (stats.currentLevel.level - 1));
    }

    private float SetAnimationValue(float value)
    {
        float result = 0f;

        value += animaitorNomalize;
        value = Mathf.Clamp(value, 0f, animaitorNomalize * 2);
        if (value != 0f)
            result = value / (animaitorNomalize * 2f);
        else
            result = 0f;

        return result;
    }

    public void SetActiveRagdoll(EnemyStats stats)
    {
        var maxCnt = this.stats.currentRagdollCnt;
        for (; ragdollIndex < maxCnt; ragdollIndex++)
        {
            ragdolls[ragdollIndex].SetActive(true);
            ragdolls[ragdollIndex].GetComponent<RagdollManager>().SetStats(stats);
        }
    }

    public void SizeSetting()
    {
        var size = stats.currentLevel.shapeSize;
        foreach (var twoValueShape in variableTwoShape)
        {
            twoValueShape.localScale = new Vector3(size, twoValueShape.localScale.y, size);
        }

        foreach (var threeValueShape in variableThreeShape)
        {
            threeValueShape.localScale = new Vector3(size, size, size);
        }

        foreach (var armShape in armsShape)
        {
            armShape.localScale = new Vector3(armShape.localScale.x, stats.currentLevel.armSize, armShape.localScale.z);
        }
    }

    private void DumbbellUIPosSetting()
    {
        dumbbellUI.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, -1f - 0.4f * stats.currentLevel.level, 0f));
    }

    public void PlayerDead()
    {
        rigid.velocity = Vector3.zero;
        animator.SetTrigger("Dead");
        animator.applyRootMotion = true;
    }

    public void GameStart()
    {
        isPlaying = true;
        animator.SetTrigger("StartGame");
    }

    public void SetLevelUpEffect()
    {
        var particle = Instantiate(levelUpEffect, transform);
        var scale = particle.transform.localScale;
        var level = stats.currentLevel.level - 2;
        particle.transform.localScale = scale * 0.5f + new Vector3(0.1f, 0.1f, 0.1f) * level;
        Destroy(particle, 2f);

        levelUptext.ShowLevelUp();

        SoundPlay(PlayerSound.LevelUp);
    }

    public void SoundPlay(PlayerSound sound)
    {
        audioSource.PlayOneShot(playerSound[(int)sound]);
    }
    public void PlayerFinish()
    {
        isPlaying = false;
        rigid.velocity = Vector3.zero;
        animator.SetTrigger("Finish");
    }
}