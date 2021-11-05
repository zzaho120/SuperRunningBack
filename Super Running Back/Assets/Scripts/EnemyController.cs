using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum STATE
    {
        IDLE,
        MOVE,
        TRACE,
        DIVING,
        PASSOVER
    }

    public List<EnemyStats> statsLevels;
    public EnemyStats stats;
    public float detectDistance;
    public Transform player;
    private Rigidbody rigid;
    private Animator animator;
    private STATE state;
    private bool isPlaying;
    private bool isDiving;
    private bool isDivingCheck;
    private float animaitorNomalize = 5f;
    private float divingDistance;
    public STATE State
    {
        get { return state; }
        set
        {
            state = value;
            switch (state)
            {
                case STATE.IDLE:
                    break;
                case STATE.MOVE:
                    animator.SetBool("Move", true);
                    break;
                case STATE.TRACE:
                    animator.SetBool("Trace", true);
                    break;
                case STATE.DIVING:
                    animator.SetBool("Diving", true);
                    break;
                case STATE.PASSOVER:
                    break;
            }
        }
    }

    private float DistanceFromPlayer
    {
        get
        {
            return Vector3.Distance(transform.position, player.position);
        }
    }

    private Vector3 DirectionFromPlayer
    {
        get
        {
            return player.position - transform.position;
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameManager.Instance.player.transform;
        Init();
    }

    public void Init()
    {
        isPlaying = false;
        State = STATE.IDLE;
        rigid.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0f, -180f, 0f);
        transform.localScale = new Vector3(stats.shapeSize, stats.shapeSize, stats.shapeSize);
        divingDistance = 10f + 2f * (stats.level - 1);
        animator.SetFloat("MoveX", 0.5f);
        var action = GetComponent<ActionByCollision>();
        var destroy = GetComponent<DestoryByCollision>();
        action.Init();
        destroy.Init();
    }

    public void Init(int level)
    {
        stats = statsLevels[level];
        Init();
    }

    private void FixedUpdate()
    {
        if(isPlaying)
        {
            switch (State)
            {
                case STATE.IDLE:
                    IdleUpdate();
                    break;
                case STATE.MOVE:
                    MoveUpdate();
                    break;
                case STATE.TRACE:
                    TraceUpdate();
                    break;
                case STATE.DIVING:
                    DivingUpdate();
                    break;
                case STATE.PASSOVER:
                    StartCoroutine(CoReturnObj(2f));
                    break;
            }
        }
    }

    private void IdleUpdate()
    {
        if (Camera.main.WorldToViewportPoint(transform.position).y < 1.0f)
            State = STATE.MOVE;
    }
    private void MoveUpdate()
    {
        rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, -stats.moveSpeed);

        if (detectDistance > DistanceFromPlayer)
            State = STATE.TRACE;
    }

    private void TraceUpdate()
    {
        rigid.velocity = DirectionFromPlayer.normalized * stats.moveSpeed;

        animator.SetFloat("MoveX", SetAnimationValue(DirectionFromPlayer.x));
        if (!isDiving && !isDivingCheck)
        {
            isDiving = Random.Range(0, 100) < stats.divingRate;
            isDivingCheck = true;
        }

        if (divingDistance > DistanceFromPlayer && isDiving)
            State = STATE.DIVING;
        else if (player.position.z > transform.position.z)
            State = STATE.PASSOVER;
    }

    private void DivingUpdate()
    {
        rigid.AddForce(DirectionFromPlayer.normalized * stats.weight * stats.moveSpeed);

        if (player.position.z > transform.position.z)
            State = STATE.PASSOVER;
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

    public void GameStart()
    {
        isPlaying = true;
    }

    private IEnumerator CoReturnObj(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.ReturnListObject(gameObject);
        ObjectPool.ReturnObject(PoolName.Enemy, gameObject);
    }
}
