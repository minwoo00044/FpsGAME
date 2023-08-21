
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
//��ǥ5: �÷��̾ ���󰡴ٰ� �ʱ� ��ġ���� ���� �Ÿ��� ����� �ʱ� ��ġ�� ���ƿ´�.
//�ʿ� �Ӽ�: �ʱ���ġ, �̵����� ����
//��ǥ6 : �ʱ���ġ�� ���ư���. Ư�� �Ÿ� �̳��� idle ���·� ��ȯ.
//�ʿ� �Ӽ� : Ư�� �Ÿ�
//��ǥ7. �÷��̾��� ������ ������ hitDamage��ŭ ���ʹ��� hp�� ���� ��Ų��.

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
    public int attckPower = 1;
    //�ʿ� �Ӽ�: �ʱ���ġ, �̵����� ����
    Vector3 originPos;
    public float moveDist = 20f;
    //�ʿ� �Ӽ� : Ư�� �Ÿ�
    float returnDist = 0.3f;
    public int hp = 3;
    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Idle;
        player = GameObject.Find("Player").transform;
        characterController = gameObject.GetComponent<CharacterController>();
        originPos = transform.position;
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
                //Damaged();
                break;
            case EnemyState.Die:
               // Die();
                break;
        }
    }

    private void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcesee());
    }
    IEnumerator DieProcesee()
    {
        yield return new WaitForSeconds(2f);
        print("���");
        Destroy(gameObject);
    }
    public void DamageAction(int damage)
    {
        //����, �̹� ���ʹ̰� �ǰ�, ������¶�� ������ ����
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
            return;
        hp -= damage;
        if(hp > 0)
        {
            enemyState = EnemyState.Damaged;
            print("������ȯ : AnyState to Damaged");
            Damaged();
        }
        else
        {
            enemyState = EnemyState.Damaged;
            print("������ȯ : AnyState to Die");
            Die();
        }
    }

    private void Damaged()
    {
        //�ǰ� ��� 0.5�� ��

        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        //�ǰ� ��� �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(0.5f);

        enemyState = EnemyState.Move;
        print("������ȯ : Damaged to Move");

    }
    //��ǥ6 : �ʱ���ġ�� ���ư���. Ư�� �Ÿ� �̳��� idle ���·� ��ȯ.
    private void Return()
    {
        float distanceToOringPos = (originPos - transform.position).magnitude;
        if(distanceToOringPos > returnDist)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }
        else 
        {
            enemyState = EnemyState.Idle;
            print("������ȯ : Return to Idle");
        }
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
                player.gameObject.GetComponent<PlayerMove>().DamageAction(attckPower);
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
        float distanceToOringPos = (originPos - transform.position).magnitude;
        if(distanceToOringPos > moveDist)
        {
            enemyState = EnemyState.Return;
            print("������ȯ : Move to Return");
        }
        else if (distanceToPlayer > attackDist)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            //�÷��̾ ���󰣴�.
            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            //���� ������ ������ ���ݻ��·� ��ȯ
            enemyState = EnemyState.Attack;
            currentTime = attackTime;
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
