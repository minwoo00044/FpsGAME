using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ǥ : ���� FSM ���̾�׷��� ���� ���۽�Ű�� �ʹ�.
//�ʿ�Ӽ� : �� ����

//��ǥ2 : �÷��̾���� �Ÿ��� �����ؼ� Ư�� ���·� ������ش�.
//�ʿ� �Ӽ� : �÷��̾���� �Ÿ�, �÷��̾� Ʈ������

//��ǥ3 : ���� ���°� Move�� ��, �÷��̾���� �Ÿ��� ���ݹ��� ���̸� ���� �÷��̾ ���󰣴�.
//�ʿ�Ӽ�3: �̵� �ӵ�, ���� �̵��� ���� ĳ���� ��Ʈ�ѷ�

//��ǥ4 : �÷��̾ ���� ���� ���� �������� Ư���ð����� ������.
//�ʿ� �Ӽ� : ����ð�, Ư���ð�
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
    //�ʿ� �Ӽ� : �÷��̾���� �Ÿ�, �÷��̾� Ʈ������
    public float findDistance = 5f;
    Transform player;
    //�ʿ�Ӽ�3: �̵� �ӵ�, ���� �̵��� ���� ĳ���� ��Ʈ�ѷ�, ���� ����
    public float moveSpeed;
    CharacterController characterController;
    public float attackDist = 2f;
    //�ʿ� �Ӽ� : ����ð�, Ư���ð�
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
    //��ǥ4 : �÷��̾ ���� ���� ���� �������� ������.
    private void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < attackDist)
        {
            //Ư���ð����� ������.
            currentTime += Time.deltaTime;
            if(currentTime >= attackTime)
            {
                print("����!");
                currentTime = 0;
            }
        }
        else
        {
            //���� ������ ������ ���ݻ��·� ��ȯ
            enemyState = EnemyState.Move;
            print("������ȯ : Attack to Move");
        }
    }
    //��ǥ3 : ���� ���°� Move�� ��, �÷��̾���� �Ÿ��� ���ݹ��� ���̸� ���� �÷��̾ ���󰣴�.
    private void Move()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackDist)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            //�÷��̾ ���󰣴�.
            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            //���� ������ ������ ���ݻ��·� ��ȯ
            enemyState = EnemyState.Attack;
            print("������ȯ : Move to Attack");
        }
    }

    private void Idle()
    {
        //��ǥ2 : �÷��̾���� �Ÿ��� �����ؼ� Ư�� ���·� ������ش�.
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        //���� �÷��̾���� �Ÿ��� Ư�� ���� ���� ���¸� Move�� �ٲ۴�.
        if (distanceToPlayer < findDistance) 
        {
            enemyState = EnemyState.Move;
            print("������ȯ : Idle to Move");
        }
    }
}
