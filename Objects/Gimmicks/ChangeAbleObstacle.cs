using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 통과여부가 조건부로 변경되는 오브젝트
public class ChangeAbleObstacle : MonoBehaviour
{
    [SerializeField] GameObject trigger;
    string t;
    void Start()
    {
        //시작 태그는 'obsta1', 따라서 통과 불가능
        t = tag;
    }

    void Update()
    {
        if (trigger.GetComponent<Crank>().on)// 불켜져있을때
        {
            GetComponent<BoxCollider2D>().isTrigger = true; // 트리거 판정
            tag = t;
        }
        else tag = "Untagged"; // 한번 레버를 동작한 이후에는 태그가 변경되어 장애물 판정이 아님
    }
}
