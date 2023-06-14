using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public StageData stage; // 스테이지 정보(플레이어 이동 가능 반경)
    [SerializeField] GameObject bulletPrefab; // 총알
    [SerializeField] GameObject chargedBullet; // 차지공격
    //플레이어 조작키
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode attack;
    [SerializeField] KeyCode jump;
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode dash;
    [SerializeField] KeyCode heal;
    [SerializeField] KeyCode action;
    float slip;

    [SerializeField] bool canDown; // 하향점프의 가능 여부
    [SerializeField] Animator anime;
    AudioSource audioSource;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip chargeCompleteSound;
    [SerializeField] AudioClip chargeShootSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip healSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip hitSound;
    int dir; // 방향
    float startTime; // 차지 시간 측정을 위한 변수
    bool chargeComplete = true; // 차지 완료 여부
    bool isAlreadyHurt = false; // 피격중인지 여부(연속 피격 방지)
    bool ceiling; // 천장에 접촉중인지 여부

    void Start()
    {
        speed = PlayerPrefs.GetFloat("SPEED");
        // 맵을 이동했을 때 플레이어 위치에 따른 방향 설정
        if (transform.position.x >= 0) dir = -1;
        else dir = 1;
        audioSource = GetComponent<AudioSource>();
        // 키세팅 함수 실행
        Key();
    }

    // 원하는 효과음을 재생하는 함수
    void playSound(string cond)
    {
        // 입력받은 문자열에 해당하는 효과음을 clip에 할당
        switch (cond)
        {
            case"Shoot":
                audioSource.clip = shootSound;
                break;
            case "ChargeComplete":
                audioSource.clip = chargeCompleteSound;
                break;
            case "ChargeShoot":
                audioSource.clip = chargeShootSound;
                break;
            case "Attack":
                audioSource.clip = attackSound;
                break;
            case "Hit":
                audioSource.clip = hitSound;
                break;
        }
        // 효과음 재생
        audioSource.Play();
    }

    void Update()
    {
        // 플레이어 효과음 음량 조절
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFFECT");
        // 플레이어가 사망했을 경우 동작하지 않음
        if (anime.GetBool("isDying")) return;
        // 플레이어가 하강중일 때
        if (GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            // 점프 횟수를 최소 1회로 설정
            GetComponent<Movement2D>().jumpCount = GetComponent<Movement2D>().jumpCount == 2 ? 2 : 1;
            // 하강 상태로 변경(애니메이션 재생)
            anime.SetBool("isJumping", false);
            anime.SetBool("isFalling", true);
        }
        // 플레이어가 y축이동이 없을 때(바닥에 닿아 있을 때)
        else if (GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            // 플레이어가 바닥에 닿으면 점프 횟수를 초기화 해주어야 하는데
            // 대시 중이거나 천장에 부딪혔을 때도 마찬가지로 y축 이동속도가 0이 되기 때문에
            // 해당 조건을 모두 검사함
            if (!ceiling && !GetComponent<Movement2D>().doNotDash)
            {
                GetComponent<Movement2D>().jumpCount = 0;
            }
            // 애니메이션 중지(idle상태로 돌아감)
            anime.SetBool("isFalling", false);
            anime.SetBool("isJumping", false);
        }
        // 상승중일 경우(점프)
        else anime.SetBool("isFalling", false);
        // 대시 스킬을 배운 상태일 때
        if (PlayerPrefs.GetInt("DASHLV") > 0)
        {
            //대시 키를 입력
            if (Input.GetKeyDown(dash))
            {
                //효과음을 대시로 변경
                audioSource.clip = dashSound;
                // 대시 작동
                GetComponent<Movement2D>().Dash(dir, anime, audioSource);
                anime.SetBool("isFalling", true);
            }
        }
        // 힐 스킬을 배운 상태일 때
        if (PlayerPrefs.GetInt("HEALLV") > 0)
        {
            //힐 키를 입력
            if (Input.GetKeyDown(heal))
            {
                // 효과음을 힐로 변경
                audioSource.clip = healSound;
                //힐 작동
                GetComponent<Skills>().Heal(audioSource);
            }
        }
        // 점프 키를 입력
        if (Input.GetKeyDown(jump))
        {
            //효과음을 점프로 변경
            audioSource.clip = jumpSound;
            // 점프 키를 눌렀을 때 아래방향 키가 같이 입력되어 있는 상태였을 경우(하향점프)
            if (Input.GetKey(down) && canDown)
            {
                //하향점프 작동
                GetComponent<Movement2D>().Down(anime, audioSource);
            }
            // 점프 작동
            else GetComponent<Movement2D>().Jump(anime, audioSource); 
        }
        PlayerMove();
        // 공격 키를 입력
        if (Input.GetKeyDown(attack))
        {
            playSound("Shoot");
            // 입력 시작한 시간 기록
            startTime = Time.time;
            // 입력을 새로 시작했으므로 차지상태 해제
            chargeComplete = false;
            // 총알 오브젝트 생성 후 캐릭터가 쳐다보는 방향으로 날아감
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x + 0.5f*dir, transform.position.y, 0), Quaternion.identity);
            bullet.GetComponent<Movement2D>().Setup(10.0f, new Vector3(dir, 0, 0));
            Destroy(bullet, 2.0f);
        }
        // 차지 공격 스킬을 배운 상태일 때
        if (PlayerPrefs.GetInt("CHARGESHOTLV") > 0)
        {
            // 차지를 완료했다면(0.7초 경과)
            if(chargeComplete == false && Time.time - startTime >= 0.7f)
            {
                // 차지 완료 효과음 재생
                playSound("ChargeComplete");
                chargeComplete = true;
            }
            // 공격 키를 뗐을 때
            if (Input.GetKeyUp(attack))
            {
                // 차지가 완료된 상태라면
                if (chargeComplete == true)
                {
                    // 차지 공격 수행
                    playSound("ChargeShoot");
                    GameObject charge = Instantiate(chargedBullet, new Vector3(transform.position.x + 0.5f * dir, transform.position.y, 0), Quaternion.identity);
                    charge.GetComponent<Movement2D>().Setup(20.0f, new Vector3(dir, 0, 0));
                    Destroy(charge, 1.0f);
                }
                else chargeComplete = true;
            }
        }
    }

    void PlayerMove()
    {
        // 좌우 방향키
        bool l = Input.GetKey(left);
        bool r = Input.GetKey(right);
        // 좌우 방향키 중 어느 쪽이 입력되더라도 달리기 상태로 변경됨
        anime.SetBool("isRunning", l || r);
        // 이동 방향에 따른 방향 변수 값 변경
        if (l)
        {
            dir = -1;
            if (slip >= -1) slip -= 1.5f * Time.deltaTime;
        }
        else if (r)
        {
            dir = 1;
            if (slip <= 1) slip += 1.5f * Time.deltaTime;
        }
        else if (slip > 0)
        {
            if (slip >= 0) slip -= 1.5f * Time.deltaTime;
        }
        else if (slip < 0)
        {
            if (slip <= 0) slip += 1.5f * Time.deltaTime;
        }
        // 이동 방향에 따른 플레이어 방향 회전
        transform.rotation = Quaternion.Euler(0, 90 - 90 * dir, 0);
        // 플레이어 이동
        if(PlayerPrefs.GetInt("SLIP")==1) transform.position += new Vector3(slip, 0, 0) * speed * Time.deltaTime;
        else transform.position += new Vector3(anime.GetBool("isRunning")?dir:0, 0, 0) * speed * Time.deltaTime;
    }

    //플레이어에게는 플레이어 크기의 circle collider와 플레이어 머리에 작은 box collider가 존재함.
    // circle collider는 통과할 수 없는 콜라이더로, 벽이나 적과 부딪히고 발판에 착지하도록 해주는 역할을 함.
    // box collider는 istrigger타입의 콜라이더로, 벽 오브젝트의 통과 여부를 관리해주는 역할을 함.
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 사망했다면 작동하지 않음
        if (anime.GetBool("isDying")) return;
        // 머리와 충돌한 오브젝트가 벽인지 확인
        if (collision.TryGetComponent(out Wall w))
        {
            // 충돌한 오브젝트가 플레이어보다 위에 있다면
            if (collision.transform.position.y > transform.position.y)
            {
                //천장 변수 = true(y축 속도가 0이라도 점프 횟수를 초기화하지 않음)
                ceiling = true;
                // 해당 오브젝트가 통과가 가능한 벽이라면 오브젝트의 collider를 비활성화하여 통과
                if (w.canPass) collision.GetComponent<BoxCollider2D>().enabled = false;
            }  
        }
    }

    //triggerstay이기 때문에 적 오브젝트와 지속적으로 닿아 있을 경우 일정 시간마다 계속 피해를 입을 수 있다.
    void OnTriggerStay2D(Collider2D collision)
    {
        // 오브젝트의 타입을 확인하기 위해 태그 값을 저장
        string t = collision.gameObject.tag;
        // 피격 무적 상태가 아닐 때
        if (t.Length > 5 && !isAlreadyHurt)
        {
            // 충돌한 오브젝트가 패턴, 적, 보스, 장애물일 경우
            if (t.Substring(0, 5) == "patte" || t.Substring(0, 5) == "enemy" || t.Substring(0, 5) == "bosss"
                || t.Substring(0, 5) == "obsta")
            {
                // 피격당했으므로 잠시동안 피격 무적 처리
                isAlreadyHurt = true;
                // 피격 애니메이션 재생
                StartCoroutine("HitColorAnimation");
                // 패턴의 경우 피격 시 패턴이 삭제됨
                if (t.Substring(0, 5) == "patte") Destroy(collision.gameObject);
                playSound("Hit");
                // 해당 오브젝트의 태그 값에 적혀 있는 숫자만큼 피해를 입음
                GetComponent<PlayerLevel>().TakeDamage(float.Parse(t.Substring(5)), anime);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 충돌이 종료되었으므로 천장에 닿아 있을 리가 없기 때문에 ceiling = false
        ceiling = false;
        // 충돌이 종료된 오브젝트가 벽인지 확인
        if (collision.TryGetComponent(out Wall w))
        {
            // 통과 가능한 벽이었을 경우 하향점프를 통해 플레이어가 벽과 겹치지 않게 되면 
            // 벽의 콜라이더를 재활성화함(활성화하지 않으면 벽이 한번 통과된 이후에 다시는 벽에 착지할 수 없기 때문)
            if (w.canPass) collision.GetComponent<BoxCollider2D>().enabled = true;
        }
           
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // 플레이어가 사망했다면 작동하지 않음
        if (anime.GetBool("isDying")) return;
        // 충돌한 오브젝트가 벽인지 확인한 뒤 맞다면 canPass 변수 값을 canDown에 대입
        // (벽이 통과 가능한(canPass) 상태일 경우 하향 점프가 가능하고(canDown), 반대의 경우 하향 점프 불가능)
        if (collision.gameObject.TryGetComponent(out Wall w) && GetComponent<Rigidbody2D>().velocity.y == 0)
            canDown = w.canPass;
        else canDown = false;
    }

    //피격 애니메이션
    IEnumerator HitColorAnimation()
    {
        // 피격 시 0.5초간 피격 무적 판정이 주어지고 캐릭터가 흐려짐
        GetComponent<SpriteRenderer>().color = Color.gray;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
        isAlreadyHurt = false;
    }

    public void Key()
    {      
        // PlayerPrefs에 저장된 문자열 형태의 플레이어 조작키 값을 가져온 뒤
        // KeyCode형태로 파싱하여 알맞은 변수에 값을 대입해 줌
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LEFT"), true);
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RIGHT"), true);
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JUMP"), true);
        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UP"), true);
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DOWN"), true);
        attack = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ATTACK"), true);
        dash = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DASH"), true);
        heal = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("HEAL"), true);
        action = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ACTION"), true);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stage.LimitMin.x, stage.LimitMax.x),
                                         Mathf.Clamp(transform.position.y, stage.LimitMin.y-3, stage.LimitMax.y));
    }
}
