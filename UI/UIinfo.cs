using UnityEngine;

public class UIinfo : MonoBehaviour
{
    [SerializeField]
    private GameObject status;
    [SerializeField]
    private GameObject skill;
    [SerializeField]
    private GameObject option;
    [SerializeField]
    private GameObject quest;
    KeyCode stKey;
    KeyCode skKey;
    KeyCode opKey;
    KeyCode quKey;
    KeyCode clKey;

    private void Awake()
    {
        // �÷��̾� ���� �ʱ�ȭ(�׽�Ʈ��)
        /*PlayerPrefs.SetInt("SwitchLock", 0);
        PlayerPrefs.SetFloat("SPEED", 5);
        PlayerPrefs.SetFloat("CHP", 3);
        PlayerPrefs.SetInt("LV", 1);
        PlayerPrefs.SetInt("PTS", 0);
        PlayerPrefs.SetFloat("DMG", 1);
        PlayerPrefs.SetInt("XP", 0);
        PlayerPrefs.SetFloat("HP", 3);
        PlayerPrefs.SetInt("GOLD", 0);
        PlayerPrefs.SetInt("CHARGESHOTLV", 0);
        PlayerPrefs.SetInt("DASHLV", 0);
        PlayerPrefs.SetInt("HEALLV", 0);
        PlayerPrefs.SetString("LEFT", "LeftArrow");
        PlayerPrefs.SetString("RIGHT", "RightArrow");
        PlayerPrefs.SetString("JUMP", "Z");
        PlayerPrefs.SetString("UP", "UpArrow");
        PlayerPrefs.SetString("DOWN", "DownArrow");
        PlayerPrefs.SetString("ATTACK", "X");
        PlayerPrefs.SetString("DASH", "C");
        PlayerPrefs.SetString("HEAL", "V");
        PlayerPrefs.SetString("ACTION", "Space");
        PlayerPrefs.SetInt("CurrentQuest", 0);
        PlayerPrefs.SetString("STATUS", "S");
        PlayerPrefs.SetString("SKILL", "K");
        PlayerPrefs.SetString("OPTION", "O");
        PlayerPrefs.SetString("QUEST", "Q");
        PlayerPrefs.SetString("CLOSE", "Escape");
        PlayerPrefs.SetFloat("MUSIC", 0.1f);
        PlayerPrefs.SetFloat("EFFECT", 0.1f);
        PlayerPrefs.SetString("Scene", "Village");
        PlayerPrefs.SetFloat("x",-0.5f);
        PlayerPrefs.SetFloat("y", 4.5f);*/
        //���� ������ �ѹ� �����ϰ� ���� �� ���κ� �ּ�ó��
        ApplyKey();
    }

    void Update()
    {
        //UIâ �¿���
        if (Input.GetKeyDown(clKey)) // esc�� ��� â �Ѳ����� ���� ����
        {
            status.SetActive(false);
            skill.SetActive(false);
            option.SetActive(false);
            quest.SetActive(false);
            if (quest.activeSelf && quest.transform.Find("Description").gameObject.activeSelf)
                quest.GetComponent<Quest>().DesOff();
        }
        else if (Input.GetKeyDown(stKey))
        {
            status.SetActive(!status.activeSelf);
        }
        else if (Input.GetKeyDown(skKey))
        {
            skill.SetActive(!skill.activeSelf);
        }
        else if (Input.GetKeyDown(opKey))
        {
            option.SetActive(!option.activeSelf);
        }
        else if (Input.GetKeyDown(quKey))
        {
            //����Ʈ ���� â ���������� ����Ʈ â�� �Բ� off�ϱ�
            if (quest.activeSelf && quest.transform.Find("Description").gameObject.activeSelf)
            {
                quest.GetComponent<Quest>().DesOff();
                quest.SetActive(false);
            }
            //�ƴ� ��� ����Ʈ â�� off
            else quest.SetActive(!quest.activeSelf);
        }
    }

    // ������ �������� ���� Ű�� �������� �� �ΰ��ӿ� �����Ű�� �Լ�
    public void ApplyKey()
    {
        stKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("STATUS"), true);
        skKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("SKILL"), true);
        opKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("OPTION"), true);
        quKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("QUEST"), true);
        clKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CLOSE"), true);
    }

    // �⺻ UI ��(��Ʈ�� �������� ���� ���� ���������� UI �Ⱥ��̰� �ϱ� ����)
    public void Activate()
    {
        transform.Find("PlayerExp").gameObject.SetActive(true);
        transform.Find("PlayerHp").gameObject.SetActive(true);
        transform.Find("PlayerInfo").gameObject.SetActive(true);
        transform.Find("Notice").gameObject.SetActive(true);
    }

    // �⺻ UI ����
    public void DeActivate()
    {
        for (int x = 0; x < transform.childCount; x++)
            transform.GetChild(x).gameObject.SetActive(false);
    }
}
