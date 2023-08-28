using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//사용자의 개인정보를 입려갛여 저장하거나(회원가입) 저장된 데이터를 읽어서 개인정보 저장 여부에 따라 로그인하고 싶다.
//필요속성 : id inputfiled, pw inputfield, 인증 텍스트
//목표2 : 아이디와 패스워드를 저장해서 회원가입
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
            authTxt.text = "가입완료!";
            PlayerPrefs.SetString(id.text, pw.text);
        }
        else
        {
            authTxt.text = "이미 사용중인 아이디입니다.";
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
                authTxt.text = "입력하신 아이디와 패스워드가 일치하지 않습니다.";
            }
        }
        else
        {
            authTxt.text = "입력하신 아이디가 존재하지 않습니다.";
        }
    }
    bool CheckInput(string _id, string _pw)
    {
        if(_id == ""||_pw == "")
        {
            authTxt.text = "아이디 또는 패스워드를 입력해주세요";
            return false;
        }
        else
        {
            return true;
        }
    }
}
