using UnityEngine;
using System.Collections;

// 적 오브젝트
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private  float maxHp;
    [SerializeField]
    private  float curHp;
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private GameObject money;
    [SerializeField]
    private float time; // 일정 시간마다 이동 방향 변경
    private int direction = 1; // 이동 방향
    public Animator anime; // 사망 애니메이션
    [SerializeField] private float exp = 1; // 처치시 획득하는 경험치

    public float MaxHp => maxHp;
    public float CurHp => curHp;

    private void Awake()
    {
        curHp = maxHp;
    }

    private void Update()
    {
        // 이미 사망한 상태라면 수행하지 않음
        if (anime.GetBool("Die")) return;
        // 좌우로 움직이며 일정 시간마다 방향을 전환하는 반복운동
        time += Time.deltaTime;
        // 이동 방향에 따른 적 방향 회전
        transform.rotation = Quaternion.Euler(0, 90 - 90 * -direction, 0);
        transform.position += new Vector3(direction, 0, 0) * 1.0f * Time.deltaTime;
        if (time > 2 )
        {
            direction = direction * (-1);
            time = time - 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (anime.GetBool("Die")) return;
        // 총알과 접촉 시
        if (collision.CompareTag("bullet") || collision.CompareTag("chargedBullet"))
        {   
            // 총알 혹은 차지 공격의 데미지 만큼 오브젝트의 체력 감소
            if (collision.CompareTag("bullet")) Destroy(collision.gameObject);
            if (collision.CompareTag("bullet")) curHp -= PlayerPrefs.GetFloat("DMG");
            else curHp -= PlayerPrefs.GetFloat("DMG") * (1 + PlayerPrefs.GetInt("CHARGESHOT")*0.4f);
            // 피격 애니메이션 실행
            StopCoroutine("HitColorAnimation");
            StartCoroutine("HitColorAnimation");
            // 체력이 0이하일 때
            if (curHp <= 0)
            {
                // 콜라이더 비활성화(처치되었음에도 플레이어와 충돌하여 데미지를 입히는 문제를 없애기 위함)
                GetComponent<BoxCollider2D>().enabled = false;
                // 경험치 획득
                player.GetComponent<PlayerLevel>().GetExp(exp);
                // 사망 애니메이션 실행
                anime.SetBool("Die", true);
                // 마릿수 값 감소
                int spawnCount = PlayerPrefs.GetInt("spawnCount") - 1;
                PlayerPrefs.SetInt("spawnCount", spawnCount);
                // 사망 이벤트
                Invoke("DropItem", 1.0f);
            }
        }
    }

    private void DropItem()
    {
        GameObject gold;
        // 일정 확률로 동전과 돈다발이 랜덤 드랍
        int rand = Random.Range(1, 11);
        if (rand <= 5) gold = Instantiate(money, transform.position, Quaternion.identity);
        else gold = Instantiate(coin, transform.position, Quaternion.identity);
        gold.GetComponent<Gold>().Drop();
        // 퀘스트 조건 검사 후 적용
        if (gameObject.tag == "enemy1")
        {
            QuestSummary.progress[1]++;
        }
        // 오브젝트 제거
        Destroy(gameObject);
    }

    private IEnumerator HitColorAnimation()
    { 
        // 적 오브젝트가 피격당할 시 잠깐 색이 흐려짐
        GetComponent<SpriteRenderer>().color = Color.gray;
        yield return new WaitForSeconds(0.05f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
