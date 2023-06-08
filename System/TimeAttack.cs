using TMPro;
using UnityEngine;

public class TimeAttack : MonoBehaviour
{
    float time;
    public float timeAttack = 10; // 제한시간
    GameObject darkness; // 시야 제한 오브젝트
    public GameObject player;
    public Transform tilemap; //스타트라인 타일(직접적으로 이동을 제한하는 오브젝트는 startline이고
                              // 해당 트랜스폼은 단순히 스타트라인의 외형을 담당함
    public Transform startline; // 스타트라인(타임어택을 활성화하지 않고 맵에 입장하지 못하게 하기 위해 막아둠)

    void Start()
    {
        // 플레이어의 시야 제한
        darkness = GameObject.Find("emptyForDarkness").transform.GetChild(0).gameObject;
    }

    void Update()
    {
        //시간 측정
        time += Time.deltaTime;
        // 일반 게임에서 카운트다운을 하듯이 3초의 여유시간을 두고 타임어택을 시작함.
        if (time < 1.5f) GetComponent<TextMeshProUGUI>().text = "Ready...";
        else if (time < 3) GetComponent<TextMeshProUGUI>().text = "Start!!!";
        // 타임어택 시작 후 끝나기 직전까지
        else if (time < timeAttack + 3)
        {
            // 스타트라인 타일 변경
            tilemap.GetComponent<TileChange>().ChangeTiles();
            // 이동을 막고 있던 스타트라인 비활성화
            startline.gameObject.SetActive(false);
            // 플레이어 이동 가능
            player.GetComponent<Player>().enabled = true;
            // 시야 방해 기능 활성화
            darkness.SetActive(true);
            // 화면 상단에 남은 시간 표시
            GetComponent<TextMeshProUGUI>().text = (timeAttack + 3 - Mathf.Ceil(time)).ToString();
        }
        //타임어택 종료
        else 
        {
            Stop();
        }
            
    }

    // 해당 오브젝트가 활성화되었을 때 자동으로 실행
    void OnEnable()
    {
        // 스크립트 수행에 필요한 오브젝트 탐색 및 기타 작업 수행
        player = GameObject.FindGameObjectWithTag("Player");
        tilemap = GameObject.Find("Grid").transform.Find("Tilemap");
        startline = GameObject.Find("walls").transform.Find("Timeattack").transform.Find("startline");
        time = 0;
        player.GetComponent<Player>().enabled = false;
    }

    // 타임어택 종료
    public void Stop()
    {
        // 타임어택 진행 도중 변경된 타일 복구
        tilemap.GetComponent<TileChange>().RecoverTiles();
        // 스타트라인 재활성화
        startline.gameObject.SetActive(true);
        // 타임어택 UI와 시야 제한 오브젝트 비활성화
        darkness.SetActive(false);
        gameObject.SetActive(false);
        //플레이어 위치 초기화
        player.transform.localPosition = new Vector3(-17, -2.5f, 0);
    }
}
