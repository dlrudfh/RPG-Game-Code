using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 감속 효과
public class Slow : MonoBehaviour
{
    // 감소된 속도
    [SerializeField] float slow;
    bool playerContacted;

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 접촉하면
        if (collision.CompareTag("Player")){
            // 플레이어 속도 감소
            playerContacted = true;
            collision.GetComponent<Player>().speed = slow;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어와 접촉이 끝나면
        if (collision.CompareTag("Player")){
            // 플레이어 속도 복구
            playerContacted = false;
            collision.GetComponent<Player>().speed = PlayerPrefs.GetFloat("SPEED");
        }
    }

    // 플레이어가 접촉한 상태로 오브젝트가 삭제되면 속도가 돌아오지 않기 때문에
    // 오브젝트가 삭제될 경우를 따로 처리
    private void OnDestroy()
    {
        if (playerContacted)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().speed
            = PlayerPrefs.GetFloat("SPEED");
        }
            
    }
}
