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
    public Transform toalShape;
    public Transform variableThreeShape;
    public List<Transform> variableTwoShape;
    public List<Transform> armsShape;
    public AudioClip GoVoice;
    public GameObject ragdolls;
    public float minDecreaseSpeed;

    private Rigidbody rigid;
    private Animator animator;
    private AudioSource audioSource;
    private GameObject[] ragdollObjs;
    private float slideSpeed;
    private float runAniSpeed = 0.17f;
    private float animaitorNomalize = 10f;

    private int ragdollIndex;
    private float decreaseSpeed = 1f;
    private bool isPlaying;
    private bool isDead;

    private bool isFirstTouch;
    private Vector2 originTouchPos;
    private Vector2 touchPos;
    private float maxMoveDistance = 24.5f;
    private float aniValue;
    private int fingerId;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ragdollObjs = GameObject.FindGameObjectsWithTag("EnemyRagdoll");
    }


    public void Init()
    {
        isDead = false;
        isPlaying = false;
        isFirstTouch = false;
        slideSpeed = 0;
        fingerId = int.MinValue;
        rigid.velocity = Vector3.zero; 
        originTouchPos = Vector2.zero;
        touchPos = Vector2.zero;
        animator.SetBool("Restart", true);
        animator.SetBool("StartGame", false);
        animator.SetBool("Dead", false);

        animator.applyRootMotion = false;
        ragdollIndex = 0;
        ragdolls.SetActive(true);
        foreach (var elem in ragdollObjs)
        {
            elem.SetActive(false);
        }
        stats.Init();
        SizeSetting();
    }

    private void FixedUpdate()
    {
        if (!isDead && isPlaying)
        {
            Move();
        }
    }

    private void Move()
    {
        rigid.velocity = new Vector3(slideSpeed * speed, rigid.velocity.y, stats.currentSpeed * decreaseSpeed);
    }

    public void horizontalMove(Touch touch)
    {
        Vector2 dir = Vector2.zero;
        switch (touch.phase)
        {
            case TouchPhase.Began:
                isFirstTouch = true;
                originTouchPos = Camera.main.ScreenToViewportPoint(touch.position);
                if (fingerId == int.MinValue)
                {
                    fingerId = touch.fingerId;
                }
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (fingerId == touch.fingerId)
                {
                    if (!isFirstTouch)
                    {
                        isFirstTouch = true;
                        originTouchPos = Camera.main.ScreenToViewportPoint(touch.position);
                    }
                    touchPos = Camera.main.ScreenToViewportPoint(touch.position);
                    dir = touchPos - originTouchPos;
                    slideSpeed = dir.x * 3f;
                    decreaseSpeed = Mathf.Lerp(1f, minDecreaseSpeed, Mathf.Abs(slideSpeed));
                }
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                fingerId = int.MinValue;
                isFirstTouch = false;
                originTouchPos = Vector2.zero;
                touchPos = Vector2.zero;
                slideSpeed = 0;
                break;
        }
        AnimationSpeedUp();

        var leftMoveLimit = dir.x < 0f && transform.position.x < -maxMoveDistance;
        var rightMoveLimit = dir.x > 0f && transform.position.x > maxMoveDistance;
        if (leftMoveLimit || rightMoveLimit)
            slideSpeed = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isDead)
        {
            if(other.tag == "FixedEnemy")
            {
                var colliders = Physics.OverlapSphere(transform.position, 1.5f);
                foreach(var collider in colliders)
                {
                    var collisable = collider.GetComponents<ICollisable>();
                    
                    foreach(var elem in collisable)
                    {
                        elem.onActionByCollision(gameObject);
                    }
                }
            }
            else
            {
                var collisable = other.GetComponents<ICollisable>();
                foreach (var elem in collisable)
                {
                    elem.onActionByCollision(gameObject);
                }
            }

            if (stats.currentSpeed < stats.gameoverSpeed)
            {
                isDead = true;
                onDieEvent.Invoke();
            }
        }
    }

    public void AnimationSpeedUp()
    {
        animator.SetFloat("MoveX", (float)Math.Round((double)slideSpeed, 1));
        //animator.SetFloat("Speed", runAniSpeed * (stats.currentLevel.level - 1));
    }

    public void SetActiveRagdoll(EnemyStats stats)
    {
        //var maxCnt = this.stats.currentRagdollCnt;
        //for (; ragdollIndex < maxCnt; ragdollIndex++)
        //{
        //    ragdollObjs[ragdollIndex].SetActive(true);
        //    ragdollObjs[ragdollIndex].GetComponent<RagdollManager>().SetStats(stats);
        //}
    }

    public void SizeSetting()
    {
        var currentScale = stats.currentScale;
        var currentTotalScale = stats.currentTotalScale;
        foreach (var twoValueShape in variableTwoShape)
        {
            twoValueShape.localScale = new Vector3(currentScale, twoValueShape.localScale.y, currentScale);
        }

        variableThreeShape.localScale = new Vector3(currentScale, currentScale, currentScale);
        toalShape.localScale = new Vector3(currentTotalScale, currentTotalScale, currentTotalScale);

        foreach (var armShape in armsShape)
        {
            armShape.localScale = new Vector3(armShape.localScale.x, stats.currentArmScale, armShape.localScale.z);
        }
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

        audioSource.PlayOneShot(GoVoice);
    }

    public void MsgGetItem()
    {
        stats.MsgGetItem();
        SizeSetting();
    }

    public void MsgGetPenalty()
    {
        stats.MsgGetPenalty();
        SizeSetting();
    }
}