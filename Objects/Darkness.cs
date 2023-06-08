using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터 시야 감소 이벤트
public class Darkness : MonoBehaviour
{
    void OnEnable()
    {   
        // 플레이어에게 상속됨
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        // 플레이어를 기준으로 위치를 재배치(플레이어가 해당 오브젝트의 중앙에 오도록)
        transform.localPosition = new Vector3(-0.18f, 0.08f, 0);
        transform.localScale = new Vector3(6, 6, 1);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
