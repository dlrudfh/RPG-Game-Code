using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// ���� ������Ʈ
public class Boss1 : MonoBehaviour
{
    [SerializeField] GameObject player; 
    [SerializeField] GameObject pattern;
    public float maxHp;
    public float curHp;
    [SerializeField] GameObject coin;
    [SerializeField] GameObject money;
    int direction = -1; // ���Ʒ� �ݺ���� ���� ���⺯��
    [SerializeField] float exp = 10; // ���� ���ް���ġ
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
        // ���̵� ����
        PlayerPrefs.SetInt("SwitchLock", 1);
        // ���� �ڷ�ƾ ����
        StartCoroutine(Pattern(5));
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // ���� ������ �� ���ǵ� = 3
        if (berserk) speed = 3;
        // ����� ���¶�� ���� ����
        if (anime.GetBool("Die")) return;
        // ü�� �����̴� ���� ���� ü�� ������ �ݿ�
        hpSlider.value = curHp/maxHp;
        // ���� �̵� �� ���⿡ ���� ȸ��
        transform.position += new Vector3(direction, 0, 0) * speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 90 + 90 * direction, 0);
        // ü���� 30% �̸��� �� ����ȭ
        if (curHp / maxHp < 0.3f) berserk = true;
        if (player.transform.position.x > transform.position.x + 2) direction = 1;
        else if (player.transform.position.x < transform.position.x - 2) direction = -1;
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
                Invoke("BossDie", 1.0f);
            }
        }
    }

    // ���� ��� �� ���� ���� ������ �������� �����
    private void BossDie()
    {
        Destroy(block);
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
        // ������ �׾����� ���� ���� ����
        if (!anime.GetBool("Die"))
        {
            yield return new WaitForSeconds(delay - 1);
            GetComponent<Animator>().Rebind();
            GetComponent<Animator>().Play("Attack");
            anime.SetBool("Attack", true);
            // ���� �ִϸ��̼��� ������ ����� �� ���� ��ġ
            yield return new WaitForSeconds(1);
            // ����ȭ ���°� �ƴ� �� ����
            if (!berserk)
            {
                // 3�� �ݺ�
                for (int a = 0; a < 3; a++)
                {
                    // ������ ��ġ�� ���� ���� ����
                    Instantiate(pattern, new Vector3(Random.Range(0.0f, 9.0f) * 2 - 8, -4.05f, 0), Quaternion.identity);
                }
            }
            else
            {
                // 5�� �ݺ�
                for (int a = 0; a < 5; a++)
                {
                    // ������ ��ġ�� ���� ���� ����
                    Instantiate(pattern, new Vector3(Random.Range(0f, 9f) * 2 - 8, -4.05f, 0), Quaternion.identity);
                }
            }
            // ���� �ð����� ���� �ݺ�(����ȭ ������ ��� ���� �ݺ� �ֱⰡ ª����)
            StartCoroutine(Pattern(berserk ? 3 : 5));
        }
    }
}
