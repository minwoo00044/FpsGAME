using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//우클릭으로 폭탄을 특정 방향으로 발4
//필요한거 폭탄, 발사위치, 방향
//순서 1 마우스 우클릭
//폭탄 게임오브젝트를 생성하고 firePosition에 위치시킨다.
//폭탄의 리지드바디에 카메라 정면방향으로 힘을 가한다.
//목적2: 마우스 왼쪽 버튼 누르면 시선 방향으로 총쏘기
//2-2 레이를 생성하고 발사 위치와 발사 방향 설정
//2-3 레이가 부딪힌 대상의 정보를 저장할 수 있는 변수 만든다.
//2-4 레이를 발사하고, 부딪힌 물체가 있으면 그 위치에 피격 효과를 만든다.
//필요속성 : 피격효과 게임오브젝트, 이펙트의 파티클 시스템.
public class PlayerFire : MonoBehaviour
{
    public GameObject bomb;
    public GameObject firePosition;
    public float power;

    //필요속성 : 피격효과 게임오브젝트, 이펙트의 파티클 시스템.
    public GameObject hitEffect;
    ParticleSystem particleSystem0;
    private void Start()
    {
        particleSystem0 = hitEffect.GetComponent<ParticleSystem>();
        //int x = 3;
        //int y = 4;
        //Swap(ref x, ref y);
        //print(string.Format("x : {0}, y:{1}", x, y));

        //int a = 3;
        //int b = 4;
        //int quotint;
        //int remainder;

        //quotint = Divide(a, b, out remainder);
        //print(string.Format("몫: {0}, 나머지: {1}", quotint, remainder));
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            GameObject instance = Instantiate(bomb);
            instance.transform.position = firePosition.transform.position;
            Rigidbody rb = instance.GetComponent<Rigidbody>();
            rb.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(ray, out hitInfo))
            {
                print("거리" + hitInfo.distance);
                //부딪힌 물체 있으면 피격효과(법선 벡터 방향으로) 만들긔
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                //피격 이펙트 재생
                particleSystem0.Play();
            }
        }
    }

    //public void Swap(ref int a, ref int b)
    //{
    //    int temp = a;
    //    a = b;
    //    b = temp;
    //}

    //public int Divide(int a, int b, out int remainder)
    //{
    //    remainder = a % b;
    //    return a / b;
    //}

}
