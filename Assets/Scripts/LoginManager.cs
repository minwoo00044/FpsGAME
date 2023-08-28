using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//������� ���������� �Է����� �����ϰų�(ȸ������) ����� �����͸� �о �������� ���� ���ο� ���� �α����ϰ� �ʹ�.
//�ʿ�Ӽ� : id inputfiled, pw inputfield, ���� �ؽ�Ʈ
//��ǥ2 : ���̵�� �н����带 �����ؼ� ȸ������
public class LoginManager : MonoBehaviour
{
    public TMP_InputField id;
    public TMP_InputField pw;
    public TMP_Text authTxt;

    // Start is called before the first frame update
    void Start()
    {
        authTxt.text = string.Empty;
    }

    // Update is called once per frame
    public void SignUp()
    {
        if(!CheckInput(id.text, pw.text))
        { 
            return; 
        }
        if (!PlayerPrefs.HasKey(id.text))
        {
            authTxt.text = "���ԿϷ�!";
            PlayerPrefs.SetString(id.text, pw.text);
        }
        else
        {
            authTxt.text = "�̹� ������� ���̵��Դϴ�.";
        }
    }
    public void Login()
    {
        if (!CheckInput(id.text, pw.text))
        {
            return;
        }
        string password = PlayerPrefs.GetString(id.text);
        if (PlayerPrefs.HasKey(id.text))
        {
            if (password == pw.text)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                authTxt.text = "�Է��Ͻ� ���̵�� �н����尡 ��ġ���� �ʽ��ϴ�.";
            }
        }
        else
        {
            authTxt.text = "�Է��Ͻ� ���̵� �������� �ʽ��ϴ�.";
        }
    }
    bool CheckInput(string _id, string _pw)
    {
        if(_id == ""||_pw == "")
        {
            authTxt.text = "���̵� �Ǵ� �н����带 �Է����ּ���";
            return false;
        }
        else
        {
            return true;
        }
    }
}
