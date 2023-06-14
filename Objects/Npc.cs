using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// NPC 대화 관련
public class Npc : MonoBehaviour
{
    KeyCode action;
    GameObject questText;
    GameObject chatWindow;
    GameObject player;
    public GameObject timer;
    public int textNum = 0;
    bool canTalk;
    public int questAccept = 0;
    public bool branchSelect;

    void Start()
    {
        // 스크립트 실행에 필요한 오브젝트 find 후 대입
        chatWindow = GameObject.Find("System").transform.Find("ChatWindow").gameObject;
        questText = chatWindow.transform.GetChild(0).gameObject;
        timer = GameObject.Find("System").transform.Find("Timer").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 접근 시 밝기를 조절하여 상호작용 활성화 여부를 표시
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
            canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 멀어지면 밝기를 되돌려 상호작용 불가 여부를 표시
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            canTalk = false;
        }
    }

    private void Update()
    {
        // 대화 창이 활성화되면 플레이어 행동 불가능
        player.GetComponent<Player>().enabled = !chatWindow.activeSelf;
        action = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ACTION"), true);
        // 분기 선택을 기다리는 상태가 아니고(분기 선택 창 활성화 시 선택지를 고르기 전까지 대화 진행 불가능)
        // 액션 버튼을 눌렀으며, 플레이어가 가까이 있다면 or 선택지를 골랐다면
        if ((!branchSelect && Input.GetKeyUp(action) && canTalk) || branchSelect)
        {
            // 해당 NPC에 맞는 대화 스크립트 실행
            Invoke(name, 0);
        }
    }

    // MaskDude 대화 스크립트
    void MaskDude()
    {
        canTalk = false;
        // 대화의 진행 정도에 따른 조건분기
        switch (textNum)
        {
            case 0:
                // 첫 대화 시 대화창 활성화
                chatWindow.SetActive(true);
                // 대화 스크립트
                questText.GetComponent<TextMeshProUGUI>().text = "Hi, I'm Mask Dude. I have a small problem. Can you help me?";
                break;
            case 1:
                questAccept = 0;
                // 선택지 활성화
                chatWindow.GetComponent<ChatWindow>().BranchOn(2);
                break;
            case 2:
                // 선택하지 않을 시 대기(창이 넘어가지 않음)
                if (questAccept == 0) textNum--;
                // 선택했을 경우 창이 넘어감
                else branchSelect = false;
                break;
            case 3:
                // 선택지 수락 시
                if (questAccept == 1)
                {
                    // 스크립트 실행, 퀘스트 수령
                    questText.GetComponent<TextMeshProUGUI>().text = "Thank you. Too many ladybugs are bothering me. Kill 30 ladybugs please.";
                    PlayerPrefs.SetInt("CurrentQuest", 1);
                }
                // 거절 시 대화 마무리
                else questText.GetComponent<TextMeshProUGUI>().text = "Bye.";
                break;
            case 4:
                // 대화 종료되어 대화창 비활성화
                chatWindow.SetActive(false);
                break;
            case 5:
                questText.GetComponent<TextMeshProUGUI>().text = "......"; ;
                chatWindow.SetActive(true);
                // 대화 종료 후 추가적인 대화 시도 시 같은 스크립트 무한 출력
                textNum -= 2;
                break;
        }
        // 대화가 끝날 때마다 다음 대화로 넘어가기 위한 변수값 증가
        textNum++;
        canTalk = true;
    }

    // MaskDude 대화 스크립트
    void MaskDude2()
    {
        canTalk = false;
        switch (textNum)
        {
            case 0:
                chatWindow.SetActive(true);
                questText.GetComponent<TextMeshProUGUI>().text = "Do you want to go beyond?";
                break;
            case 1:
                questText.GetComponent<TextMeshProUGUI>().text = "That area is very slippery and you can encounter powerful monsters.";
                break;
            case 2:
                questText.GetComponent<TextMeshProUGUI>().text = "Be careful.";
                break;
            case 3:
                chatWindow.SetActive(false);
                textNum = -1;
                break;
        }
        textNum++;
        canTalk = true;
    }

    // Developer 대화 스크립트
    void Developer()
    {
        canTalk = false;
        switch (textNum)
        {
            case 0:
                chatWindow.SetActive(true);
                questText.GetComponent<TextMeshProUGUI>().text = "Welcome to RPG game!";
                break;
            case 1:
                questText.GetComponent<TextMeshProUGUI>().text = "I am developer, and I want you to enjoy this game.";
                break;
            case 2:
                questText.GetComponent<TextMeshProUGUI>().text = "Thank you.";
                break;
            case 3:
                chatWindow.SetActive(false);
                textNum = -1;
                break;
        }
        textNum++;
        canTalk = true;
    }


    // NinjaFrog 대화 스크립트
    void NinjaFrog()
    {
        // 타임어택 스테이지를 막고 있는 벽 오브젝트
        Transform startline = GameObject.Find("walls").transform.Find("Timeattack").transform.Find("startline");
        Transform tilemap = GameObject.Find("Grid").transform.Find("Tilemap");
        canTalk = false;
        switch (textNum)
        {
            case 0:
                chatWindow.SetActive(true);
                questText.GetComponent<TextMeshProUGUI>().text = "Hey, there is a maze. You can get a reward if you can complete this maze.";
                break;
            case 1:
                questText.GetComponent<TextMeshProUGUI>().text = "Will you try?";
                break;
            case 2:
                questAccept = 0;
                chatWindow.GetComponent<ChatWindow>().BranchOn(2);
                branchSelect = true;
                break;
            case 3:
                if (questAccept == 0) textNum--;
                else branchSelect = false;
                break;
            case 4:
                if (questAccept == 1)
                {
                    // 타임어택 활성화, 60초
                    timer.SetActive(true);
                    timer.GetComponent<TimeAttack>().timeAttack = 60;
                }
                else
                {
                    textNum = -1; // 거절하면 초기화
                }
                chatWindow.SetActive(false);
                break;
            case 5:
                if (timer.activeSelf)
                {
                    // 타임어택 잔행 중 대화 시 중단 여부 물어봄
                    chatWindow.SetActive(true);
                    questText.GetComponent<TextMeshProUGUI>().text = "Will you stop?";
                    questAccept = 0;
                    chatWindow.GetComponent<ChatWindow>().BranchOn(2);
                    branchSelect = true;
                }
                else textNum = -1; // 타임어택 시간초과 시 초기화
                break;
            case 6:
                if (questAccept == 0) textNum--;
                else branchSelect = false;
                break;
            case 7:
                if (questAccept == 1) // 도전 중단 시
                {
                    // 중단 함수 실행
                    timer.GetComponent<TimeAttack>().Stop();
                    // 대화 초기화
                    textNum = -1;
                }
                else textNum = textNum - 3;
                chatWindow.SetActive(false);
                break;
        }
        textNum++;
        canTalk = true;
    }
}
