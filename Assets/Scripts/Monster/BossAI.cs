using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public CharacterController controller;
    public Transform player;
    public Animator anim;
    public float speed = 2f;
    public float rotateSpeed = 15f;
    //public float chaseDistance = 5f;
    public float gravity = -9.18f;

    public Transform[] patrolPointsTF;
    public Vector3[] patrolPoints;
    PatrolState patrolState;
    ChaseState chaseState;
    private BossState currentState;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        patrolPoints = new Vector3[patrolPointsTF.Length];
        for(int i=0; i< patrolPoints.Length; i++)
        {
            patrolPoints[i] = patrolPointsTF[i].position;
        }
        patrolState = new PatrolState(this, patrolPoints);
        chaseState = new ChaseState(this);

        currentState = patrolState;
        currentState.Enter();
    }
    private Vector3 velocity;
    private void Update()
    {
        currentState.Update();

        if (controller.isGrounded)
            velocity.y = 0;
        else
            velocity.y += gravity;

        controller.Move(velocity * Time.deltaTime);
    }

    public void ChangeState(BossState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    // jinru quyu 
    public void OnEnterArea()
    {
        ChangeState(chaseState);
    }
    // likaiquyu
    public void OnExitArea()
    {
        ChangeState(patrolState);
    }
    #region PatrolState
    public class PatrolState : BossState
    {
        private Vector3[] patrolPoints;
        private int currentPointIndex = 0;

        public PatrolState(BossAI bossAI, Vector3[] patrolPoints) : base(bossAI)
        {
            this.patrolPoints = patrolPoints;
        }

        public override void Enter()
        {
            // 进入巡逻状态时的逻辑
            bossAI.anim.SetBool("Walk",true);
        }

        public override void Update()
        {
            Vector3 target = patrolPoints[currentPointIndex];
            Vector3 direction = (target - bossAI.transform.position).normalized;

            bossAI.controller.Move(direction * bossAI.speed * Time.deltaTime);

            direction.y = 0;
            bossAI.transform.rotation = Quaternion.Lerp(bossAI.transform.rotation, Quaternion.LookRotation(direction), bossAI.rotateSpeed);
            if (Vector3.Distance(bossAI.transform.position, target) < 1f)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            }
        }

        public override void Exit()
        {
            // 可以添加退出巡逻状态时的逻辑
            bossAI.anim.SetBool("Walk", false);
        }
    }
    #endregion
    #region ChaseState
    public class ChaseState : BossState
    {
        public ChaseState(BossAI bossAI) : base(bossAI) { }

        public override void Enter()
        {
            // 可以添加进入追击状态时的逻辑
            bossAI.anim.SetBool("Run", true);
        }

        public override void Update()
        {
            Vector3 direction = (bossAI.player.position - bossAI.transform.position).normalized;
            bossAI.controller.Move(direction * bossAI.speed * Time.deltaTime);

            direction.y = 0;
            bossAI.transform.rotation = Quaternion.Lerp(bossAI.transform.rotation, Quaternion.LookRotation(direction), bossAI.rotateSpeed);
        }

        public override void Exit()
        {
            // 可以添加退出追击状态时的逻辑
            bossAI.anim.SetBool("Run", false);
        }
    }
    #endregion
}

public abstract class BossState
{
    protected BossAI bossAI;

    public BossState(BossAI bossAI)
    {
        this.bossAI = bossAI;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
