using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//목적: 게임의 상태를 구별하고, 게임의 시작과 끝을 TextUI로 표현하고 싶다.
//필요속성 : 게임상태 열거형 변수, TextUI
//목적2 : 2초 후 레디 상태에서 게임이 시작된다.
//목적3 : Ready 상태일 때는 플레이어, 적이 움직일 수 없도록 한다.
//목적4 플레이어의 hp가 0보다 작으면 상태 텍스트를 GameOver로 바꿔주고 상태도 슥삭
//필요속성: playerMove
//목적5 : 플레이어의 hp가 0 이하면 애님 멈춘다.
//필요 속성: 플레이어의 애니메이터 컴포넌트
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //필요속성 : 게임상태 열거형 변수, TextUI
    public enum GameState
    {
        Ready,
        Start,
        GameOver
    }

    public GameState gameState = GameState.Ready;
    public TMP_Text gameStateText;
    public PlayerMove playerMove;
    Animator animator;
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
            gameState = GameState.GameOver;
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckGameOver();
    }
}
