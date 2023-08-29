using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//목표1 : 다음 씬을 비동기 방식으로 로드
//필요 속성: 다음 진행할 씬 번호
//목표2 : 현재 씬에 로딩 진행률을 슬라이더로 표현하고 싶다.
//필요속성 : 로딩 슬라이더, 로딩텍스트
public class LoadingNextScene : MonoBehaviour
{
    public int scneneNumber = 2;
    public Slider loadingSlider;
    public TMP_Text loadingText;

    private void Start()
    {
        //비동기 씬을 코루틴 함수로 로드한다.
        StartCoroutine(AsyncNextScene(scneneNumber));
    }

    IEnumerator AsyncNextScene(int num)
    {
        //지정된 씬을 비동기 방식으로 만든다.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(num);
        asyncOperation.allowSceneActivation = false;
        //목표2 : 현재 씬에 로딩 진행률을 슬라이더로 표현하고 싶다.
        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;
            loadingText.text = (asyncOperation.progress * 100).ToString() + "%";

            //특정 진행률일 때 다음 씬을 보여주고 싶다.
            if(asyncOperation.progress >= 0.9f) 
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
