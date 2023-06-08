using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

// ����Ű ����
public class KeySetting : MonoBehaviour
{
    // Ű���� �Է��� ������ ������
    private void OnGUI()
    {
        Event e = Event.current;
        // Ű���� �Է��� �����Ǹ�
        if (e != null && e.type == EventType.KeyDown)
        {
            // ���� �����Ϸ��� ����Ű
            string key = PlayerPrefs.GetString("KEY");
            // �����Ϸ��� ����Ű�� �Էµ� Ű�� ����
            PlayerPrefs.SetString(key, e.keyCode.ToString());
            // ����Ű�� ���� �÷��̿� �����Ŵ
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Key();
            GameObject.FindGameObjectWithTag(key).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString(key);
            transform.parent.parent.GetComponentInParent<UIinfo>().ApplyKey();
            // ����Ű �Է� â ����
            gameObject.SetActive(false);
        }
    }
}