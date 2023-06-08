using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아래로 내려찍는 돌덩이 오브젝트
public class Rockhead : MonoBehaviour
{
    GameObject player;
    float speed = 10; // 떨어지는 속도
    int fall = 0; // 떨어지는 상태(0 = 정지, 1 = 하강, 2 = 상승)
    [SerializeField] float maxY; // 최대 높이(기존 위치)
    [SerializeField] float minY; // 최소 높이(바닥)
    
    void Start()
    {   
        // 플레이어 오브젝트 탐색
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (transform.position.y <= minY) fall = 2; // 바닥에 닿을 경우 상승 상태
        else if (transform.position.y >= maxY) // 원래 위치로 복귀 완료했을 경우
        {   
            // 플레이어가 오브젝트 하단의 인식범위 내에 들어왔을 경우
            if (player.transform.position.y <= transform.position.y &&
            player.transform.position.x >= transform.position.x - 0.5f &&
            player.transform.position.x <= transform.position.x + 0.5f) 
                fall = 1; // 하강상태
            else fall = 0; // 정지(오브젝트가 기존 위치보다 올라가지 않도록 하기 위함)
        }
        // 하단에 플레이어 존재 시 급격하게 하강
        if (fall==1) transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
        // fall 변수가 2일 시 천천히 상승
        else if (fall==2) transform.position += new Vector3(0, 1, 0) * 1 * Time.deltaTime;
    }
}
