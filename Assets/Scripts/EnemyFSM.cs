using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//목표 : 적을 FSM 다이어그램에 따라 동작시키고 싶다.
//필요속성 : 적 상태

//목표2 : 플레이어와의 거리를 측정해서 특정 상태로 만들어준다.
//필요 속성 : 플레이어와의 거리, 플레이어 트랜스폼

//목표3 : 적의 상태가 Move일 때, 플레이어와의 거리가 공격범위 밖이면 적이 플레이어를 따라간다.
//필요속성3: 이동 속도, 적의 이동을 위한 캐릭터 컨트롤러

//목표4 : 플레이어가 공격 범위 내에 들어왔으면 특정시간마다 때린다.
//필요 속성 : 현재시간, 특정시간
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


    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Idle;
        player = GameObject.Find("Player").transform;
        characterController = gameObject.GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
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
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    private void Die()
    {
    }

    private void Damaged()
    {
    }

    private void Return()
    {
    }
    //목표4 : 플레이어가 공격 범위 내에 들어왔으면 때린다.
    private void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < attackDist)
        {
            //특정시간마다 때린다.
            currentTime += Time.deltaTime;
            if(currentTime >= attackTime)
            {
                print("공격!");
                currentTime = 0;
            }
        }
        else
        {
            //공격 범위로 들어오면 공격상태로 전환
            enemyState = EnemyState.Move;
            print("상태전환 : Attack to Move");
        }
    }
    //목표3 : 적의 상태가 Move일 때, 플레이어와의 거리가 공격범위 밖이면 적이 플레이어를 따라간다.
    private void Move()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackDist)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            //플레이어를 따라간다.
            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            //공격 범위로 들어오면 공격상태로 전환
            enemyState = EnemyState.Attack;
            print("상태전환 : Move to Attack");
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
        }
    }
}
