using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//��ǥ1 : ���� ���� �񵿱� ������� �ε�
//�ʿ� �Ӽ�: ���� ������ �� ��ȣ
//��ǥ2 : ���� ���� �ε� ������� �����̴��� ǥ���ϰ� �ʹ�.
//�ʿ�Ӽ� : �ε� �����̴�, �ε��ؽ�Ʈ
public class LoadingNextScene : MonoBehaviour
{
    public int scneneNumber = 2;
    public Slider loadingSlider;
    public TMP_Text loadingText;

    private void Start()
    {
        //�񵿱� ���� �ڷ�ƾ �Լ��� �ε��Ѵ�.
        StartCoroutine(AsyncNextScene(scneneNumber));
    }

    IEnumerator AsyncNextScene(int num)
    {
        //������ ���� �񵿱� ������� �����.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(num);
        asyncOperation.allowSceneActivation = false;
        //��ǥ2 : ���� ���� �ε� ������� �����̴��� ǥ���ϰ� �ʹ�.
        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;
            loadingText.text = (asyncOperation.progress * 100).ToString() + "%";

            //Ư�� ������� �� ���� ���� �����ְ� �ʹ�.
            if(asyncOperation.progress >= 0.9f) 
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
