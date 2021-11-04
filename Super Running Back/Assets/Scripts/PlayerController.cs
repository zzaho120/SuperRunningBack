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
    public Transform originTr;

    private Rigidbody rigid;
    private Animator animator;
    private AudioSource audioSource;
    private GameObject[] ragdolls;
    private float slideSpeed;
    private float runAniSpeed = 0.25f;
    private float animaitorNomalize = 10f;

    private int ragdollIndex;
    private float decreaseSpeed = 1f;
    public float minDecreaseSpeed;
    private bool isPlaying;
    private bool isDead;

    private Vector3 touchPos;
    private Vector3 touchViewPos;
    private float aniValue;

    public ParticleSystem levelUpParticle;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ragdolls = GameObject.FindGameObjectsWithTag("EnemyRagdoll");
    }


    public void Init()
    {
        isDead = false;
        isPlaying = false;
        rigid.velocity = Vector3.zero;
        animator.SetBool("Restart", true);
        animator.SetBool("StartGame", false);
        animator.SetBool("Dead", false);

        transform.localPosition = originTr.localPosition;
        transform.rotation = originTr.rotation;
        animator.applyRootMotion = false;
        animator.SetFloat("MoveX", 0.5f);
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

    public void horizontalMove(float h)
    {
        slideSpeed = speed * h;

        if (slideSpeed != 0)
            decreaseSpeed = minDecreaseSpeed;
        else
            decreaseSpeed = 1f;

        aniValue = h + 1.0f;
        aniValue *= 0.5f;
        animator.SetFloat("MoveX", (float)Math.Round((double)aniValue, 1));
    }
    public void horizontalMove(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                touchPos = touch.position;
                touchViewPos = Camera.main.ScreenToViewportPoint(touchPos);

                if (touchViewPos.x < 0.3f)
                {
                    SetMoveValue(-speed, -Time.deltaTime, minDecreaseSpeed);
                }
                else if(touchViewPos.x > 0.7f)
                {
                    SetMoveValue(speed, Time.deltaTime, minDecreaseSpeed);
                }
                else
                {
                    SetMoveValue(0, 0.5f, 1f, true);
                }
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                SetMoveValue(0, 0.5f, 1f, true);
                break;
        }
        animator.SetFloat("MoveX", (float)Math.Round((double)aniValue, 1));
    }

    private void SetMoveValue(float slide, float ani, float decrease, bool isCenter = false)
    {
        if (isCenter)
        {
            aniValue = ani;
        }
        else 
        {
            aniValue += ani;
            aniValue = Mathf.Clamp(aniValue, -1f, 1f);
        }
        slideSpeed = slide;
        decreaseSpeed = decrease;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isDead)
        {
            var collisable = other.GetComponents<ICollisable>();
            var colliEnemy = other.GetComponent<EnemyController>();
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
        animator.SetBool("Dead", true);
        animator.applyRootMotion = true;
    }

    public void GameStart()
    {
        isPlaying = true;
        animator.SetBool("StartGame", true);
        animator.SetBool("Restart", false);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void SetLevelUpEffect()
    {
        var scale = levelUpParticle.transform.localScale;
        var level = stats.currentLevel.level - 2;
        levelUpParticle.transform.localScale = scale * 0.5f + new Vector3(0.1f, 0.1f, 0.1f) * level;

        levelUptext.ShowLevelUp();
        audioSource.Play();
    }
}