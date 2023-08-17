using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��Ŭ������ ��ź�� Ư�� �������� ��4
//�ʿ��Ѱ� ��ź, �߻���ġ, ����
//���� 1 ���콺 ��Ŭ��
//��ź ���ӿ�����Ʈ�� �����ϰ� firePosition�� ��ġ��Ų��.
//��ź�� ������ٵ� ī�޶� ����������� ���� ���Ѵ�.
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
