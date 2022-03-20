using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;
    public Transform toalShape;
    public Transform variableThreeShape;
    public List<Transform> variableTwoShape;
    public List<Transform> armsShape;
    public AudioClip GoVoice;
    public GameObject ragdollObject;
    public UnityEvent onDieEvent;
    

    private Rigidbody rigid;
    private Animator animator;
    private AudioSource audioSource;

    private bool isPlaying;
    private bool isDead;

    // input and move
    private bool isFirstTouch;
    private Vector2 firstTouchPos;
    private Vector2 currentTouchPos;
    private float slideSpeed;
    private float moveXSpeed = 15f;
    private float decreaseSpeed = 1f;
    private float maxMoveDistance = 24.5f;
    private float rotSpeed = 30f;
    private float minDecreaseSpeed = 0.75f;
    private int fingerId;

    private int ragdollIdx;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    public void Init()
    {
        isDead = false;
        isPlaying = false;
        isFirstTouch = false;
        slideSpeed = 0;
        fingerId = int.MinValue;
        rigid.velocity = Vector3.zero; 
        firstTouchPos = Vector2.zero;
        currentTouchPos = Vector2.zero;
        animator.SetBool("Restart", true);
        animator.SetBool("StartGame", false);
        animator.SetBool("Dead", false);

        animator.applyRootMotion = false;
        stats.Init();
        SizeSetting();

        ragdollIdx = 0;
        for(var idx = 0; idx < ragdollObject.transform.childCount; idx++)
        {
            var ragdoll = ragdollObject.transform.GetChild(idx);
            var ragdollMgr = ragdoll.GetComponent<RagdollManager>();
            var rigid = ragdollMgr.rigid;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            rigid.Sleep();
            ragdoll.gameObject.SetActive(false);
        }
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
        rigid.velocity = new Vector3(slideSpeed * moveXSpeed, rigid.velocity.y, stats.currentSpeed * decreaseSpeed);
    }

    public void horizontalMove(Touch touch)
    {
        Vector2 dir = Vector2.zero;
        switch (touch.phase)
        {
            case TouchPhase.Began:
                isFirstTouch = true;
                if (fingerId == int.MinValue)
                    fingerId = touch.fingerId;
                firstTouchPos = Camera.main.ScreenToViewportPoint(touch.position);
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (fingerId == touch.fingerId)
                {
                    if (!isFirstTouch)
                    {
                        isFirstTouch = true;
                        firstTouchPos = Camera.main.ScreenToViewportPoint(touch.position);
                    }
                    currentTouchPos = Camera.main.ScreenToViewportPoint(touch.position);
                    dir = currentTouchPos - firstTouchPos;
                    slideSpeed = dir.x * 3f;
                    decreaseSpeed = Mathf.Lerp(1f, minDecreaseSpeed, Mathf.Abs(slideSpeed));
                }
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                fingerId = int.MinValue;
                isFirstTouch = false;
                firstTouchPos = Vector2.zero;
                currentTouchPos = Vector2.zero;
                slideSpeed = 0;
                break;
        }

        var leftMoveLimit = dir.x < 0f && transform.position.x < -maxMoveDistance;
        var rightMoveLimit = dir.x > 0f && transform.position.x > maxMoveDistance;
        if (leftMoveLimit || rightMoveLimit)
            slideSpeed = 0;

        var rot = Mathf.Clamp(slideSpeed * rotSpeed, -20f, 20f);
        transform.rotation = Quaternion.Euler(0f, rot, 0f);
    }

    public void horizontalMove(float h)
    {
        slideSpeed = h;

        var rot = Mathf.Clamp(slideSpeed * rotSpeed, -20f, 20f);
        transform.rotation = Quaternion.Euler(0f, rot, 0f);
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
                        elem.OnActionByCollision(gameObject);
                    }
                }
            }
            else
            {
                var collisable = other.GetComponents<ICollisable>();
                foreach (var elem in collisable)
                {
                    elem.OnActionByCollision(gameObject);
                }
            }

            if (stats.currentSpeed <= stats.gameoverSpeed)
            {
                isDead = true;
                onDieEvent.Invoke();
            }
        }
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
        var ragdollTr = ragdollObject.transform;
        var ragdollPos = ragdollTr.position;
        ragdollTr.position = new Vector3(ragdollPos.x, ragdollPos.y + 0.2f, ragdollPos.z);

        var particleObj = ObjectPool.GetObject(PoolName.ItemParticle);
        var particle = particleObj.GetComponent<ParticleSystem>();

        particleObj.transform.position = gameObject.transform.position + new Vector3(0f, 2f);
        particleObj.transform.SetParent(gameObject.transform);
        particle.Play();
        SizeSetting();
    }

    public void MsgGetPenalty()
    {
        stats.MsgGetPenalty();
        var ragdollTr = ragdollObject.transform;
        var ragdollPos = ragdollTr.position;
        ragdollTr.position = new Vector3(ragdollPos.x, ragdollPos.y - 0.1f, ragdollPos.z);
        SizeSetting();
    }

    public void MsgGetRagdoll(EnemyStats stats)
    {
        if(ragdollIdx < ragdollObject.transform.childCount)
        {
            var ragdoll = ragdollObject.transform.GetChild(ragdollIdx);
            ragdoll.gameObject.SetActive(true);

            var ragdollMgr = ragdoll.GetComponent<RagdollManager>();
            ragdollMgr.rigid.WakeUp();
            ragdollMgr.SetStats(stats);

            ragdollIdx++;
        }
    }
}