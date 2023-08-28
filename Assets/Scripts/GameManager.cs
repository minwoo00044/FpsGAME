using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

//����: ������ ���¸� �����ϰ�, ������ ���۰� ���� TextUI�� ǥ���ϰ� �ʹ�.
//�ʿ�Ӽ� : ���ӻ��� ������ ����, TextUI
//����2 : 2�� �� ���� ���¿��� ������ ���۵ȴ�.
//����3 : Ready ������ ���� �÷��̾�, ���� ������ �� ������ �Ѵ�.
//����4 �÷��̾��� hp�� 0���� ������ ���� �ؽ�Ʈ�� GameOver�� �ٲ��ְ� ���µ� ����
//�ʿ�Ӽ�: playerMove
//����5 : �÷��̾��� hp�� 0 ���ϸ� �ִ� �����.
//�ʿ� �Ӽ�: �÷��̾��� �ִϸ����� ������Ʈ
//����6 : Setting ��ư ������ option ui ����
//�ʿ�Ӽ� : Optionui ���ӿ�����Ʈ, �Ͻ�����
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //�ʿ�Ӽ� : ���ӻ��� ������ ����, TextUI
    public enum GameState
    {
        Ready,
        Start,
        Pause,
        GameOver
    }

    public GameState gameState = GameState.Ready;
    public TMP_Text gameStateText;
    public PlayerMove playerMove;
    Animator animator;
    public GameObject optionUI;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    void Start()
    {
        playerMove = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        gameStateText.text = "Ready";
        gameStateText.color = new Color32(255, 185, 0, 255);
        animator = playerMove.GetComponentInChildren<Animator>();
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2f);
        gameStateText.text = "Game Start";
        gameStateText.color = new Color32(0, 255, 0, 255);
        yield return new WaitForSeconds(0.5f);
        gameStateText.gameObject.SetActive(false);
        gameState = GameState.Start;
    }

    void CheckGameOver()
    {
        if(playerMove.hp <= 0)
        {
            animator.SetFloat("MoveMotion", 0f);
            gameStateText.gameObject.SetActive(true);
            gameStateText.text = "Game Over";
            gameStateText.color = new Color32(255, 255, 255, 255);
            GameObject retryBtn = gameStateText.transform.GetChild(0).gameObject;
            GameObject quitBtn = gameStateText.transform.GetChild(1).gameObject;
            retryBtn.SetActive(true);
            quitBtn.SetActive(true);
            playerMove.hpSlider.gameObject.SetActive(false);
            gameState = GameState.GameOver;
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckGameOver();
    }
    public void OpenOptionWindow()
    {
        optionUI.SetActive(true);
        Time.timeScale = 0f;
        gameState = GameState.Pause;
    }
    public void CloseOptionWindow()
    {
        optionUI.SetActive(false);
        Time.timeScale = 1f;
        gameState = GameState.Start;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
