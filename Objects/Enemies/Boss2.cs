using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// ���� ������Ʈ
public class Boss2 : MonoBehaviour
{
    [SerializeField] GameObject player; 
    [SerializeField] GameObject pattern;
    public float maxHp;
    public float curHp;
    [SerializeField] GameObject coin;
    [SerializeField] GameObject money;
    int direction = 1; // ���Ʒ� �ݺ���� ���� ���⺯��
    [SerializeField] float exp = 10; // ���� ���ް���ġ
    [SerializeField] Animator anime;
    [SerializeField] private Slider hpSlider;
    float speed = 3;
    bool berserk;
    bool pattern2;
    float x; // ���� ������ x���� �ӵ�

    public float MaxHp => maxHp;
    public float CurHp => curHp;

    private void Start()
    {
        curHp = maxHp;
        // ���̵� ����
        PlayerPrefs.SetInt("SwitchLock", 1);
        player = GameObject.FindGameObjectWithTag("Player");
        // ���� �ڷ�ƾ ����
        StartCoroutine(Pattern(0.8f));
    }

    private void Update()
    {
        // ���� ������ �� ���ǵ� = 5
        if (berserk) speed = 5;
        // ����� ���¶�� ���� ����
        if (anime.GetBool("Die")) return;
        // ü�� �����̴� ���� ���� ü�� ������ �ݿ�
        hpSlider.value = curHp/maxHp;
        // �� �Ʒ��� ���� �ð����� ������ �ٲٸ� �ݺ� �
        transform.position += new Vector3(x, direction, 0) * speed * Time.deltaTime;
        if (!berserk)
        {
            if (transform.position.y < -3.3f) direction = 1;
            else if (transform.position.y > 5) direction = -1;
            // ü���� 30% �̸��� �� ����ȭ
            if (curHp / maxHp < 0.3f)
            {
                berserk = true;
                StartCoroutine(Pattern2(3));
            }
        }
        // ����ȭ ������ �� ������
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
        // �Ѿ˰� �浹 ��
        if (collision.CompareTag("bullet") || collision.CompareTag("chargedBullet"))
        {
            // �������� ����
            if (collision.CompareTag("bullet")) Destroy(collision.gameObject);
            if (collision.CompareTag("bullet")) curHp -= PlayerPrefs.GetFloat("DMG");
            else curHp -= PlayerPrefs.GetFloat("DMG") * (1 + PlayerPrefs.GetInt("CHARGESHOT") * 0.4f);
            // �浹 �ִϸ��̼�
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

    // ���� óġ �� �������� �����
    private void DropItem()
    {
        GameObject gold;
        // ���� ���� ������ ���� Ȯ���� ���ٹ߰� ������ �����
        int rand = Random.Range(1, 6);
        if (rand <= 3) gold = Instantiate(money, transform.position, Quaternion.identity);
        else gold = Instantiate(coin, transform.position, Quaternion.identity);
        gold.GetComponent<Gold>().Drop();
        // ���� óġ �� �� �̵� ��� Ȱ��ȭ
        PlayerPrefs.SetInt("SwitchLock", 0);
        //��� �Ϸ� �� ������ ü�� �� ����
        Destroy(hpSlider.gameObject);
        Destroy(gameObject);
    }

    // �ǰ� �ִϸ��̼�
    private IEnumerator HitColorAnimation()
    { 
        // ������ ��� ������� �ǰݵǾ����� ǥ����
        GetComponent<SpriteRenderer>().color = Color.gray;
        yield return new WaitForSeconds(0.05f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // ���� ����
    private IEnumerator Pattern(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!pattern2)
        {
            // ���� ����
            GameObject pat = Instantiate(pattern, new Vector3(7, transform.position.y - 0.8f, 0), Quaternion.identity);
            // ������ ������ �ӵ��� �������� ���ư�
            pat.GetComponent<Movement2D>().Setup(Random.Range(3f, 15f), Vector3.left);
        }
        // ���� �ð����� ���� �ݺ�(����ȭ ������ ��� ���� �ݺ� �ֱⰡ ª����)
        StartCoroutine(Pattern(berserk ? 0.4f : 0.8f));
    }

    // ���� ���� 2(����)
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
            // ���� �ð����� ���� �ݺ�
            StartCoroutine(Pattern2(3));
        }
    }
}
