using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// 보스 오브젝트
public class Boss1 : MonoBehaviour
{
    [SerializeField] GameObject player; 
    [SerializeField] GameObject pattern;
    public float maxHp;
    public float curHp;
    [SerializeField] GameObject coin;
    [SerializeField] GameObject money;
    int direction = -1; // 위아래 반복운동을 위한 방향변수
    [SerializeField] float exp = 10; // 보스 지급경험치
    [SerializeField] Animator anime;
    [SerializeField] private Slider hpSlider;
    [SerializeField] GameObject block;
    float speed = 2;
    bool berserk;

    public float MaxHp => maxHp;
    public float CurHp => curHp;

    private void Start()
    {
        curHp = maxHp;
        // 맵이동 금지
        PlayerPrefs.SetInt("SwitchLock", 1);
        // 패턴 코루틴 시작
        StartCoroutine(Pattern(5));
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // 광폭 상태일 때 스피드 = 3
        if (berserk) speed = 3;
        // 사망한 상태라면 실행 중지
        if (anime.GetBool("Die")) return;
        // 체력 슬라이더 값을 현재 체력 비율로 반영
        hpSlider.value = curHp/maxHp;
        // 보스 이동 및 방향에 따른 회전
        transform.position += new Vector3(direction, 0, 0) * speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 90 + 90 * direction, 0);
        // 체력이 30% 미만일 때 광폭화
        if (curHp / maxHp < 0.3f) berserk = true;
        if (player.transform.position.x > transform.position.x + 2) direction = 1;
        else if (player.transform.position.x < transform.position.x - 2) direction = -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (anime.GetBool("Die")) return;
        // 총알과 충돌 시
        if (collision.CompareTag("bullet") || collision.CompareTag("chargedBullet"))
        {
            // 데미지를 입음
            if (collision.CompareTag("bullet")) Destroy(collision.gameObject);
            if (collision.CompareTag("bullet")) curHp -= PlayerPrefs.GetFloat("DMG");
            else curHp -= PlayerPrefs.GetFloat("DMG") * (1 + PlayerPrefs.GetInt("CHARGESHOT") * 0.4f);
            // 충돌 애니메이션
            StopCoroutine("HitColorAnimation");
            StartCoroutine("HitColorAnimation");
            if (curHp <= 0)
            {
                hpSlider.value = 0;
                player.GetComponent<PlayerLevel>().GetExp(exp);
                anime.SetBool("Die", true);
                Invoke("BossDie", 1.0f);
            }
        }
    }

    // 보스 사망 후 막힌 길이 열리고 아이템이 드랍됨
    private void BossDie()
    {
        Destroy(block);
        GameObject gold;
        // 랜덤 값을 생성해 일정 확률로 돈다발과 코인을 드랍함
        int rand = Random.Range(1, 6);
        if (rand <= 3) gold = Instantiate(money, transform.position, Quaternion.identity);
        else gold = Instantiate(coin, transform.position, Quaternion.identity);
        gold.GetComponent<Gold>().Drop();
        // 보스 처치 시 맵 이동 기능 활성화
        PlayerPrefs.SetInt("SwitchLock", 0);
        //드랍 완료 후 보스와 체력 바 제거
        Destroy(hpSlider.gameObject);
        Destroy(gameObject);
    }

    // 피격 애니메이션
    private IEnumerator HitColorAnimation()
    {
        // 보스가 잠깐 흐려지며 피격되었음을 표시함
        GetComponent<SpriteRenderer>().color = Color.gray;
        yield return new WaitForSeconds(0.05f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // 보스 패턴
    private IEnumerator Pattern(float delay)
    {
        // 보스가 죽었으면 패턴 실행 중지
        if (!anime.GetBool("Die"))
        {
            yield return new WaitForSeconds(delay - 1);
            GetComponent<Animator>().Rebind();
            GetComponent<Animator>().Play("Attack");
            anime.SetBool("Attack", true);
            // 공격 애니메이션이 완전히 재생된 후 패턴 설치
            yield return new WaitForSeconds(1);
            // 광폭화 상태가 아닐 때 패턴
            if (!berserk)
            {
                // 3번 반복
                for (int a = 0; a < 3; a++)
                {
                    // 무작위 위치에 감속 장판 생성
                    Instantiate(pattern, new Vector3(Random.Range(0.0f, 9.0f) * 2 - 8, -4.05f, 0), Quaternion.identity);
                }
            }
            else
            {
                // 5번 반복
                for (int a = 0; a < 5; a++)
                {
                    // 무작위 위치에 감속 장판 생성
                    Instantiate(pattern, new Vector3(Random.Range(0f, 9f) * 2 - 8, -4.05f, 0), Quaternion.identity);
                }
            }
            // 일정 시간마다 패턴 반복(광폭화 상태일 경우 패턴 반복 주기가 짧아짐)
            StartCoroutine(Pattern(berserk ? 3 : 5));
        }
    }
}
