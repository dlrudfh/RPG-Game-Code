using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// 보스 오브젝트
public class Boss2 : MonoBehaviour
{
    [SerializeField] GameObject player; 
    [SerializeField] GameObject pattern;
    public float maxHp;
    public float curHp;
    [SerializeField] GameObject coin;
    [SerializeField] GameObject money;
    int direction = 1; // 위아래 반복운동을 위한 방향변수
    [SerializeField] float exp = 10; // 보스 지급경험치
    [SerializeField] Animator anime;
    [SerializeField] private Slider hpSlider;
    float speed = 3;
    bool berserk;
    bool pattern2;
    float x; // 돌진 패턴의 x방향 속도

    public float MaxHp => maxHp;
    public float CurHp => curHp;

    private void Start()
    {
        curHp = maxHp;
        // 맵이동 금지
        PlayerPrefs.SetInt("SwitchLock", 1);
        player = GameObject.FindGameObjectWithTag("Player");
        // 패턴 코루틴 시작
        StartCoroutine(Pattern(0.8f));
    }

    private void Update()
    {
        // 광폭 상태일 때 스피드 = 5
        if (berserk) speed = 5;
        // 사망한 상태라면 실행 중지
        if (anime.GetBool("Die")) return;
        // 체력 슬라이더 값을 현재 체력 비율로 반영
        hpSlider.value = curHp/maxHp;
        // 위 아래로 일정 시간마다 방향을 바꾸며 반복 운동
        transform.position += new Vector3(x, direction, 0) * speed * Time.deltaTime;
        if (!berserk)
        {
            if (transform.position.y < -3.3f) direction = 1;
            else if (transform.position.y > 5) direction = -1;
            // 체력이 30% 미만일 때 광폭화
            if (curHp / maxHp < 0.3f)
            {
                berserk = true;
                StartCoroutine(Pattern2(3));
            }
        }
        // 광폭화 상태일 때 움직임
        else if (!pattern2)
        {
            if (player.transform.position.y + 0.8f > transform.position.y + 1) direction = 1;
            else if (player.transform.position.y + 0.8f < transform.position.y - 1) direction = -1;
        }
        else direction = 0;
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
                Invoke("DropItem", 1.0f);
            }
        }
    }

    // 보스 처치 시 아이템을 드랍함
    private void DropItem()
    {
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
        yield return new WaitForSeconds(delay);
        if (!pattern2)
        {
            // 패턴 생성
            GameObject pat = Instantiate(pattern, new Vector3(7, transform.position.y - 0.8f, 0), Quaternion.identity);
            // 패턴이 무작위 속도로 왼쪽으로 날아감
            pat.GetComponent<Movement2D>().Setup(Random.Range(3f, 15f), Vector3.left);
        }
        // 일정 시간마다 패턴 반복(광폭화 상태일 경우 패턴 반복 주기가 짧아짐)
        StartCoroutine(Pattern(berserk ? 0.4f : 0.8f));
    }

    // 보스 패턴 2(돌진)
    private IEnumerator Pattern2(float delay)
    {
        if (berserk)
        {
            pattern2 = true;
            yield return new WaitForSeconds(0.5f);
            x = -20;
            yield return new WaitForSeconds(0.12f);
            x = 4;
            yield return new WaitForSeconds(0.6f);
            transform.position = new Vector3(7, transform.position.y, transform.position.z);
            x = 0;
            pattern2 = false;
            yield return new WaitForSeconds(delay-1.22f);
            // 일정 시간마다 패턴 반복
            StartCoroutine(Pattern2(3));
        }
    }
}
