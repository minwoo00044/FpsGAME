using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
// 현재 플레이어의 hp(%)를 hp슬라이더에 적용한다.
//필요속성 : hp, maxhp, slider
//목적5: 공격을 당했을 때 hitImage를 켰다가 꺼준다.
//필요속성: hitImage 게임오브젝트

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    public float jumpPower = 10;
    CharacterController characterController;
    float gravity = -20f;
    float yVelocity = 0;
    public bool isJumping = false;
    public int hp = 10;
    int maxHp = 10;
    public Slider hpSlider;

    public GameObject hitImage;
    float currentTime;
    public float hitImageEndTime;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        maxHp = hp;
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
        // 현재 플레이어의 hp(%)를 hp슬라이더에 적용한다.
        hpSlider.value = (float)hp / maxHp;
    }

    public void DamageAction(int damage)
    {
        hp -= damage;
        //목적5: 공격을 당했을 때 hitImage를 켰다가 꺼준다.
        if(hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
        else if(hp <= 0)
        {
            StartCoroutine(PlayDieEffect());
        }
    }
    IEnumerator PlayHitEffect()
    {
        hitImage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitImage.SetActive(false);
    }
    IEnumerator PlayDieEffect() 
    {
        hitImage.SetActive(true);
        Color color = hitImage.GetComponent<Image>().color;
        while (true) 
        {
            currentTime += Time.deltaTime;
            yield return null;
            color.a = Mathf.Lerp(0, 1, currentTime / hitImageEndTime);
            hitImage.GetComponent<Image>().color = color;

            if (currentTime >= hitImageEndTime)
            {
                currentTime = 0;
                break;
            }
        }
        
    }
}
