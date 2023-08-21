using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//목적 : wasd를 누르면 캐릭터를 그 방향으로 이동시키고 싶다.
//필요속성 : 이동속도

//목적2 : 스페이스 누르면 수직으로 점프하고 싶다.
//필요속성 : 캐릭터 컨트롤러, 중력 변수, 수직 속력 변수. , 점프파워, 점프 상태 변수
//2-1. 캐릭터 수직 속도에 중력을 적용하고 싶다.
//2-2. 캐릭터 컨트롤러로 나를 이동시키고 싶다.
//2-3 스페이스를 누르면 수직속도에 점프파워 적용하고 싶다.
// 2-4 점프 중인지 확인하고 싶다.

//목적3. 점프 중인지 확인하고 점프 중이면 점프전 상태로 초기화 하고 싶다.
//그리고
// 목적4 : 플레이어가 피격 당하면 hp 를 damage 만큼 깎는다.
//필요 속성 : hp
public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    public float jumpPower = 10;
    CharacterController characterController;
    float gravity = -20f;
    float yVelocity = 0;
    public bool isJumping = false;
    public int hp = 10;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        //입력

        if (isJumping && characterController.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            
        }
        //바닥에 닿아있을때 수직 속도 초기화
        else if (characterController.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
        }
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //이동방향 설정

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);

        //중력500배
        yVelocity = yVelocity + gravity * Time.deltaTime;
        dir.y = yVelocity;
        //만약 점프 중이었다면 점프 전 상태로 초기화 하고 싶다.
        //이동
        //transform.position += dir * speed * Time.deltaTime;
        //2-2. 캐릭터 컨트롤러로 나를 이동시키고 싶다.
        characterController.Move(dir * speed * Time.deltaTime);
    }

    public void DamageAction(int damage)
    {
        hp -= damage;
    }
}
