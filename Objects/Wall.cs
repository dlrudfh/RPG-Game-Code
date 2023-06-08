using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ������Ʈ
public class Wall : MonoBehaviour
{
    public bool canPass = true; // true�� ��� �������� ������ ��
    public bool canSpawn = true; // true�� ��� ���� ���� �������� ������ ������ ��
    public bool slippery = false; // true�� ��� �÷��̾ �̲�����

    void OnCollisionStay2D(Collision2D collision)
    {
        //�÷��̾ ������ ���
        if (collision.gameObject.CompareTag("Player"))
        {
            //��������
            if(PlayerPrefs.GetInt("FALL") == 1){
                // �ش� �� ������Ʈ�� �ݶ��̴��� ��Ȱ��ȭ�� �÷��̾ ���
                GetComponent<BoxCollider2D>().enabled = false;
                // �÷��̾��� �������� �ɼ��� ����
                PlayerPrefs.SetInt("FALL", 0);
            }
            //���� �̲�����, �÷��̾ ���� ���� �� ���� ��� �̲�����
            if (slippery && collision.transform.position.y > transform.position.y)
                PlayerPrefs.SetInt("SLIP", 1);
            else PlayerPrefs.SetInt("SLIP", 0);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //�÷��̾ ������ ���
        if (collision.gameObject.CompareTag("Player"))
        {
            // ���̻� �̲������� ����
            PlayerPrefs.SetInt("SLIP", 0);
            //��������
            if (PlayerPrefs.GetInt("FALL") == 1)
            {
                // �ش� �� ������Ʈ�� �ݶ��̴��� ��Ȱ��ȭ�� �÷��̾ ���
                GetComponent<BoxCollider2D>().enabled = false;
                // �÷��̾��� �������� �ɼ��� ����
                PlayerPrefs.SetInt("FALL", 0);
            }
        }
    }

}
