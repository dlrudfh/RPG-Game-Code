using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//대화창
public class ChatWindow : MonoBehaviour
{
    [SerializeField] GameObject branch;
    [SerializeField] GameObject select;
    int selectCount;
    int selectPos = 1;
    KeyCode up;
    KeyCode down;
    KeyCode action;

    //선택지 활성화
    public void BranchOn(int n)
    {
        branch.SetActive(true);
        selectCount = n;
    }

    //선택지 비활성화
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

    //선택지 선택 이후
    public void BranchValue(int i)
    {
        //branchSelect를 true로 변경해 선택을 완료했음을 전달해 이후 대화가 가능해지고, 
        //questAccept 값을 변경해 수락 or 거절 여부 판단 가능
        GameObject.FindGameObjectWithTag("NPC").GetComponent<Npc>().questAccept = i;
        GameObject.FindGameObjectWithTag("NPC").GetComponent<Npc>().branchSelect = true;
        // 선택을 완료했으므로 선택지 창 비활성화
        BranchOff();
    }
}
