using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ȿ��
public class Slow : MonoBehaviour
{
    // ���ҵ� �ӵ�
    [SerializeField] float slow;
    bool playerContacted;

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾�� �����ϸ�
        if (collision.CompareTag("Player")){
            // �÷��̾� �ӵ� ����
            playerContacted = true;
            collision.GetComponent<Player>().speed = slow;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �÷��̾�� ������ ������
        if (collision.CompareTag("Player")){
            // �÷��̾� �ӵ� ����
            playerContacted = false;
            collision.GetComponent<Player>().speed = PlayerPrefs.GetFloat("SPEED");
        }
    }

    // �÷��̾ ������ ���·� ������Ʈ�� �����Ǹ� �ӵ��� ���ƿ��� �ʱ� ������
    // ������Ʈ�� ������ ��츦 ���� ó��
    private void OnDestroy()
    {
        if (playerContacted)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().speed
            = PlayerPrefs.GetFloat("SPEED");
        }
            
    }
}
