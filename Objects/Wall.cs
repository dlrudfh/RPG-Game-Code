using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 벽 오브젝트
public class Wall : MonoBehaviour
{
    public bool canPass = true; // true일 경우 하향점프 가능한 벽
    public bool canSpawn = true; // true일 경우 몬스터 스폰 지역에서 스폰이 가능한 벽
    public bool slippery = false; // true일 경우 플레이어가 미끄러짐

    void OnCollisionStay2D(Collision2D collision)
    {
        //플레이어가 접촉할 경우
        if (collision.gameObject.CompareTag("Player"))
        {
            //하향점프
            if(PlayerPrefs.GetInt("FALL") == 1){
                // 해당 벽 오브젝트의 콜라이더를 비활성화해 플레이어가 통과
                GetComponent<BoxCollider2D>().enabled = false;
                // 플레이어의 하향점프 옵션을 제거
                PlayerPrefs.SetInt("FALL", 0);
            }
            //벽이 미끄럽고, 플레이어가 벽의 위에 서 있을 경우 미끄러짐
            if (slippery && collision.transform.position.y > transform.position.y)
                PlayerPrefs.SetInt("SLIP", 1);
            else PlayerPrefs.SetInt("SLIP", 0);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //플레이어가 떨어질 경우
        if (collision.gameObject.CompareTag("Player"))
        {
            // 더이상 미끄러지지 않음
            PlayerPrefs.SetInt("SLIP", 0);
            //하향점프
            if (PlayerPrefs.GetInt("FALL") == 1)
            {
                // 해당 벽 오브젝트의 콜라이더를 비활성화해 플레이어가 통과
                GetComponent<BoxCollider2D>().enabled = false;
                // 플레이어의 하향점프 옵션을 제거
                PlayerPrefs.SetInt("FALL", 0);
            }
        }
    }

}
