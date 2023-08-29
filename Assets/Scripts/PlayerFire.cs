using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
//목표3 : 레이가 부딪힌 대상이 에너미라면 데미지를 주겠다.
//목적4 : 이동 블랜드 트리의 파라메터 값이 0일 때 Attack Trigger를 시전하겠다.
//필요속성 : 자식 오브젝트의 애니메이터
//목적5 : 특정 키 입력으로 무기모드 전환
//필요 속성: 무기모드 열거형 변수, 줌 확인 변수
//목적6 : 총을 발사할 때 빵야빵야
public class PlayerFire : MonoBehaviour
{
    public GameObject bomb;
    public GameObject firePosition;
    public float power;

    //필요속성 : 피격효과 게임오브젝트, 이펙트의 파티클 시스템.
    public GameObject hitEffect;
    ParticleSystem particleSystem0;
    public int weaponPower = 2;
    Animator animator;
    public enum WeaponMode
    {
        Noraml,
        Sniper
    }
    public WeaponMode weaponMode = WeaponMode.Noraml;
    bool isZoomMode = false;
    public TMP_Text WeaponModeText;
    public GameObject[] fireFlashEffs;

    public GameObject crossHair0;
    public GameObject crossHair1;
    public GameObject rifleImage0;
    public GameObject rifleImage1;
    public GameObject granadeImage;
    private void Start()
    {
        particleSystem0 = hitEffect.GetComponent<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        WeaponModeText.text = weaponMode.ToString() + "MODE";
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
        //Cursor.lockState = CursorLockMode.Locked;

        if (GameManager.Instance.gameState != GameManager.GameState.Start)
            return;
        if (Input.GetMouseButtonDown(1))
        {
            switch (weaponMode)
            {
                //노멀모드 일때는 오른쪽 버튼 누르면 수류탄 발사
                case WeaponMode.Noraml:
                    GameObject instance = Instantiate(bomb);
                    instance.transform.position = firePosition.transform.position;
                    Rigidbody rb = instance.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);
                    break;
                //스나이퍼 모드 : 마우스 오른쪽 버튼을 누르면 줌인
                case WeaponMode.Sniper:
                    if(!isZoomMode)
                    {
                        Camera.main.fieldOfView = 15;
                        crossHair0.SetActive(false);
                        crossHair1.SetActive(true);
                        isZoomMode = true;
                    }
                    else
                    {

                        crossHair0.SetActive(true);
                        crossHair1.SetActive(false);
                        Camera.main.fieldOfView = 60;
                        isZoomMode = false;
                    }
                    break;
            }

        }
        if (Input.GetMouseButtonDown(0))
        {
            if (animator.GetFloat("MoveMotion") == 0)
            {
                animator.SetTrigger("Attack");
            }
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

                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM fsm = hitInfo.transform.gameObject.GetComponent<EnemyFSM>();
                    fsm.DamageAction(weaponPower);
                }

            }
            //총구 이펙트 실행
            StartCoroutine(ShootEffOn(0.05f));
        }
        //키보드 숫자 1번 누르면, 무기 모드를 노말모드로
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponMode = WeaponMode.Noraml;
            Camera.main.fieldOfView = 60f;
            WeaponModeText.text = weaponMode.ToString() + "MODE";

            crossHair0.SetActive(true);
            crossHair1.SetActive(false);
            rifleImage0.SetActive(true);
            rifleImage1.SetActive(false);
            granadeImage.SetActive(true);
        }
        //키보드 숫자 2번 누르면, 무기 모드를 저격모드로
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponMode = WeaponMode.Sniper;
            WeaponModeText.text = weaponMode.ToString() + "MODE";


            rifleImage0.SetActive(false);
            rifleImage1.SetActive(true);
            granadeImage.SetActive(false);
        }
    }

    IEnumerator ShootEffOn(float duration)
    {
        int randNum = Random.Range(0, fireFlashEffs.Length - 1);
        fireFlashEffs[randNum].SetActive(true);

        yield return new WaitForSeconds(duration);

        fireFlashEffs[randNum].SetActive(false);

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
