using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ȭâ
public class ChatWindow : MonoBehaviour
{
    [SerializeField] GameObject branch;
    [SerializeField] GameObject select;
    int selectCount;
    int selectPos = 1;
    KeyCode up;
    KeyCode down;
    KeyCode action;

    //������ Ȱ��ȭ
    public void BranchOn(int n)
    {
        branch.SetActive(true);
        selectCount = n;
    }

    //������ ��Ȱ��ȭ
    public void BranchOff()
    {
        branch.SetActive(false);
    }

    private void Update()
    {
        float x = select.transform.localPosition.x;
        float y = select.transform.localPosition.y;
        float z = select.transform.localPosition.z;
        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UP"), true);
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DOWN"), true);
        action = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ACTION"), true);
        if (Input.GetKeyDown(up) && selectPos > 1)
        {
            select.transform.localPosition = new Vector3(x, y + 80, z);
            selectPos--;
        }
        else if (Input.GetKeyDown(down) && selectPos < selectCount)
        {
            select.transform.localPosition = new Vector3(x, y - 80, z);
            selectPos++;
        }
        else if (Input.GetKeyDown(action) && branch.activeSelf)
        {
            BranchValue(selectPos);
        }
    }

    //������ ���� ����
    public void BranchValue(int i)
    {
        //branchSelect�� true�� ������ ������ �Ϸ������� ������ ���� ��ȭ�� ����������, 
        //questAccept ���� ������ ���� or ���� ���� �Ǵ� ����
        GameObject.FindGameObjectWithTag("NPC").GetComponent<Npc>().questAccept = i;
        GameObject.FindGameObjectWithTag("NPC").GetComponent<Npc>().branchSelect = true;
        // ������ �Ϸ������Ƿ� ������ â ��Ȱ��ȭ
        BranchOff();
    }
}
