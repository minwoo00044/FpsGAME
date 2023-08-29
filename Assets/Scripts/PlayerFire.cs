using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
//��ǥ3 : ���̰� �ε��� ����� ���ʹ̶�� �������� �ְڴ�.
//����4 : �̵� ���� Ʈ���� �Ķ���� ���� 0�� �� Attack Trigger�� �����ϰڴ�.
//�ʿ�Ӽ� : �ڽ� ������Ʈ�� �ִϸ�����
//����5 : Ư�� Ű �Է����� ������ ��ȯ
//�ʿ� �Ӽ�: ������ ������ ����, �� Ȯ�� ����
//����6 : ���� �߻��� �� ���߻���
public class PlayerFire : MonoBehaviour
{
    public GameObject bomb;
    public GameObject firePosition;
    public float power;

    //�ʿ�Ӽ� : �ǰ�ȿ�� ���ӿ�����Ʈ, ����Ʈ�� ��ƼŬ �ý���.
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
        //print(string.Format("��: {0}, ������: {1}", quotint, remainder));
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
                //��ָ�� �϶��� ������ ��ư ������ ����ź �߻�
                case WeaponMode.Noraml:
                    GameObject instance = Instantiate(bomb);
                    instance.transform.position = firePosition.transform.position;
                    Rigidbody rb = instance.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);
                    break;
                //�������� ��� : ���콺 ������ ��ư�� ������ ����
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
                print("�Ÿ�" + hitInfo.distance);
                //�ε��� ��ü ������ �ǰ�ȿ��(���� ���� ��������) �����
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                //�ǰ� ����Ʈ ���
                particleSystem0.Play();

                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM fsm = hitInfo.transform.gameObject.GetComponent<EnemyFSM>();
                    fsm.DamageAction(weaponPower);
                }

            }
            //�ѱ� ����Ʈ ����
            StartCoroutine(ShootEffOn(0.05f));
        }
        //Ű���� ���� 1�� ������, ���� ��带 �븻����
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
        //Ű���� ���� 2�� ������, ���� ��带 ���ݸ���
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
