using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
//목표 : 적을 FSM 다이어그램에 따라 동작시키고 싶다.
//필요속성 : 적 상태

//목표2 : 플레이어와의 거리를 측정해서 특정 상태로 만들어준다.
//필요 속성 : 플레이어와의 거리, 플레이어 트랜스폼

//목표3 : 적의 상태가 Move일 때, 플레이어와의 거리가 공격범위 밖이면 적이 플레이어를 따라간다.
//필요속성3: 이동 속도, 적의 이동을 위한 캐릭터 컨트롤러

//목표4 : 플레이어가 공격 범위 내에 들어왔으면 특정시간마다 때린다.
//필요 속성 : 현재시간, 특정시간
//목표5: 플레이어를 따라가다가 초기 위치에서 일정 거리를 벗어나면 초기 위치로 돌아온다.
//필요 속성: 초기위치, 이동가능 범위
//목표6 : 초기위치로 돌아간다. 특정 거리 이내면 idle 상태로 전환.
//필요 속성 : 특정 거리
//목표7. 플레이어의 공격을 받으면 hitDamage만큼 에너미의 hp를 감소 시킨다.
//목표 : Idle 상태에서 Move 상태로 Animation을 전환한다.
//필요 속성: 애니메이터
//목표 : 네비게이션 에이전트의 최소 거리를 입력해 주고, 플레이어를 따라갈 수 있도록 한다. 
//필요속성 : 네비게이션 에이전트
public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    EnemyState enemyState;
    //필요 속성 : 플레이어와의 거리, 플레이어 트랜스폼
    public float findDistance = 5f;
    Transform player;
    //필요속성3: 이동 속도, 적의 이동을 위한 캐릭터 컨트롤러, 공격 범위
    public float moveSpeed;
    CharacterController characterController;
    public float attackDist = 2f;
    //필요 속성 : 현재시간, 특정시간
    float currentTime;
    public float attackTime = 2f;
    public int attckPower = 1;
    //필요 속성: 초기위치, 이동가능 범위
    Vector3 originPos;
    public float moveDist = 20f;
    //필요 속성 : 특정 거리
    float returnDist = 0.3f;
    public int hp = 3;
    int maxHp = 3;
    public Slider hpSlider;

    Animator animator;

    NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Idle;
        player = GameObject.Find("Player").transform;
        characterController = gameObject.GetComponent<CharacterController>();
        originPos = transform.position;
        maxHp = hp;

        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Start)
            return;
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                // Die();
                break;
        }
        hpSlider.value = (float)hp / maxHp;
    }
    private void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcesee());
    }
    IEnumerator DieProcesee()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(2f);
        print("사망");
        Destroy(gameObject);
    }
    public void DamageAction(int damage)
    {
        //만약, 이미 에너미가 피격, 사망상태라면 데미지 ㄴㄴ
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
            return;
        hp -= damage;

        //이동을 멈추고 경로를 초기화한다.
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();

        if (hp > 0)
        {
            enemyState = EnemyState.Damaged;
            print("상태전환 : AnyState to Damaged");
            Damaged();
        }
        else
        {
            enemyState = EnemyState.Damaged;
            print("상태전환 : AnyState to Die");
            Die();
        }
    }

    private void Damaged()
    {
        //피격 모션 0.5초 간
        animator.SetTrigger("Damaged");
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        //피격 모션 시간만큼 기다린다.
        yield return new WaitForSeconds(0.5f);

        enemyState = EnemyState.Move;
        print("상태전환 : Damaged to Move");
        animator.SetTrigger("Damaged2Move");

    }
    //목표6 : 초기위치로 돌아간다. 특정 거리 이내면 idle 상태로 전환.
    private void Return()
    {
        float distanceToOringPos = (originPos - transform.position).magnitude;
        if (distanceToOringPos > returnDist)
        {
            //Vector3 dir = (originPos - transform.position).normalized;
            //characterController.Move(dir * moveSpeed * Time.deltaTime);
            //transform.forward = dir;


            navMeshAgent.SetDestination(originPos);
            //네비게이션으로 접근하는 최소 거리 초기화
            navMeshAgent.stoppingDistance = 0;
        }
        else
        {
            //이동을 멈추고 경로를 초기화한다.
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            enemyState = EnemyState.Idle;
            print("상태전환 : Return to Idle");
            animator.SetTrigger("Return2Idle");
        }
    }
    //목표4 : 플레이어가 공격 범위 내에 들어왔으면 때린다.
    private void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < attackDist)
        {
            //특정시간마다 때린다.
            currentTime += Time.deltaTime;
            if (currentTime >= attackTime)
            {
                if (player.gameObject.GetComponent<PlayerMove>().hp > 0)
                    player.gameObject.GetComponent<PlayerMove>().DamageAction(attckPower);
                print("공격!");
                currentTime = 0;

                animator.SetTrigger("AttackDelay2Attack");
            }
        }
        else
        {
            //공격 범위로 들어오면 공격상태로 전환
            enemyState = EnemyState.Move;
            print("상태전환 : Attack to Move");
            animator.SetTrigger("Attack2Move");
        }
    }
    public void AttackAction()
    {

    }
    //목표3 : 적의 상태가 Move일 때, 플레이어와의 거리가 공격범위 밖이면 적이 플레이어를 따라간다.
    private void Move()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToOringPos = (originPos - transform.position).magnitude;
        if (distanceToOringPos > moveDist)
        {
            enemyState = EnemyState.Return;
            print("상태전환 : Move to Return");
            animator.SetTrigger("Move2Return");
        }
        else if (distanceToPlayer > attackDist)
        {
            //Vector3 dir = (player.position - transform.position).normalized;
            ////플레이어를 따라간다.
            //characterController.Move(dir * moveSpeed * Time.deltaTime);
            //transform.forward = dir;

            //이동을 멈추고 경로를 초기화한다.
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();

            navMeshAgent.stoppingDistance = attackDist;
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            //공격 범위로 들어오면 공격상태로 전환
            enemyState = EnemyState.Attack;
            currentTime = attackTime;
            print("상태전환 : Move to Attack");
            animator.SetTrigger("Move2AttackDelay");
        }
    }

    private void Idle()
    {
        //목표2 : 플레이어와의 거리를 측정해서 특정 상태로 만들어준다.
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        //현재 플레이어와의 거리가 특정 범위 내면 상태를 Move로 바꾼다.
        if (distanceToPlayer < findDistance)
        {
            enemyState = EnemyState.Move;
            print("상태전환 : Idle to Move");

            animator.SetTrigger("Idle2Move");
        }
    }

}
