using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//우클릭으로 폭탄을 특정 방향으로 발4
//필요한거 폭탄, 발사위치, 방향
//순서 1 마우스 우클릭
//폭탄 게임오브젝트를 생성하고 firePosition에 위치시킨다.
//폭탄의 리지드바디에 카메라 정면방향으로 힘을 가한다.
public class PlayerFire : MonoBehaviour
{
    public GameObject bomb;
    public GameObject firePosition;
    public float power;

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            GameObject instance = Instantiate(bomb);
            instance.transform.position = firePosition.transform.position;

            Rigidbody rb = instance.GetComponent<Rigidbody>();
            rb.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);
        }
    }
}
