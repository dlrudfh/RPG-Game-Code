using System.Collections;
using UnityEngine;

// 각종 오브젝트를 생성하는 스크립트
public class Spawner : MonoBehaviour
{
    [SerializeField]
    private StageData stage;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject notice;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject enemyHp;
    [SerializeField]
    private float spawnTime; // 적 생성 주기
    private int spawnCount; // 맵에 생성된 적 수
    Transform walls; // 벽 오브젝트를 모아 놓은 부모 오브젝트
    [SerializeField] bool spawn; // 해당 맵에서 적 소환을 할 것인지 여부(inspector에서 설정 가능)
    [SerializeField] int spawnLimit = 20; // 적 최대 생성 제한

    private void Awake()
    {
        // 적 소환이 가능한 맵일 경우
        if (spawn)
        {
            // 벽 오브젝트를 탐색
            walls = GameObject.Find("walls").transform;
            // 스폰 코루틴 시작
            StartCoroutine("SpawnStart");
        }
        PlayerPrefs.SetInt("spawnCount", 0);
        // 플레이어를 주어진 좌표에 생성
        GameObject p = Instantiate(player, new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"), 0), Quaternion.identity);
        // 플레이어의 이동반경을 설정하기 위해 stageata 삽입
        p.GetComponent<Player>().stage = stage;
    }

    // 적 소환 함수
    private IEnumerator SpawnStart()
    {
        yield return new WaitForSeconds(spawnTime);
        spawnCount = PlayerPrefs.GetInt("spawnCount");
        // 맵에 존재하는 벽들 중 하나를 선택
        Transform floor = walls.GetChild(Random.Range(0, walls.childCount));
        while (true)
        {
            // 무한 반복하면서 적 생성이 가능한 벽이 선택될 때까지 반복
            // (특정 벽에서는 적 생성이 불가능하도록 설정하기 위해 구현)
            if(floor.TryGetComponent(out Wall w))
                if (w.canSpawn) break;
            floor = walls.GetChild(Random.Range(0, walls.childCount));
        }
        // 적 생성 갯수를 초과하지 않았을 때
        if (spawnCount <= spawnLimit)
        {
            spawnCount++;
            PlayerPrefs.SetInt("spawnCount", spawnCount);
            // 선택된 발판의 랜덤한 위치에 적 생성
            float x = Random.Range(floor.position.x - floor.localScale.x/2 + 0.5f, floor.position.x + floor.localScale.x/2 - 2.5f);
            float y = floor.position.y + 1; 
            Vector3 position = new Vector3(x, y, 1);
            GameObject enemyClone = Instantiate(enemyPrefab, position, Quaternion.identity);
            // 적의 hp바도 함께 생성
            SpawnHp(enemyClone);
        }
        //코루틴 반복
        StopCoroutine("SpawnStart");
        StartCoroutine("SpawnStart");
    }

    //적 체력 바 생성 함수
    private void SpawnHp(GameObject enemy)
    {
        //체력 바 생성
        GameObject sliderClone = Instantiate(enemyHp);
        // 체력 바는 UI이기 때문에 캔버스(System)의 자식 오브젝트로 상속되어야 함
        sliderClone.transform.SetParent(GameObject.Find("System").transform);
        sliderClone.transform.localScale = Vector3.one;
        // 체력 바가 적을 추적하고, 적 체력 비율에 맞게
        // 슬라이더 값을 조절하기 위해 컴포넌트 세팅
        sliderClone.GetComponent<SpawnedUISet>().Setup(enemy.transform);
        sliderClone.GetComponent<EnemyHpViewer>().Setup(enemy.GetComponent<Enemy>());
    }
}
