using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��Ŭ������ ��ź�� Ư�� �������� ��4
//�ʿ��Ѱ� ��ź, �߻���ġ, ����
//���� 1 ���콺 ��Ŭ��
//��ź ���ӿ�����Ʈ�� �����ϰ� firePosition�� ��ġ��Ų��.
//��ź�� ������ٵ� ī�޶� ����������� ���� ���Ѵ�.
//����2: ���콺 ���� ��ư ������ �ü� �������� �ѽ��
//2-2 ���̸� �����ϰ� �߻� ��ġ�� �߻� ���� ����
//2-3 ���̰� �ε��� ����� ������ ������ �� �ִ� ���� �����.
//2-4 ���̸� �߻��ϰ�, �ε��� ��ü�� ������ �� ��ġ�� �ǰ� ȿ���� �����.
//�ʿ�Ӽ� : �ǰ�ȿ�� ���ӿ�����Ʈ, ����Ʈ�� ��ƼŬ �ý���.
public class PlayerFire : MonoBehaviour
{
    public GameObject bomb;
    public GameObject firePosition;
    public float power;

    //�ʿ�Ӽ� : �ǰ�ȿ�� ���ӿ�����Ʈ, ����Ʈ�� ��ƼŬ �ý���.
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
        //print(string.Format("��: {0}, ������: {1}", quotint, remainder));
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
                print("�Ÿ�" + hitInfo.distance);
                //�ε��� ��ü ������ �ǰ�ȿ��(���� ���� ��������) �����
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                //�ǰ� ����Ʈ ���
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
