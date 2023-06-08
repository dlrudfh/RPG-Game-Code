using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerLevel : MonoBehaviour
{
    public int level;
    public float damage;
    public float exp;
    public float maxHp;
    public float curHp;
    public int point;
    public int gold;
    public int chargeshot;
    public int dash;

    //����ġ ȹ�� �Լ�
    public void GetExp(float enemyExp)
    {
        level = PlayerPrefs.GetInt("LV");
        exp = PlayerPrefs.GetFloat("XP");

        //����ġ ȹ��
        exp += enemyExp;
        //�������� ������ ��
        if(exp>= level)
        {
            //����ġ�� 100% �̻��� ��(����ġ�� �� ���� ���� ��� ���� ������
            //�ö�� �� �� �����Ƿ� while���
            while (exp >= PlayerPrefs.GetInt("LV"))
            {
                //�������� �ʿ��� �䱸 ����ġ ��ŭ ����ġ�� ���ҽ�Ŵ
                exp -= PlayerPrefs.GetInt("LV");
                //����, ����Ʈ ����
                PlayerPrefs.SetInt("LV", PlayerPrefs.GetInt("LV")+1);
                PlayerPrefs.SetInt("PTS", PlayerPrefs.GetInt("PTS")+1);
                // ü���� �ִ�� ȸ��
                PlayerPrefs.SetFloat("CHP", PlayerPrefs.GetFloat("HP"));
            }
            // ������ ���� �ȳ� �޽���(���� ������ ������ ���� ������ ��������
            // ��ų �� ���� ���� ������ ��ų, ���ٸ� ������ �޽��� ���)
            if (level < 10 && PlayerPrefs.GetInt("LV") >= 10)
                PlayerPrefs.SetString("Notice", "Now you can use Heal!!");
            else if (level < 5 && PlayerPrefs.GetInt("LV") >= 5)
                PlayerPrefs.SetString("Notice", "Now you can use Dash!!");
            else if (level < 3 && PlayerPrefs.GetInt("LV") >= 3)
                PlayerPrefs.SetString("Notice", "Now you can use Charge Shot!!");
            else
                PlayerPrefs.SetString("Notice", "LEVEL UP!!");
        }
        
        //���ҵ� ����ġ�� PlayerPrefs�� �ݿ�
        PlayerPrefs.SetFloat("XP", exp);
        point = PlayerPrefs.GetInt("PTS");
    }

    //������ ���� �Լ�
    public void DamageUp()
    {
        point = PlayerPrefs.GetInt("PTS");
        damage = PlayerPrefs.GetFloat("DMG");
        // ����Ʈ�� ���� ���
        if (point > 0)
        {
            //����Ʈ�� �Ҹ��� ��������
            PlayerPrefs.SetFloat("DMG", ++damage);
            PlayerPrefs.SetInt("PTS", --point);
        }
        
    }

    //����Ʈ ȹ�� �Լ�
    public void PointUp()
    {
        point = PlayerPrefs.GetInt("PTS");
        gold = PlayerPrefs.GetInt("GOLD");
        // ����� ��尡 ���� ���
        if (gold >= 100)
        {
            // ��带 �Ҹ��� ����Ʈ ȹ��
            PlayerPrefs.SetInt("PTS", ++point);
            PlayerPrefs.SetInt("GOLD", gold-100);
        }

    }

    // ü�� ���� �Լ�
    public void HpUp()
    {
        point = PlayerPrefs.GetInt("PTS");
        maxHp = PlayerPrefs.GetFloat("HP");
        curHp = PlayerPrefs.GetFloat("CHP");
        // ����Ʈ�� ���� ���
        if (point > 0)
        {
            // ����Ʈ�� �Ҹ��� �ִ� ü���� ������Ŵ(���� ü�µ� ���� ��ġ��ŭ ����)
            PlayerPrefs.SetFloat("HP", ++maxHp);
            PlayerPrefs.SetFloat("CHP", ++curHp);
            PlayerPrefs.SetInt("PTS", --point);
        }

    }

    //��ų ������ �Լ�(��ư���κ��� string���·� ��ų���� ���޹޾� �ش� ��ų ������ �ø�)
    public void SkillLVUP(string skillLV)
    {
        // ����Ʈ�� �ְ�, ��ų�� ������ �ƴ� ���
        if (PlayerPrefs.GetInt("PTS") > 0 && PlayerPrefs.GetInt(skillLV) < 10)
        {
            //����Ʈ�� �Ҹ��� ��ų ������ �ø�
            PlayerPrefs.SetInt(skillLV, PlayerPrefs.GetInt(skillLV) + 1);
            PlayerPrefs.SetInt("PTS", PlayerPrefs.GetInt("PTS") - 1);
        }
    }

    // �ǰ� ������ �Լ�
    public void TakeDamage(float enemyDmg, Animator a)
    {
        // �÷��̾��� ���� ü���� ���� ���ݷ¸�ŭ ����
        PlayerPrefs.SetFloat("CHP", PlayerPrefs.GetFloat("CHP")-enemyDmg);
        //ü���� 0 ������ ���
        if (PlayerPrefs.GetFloat("CHP") <= 0)
        {
            //��� ����
            a.SetBool("isDying", true);
            a.SetBool("isRunning", false);
            a.SetBool("isJumping", false);
            a.SetBool("isDashing", false);
            a.SetBool("isFalling", false);
            //1�� �� ���ӿ��� ������ ��ȯ
            Invoke("Gameover", 1.0f);
        }
    }

    private void Gameover()
    {
        // ������ óġ���� �ʰ� ���� ���� ������ �̵��ϱ� ������
        // �̵� ���� ������ �����ؼ� ���̵��� �Ұ������� �ʵ��� ����
        PlayerPrefs.SetInt("SwitchLock", 0);
        // UI�� ���� ��Ȱ��ȭ(���� ���� â������ UI�� ������ ����
        GameObject.Find("System").GetComponent<UIinfo>().DeActivate();
        SceneManager.LoadScene("GameOver");
    }

}
