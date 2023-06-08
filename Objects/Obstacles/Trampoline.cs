using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 트램펄린(점프대)
public class Trampoline : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 위에서 떨어지며 충돌한 경우(좌우 방향 이동으로 충돌할 경우에는 동작하지 않음)
        if (collision.CompareTag("Player") && collision.GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            // 플레이어가 위로 튀어오름
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.up * 15;
            collision.GetComponent<Movement2D>().jumpCount = 1;
            // 트램펄린 애니메이션을 한 번 실행
            GetComponent<Animator>().Rebind();
            GetComponent<Animator>().Play("active");
        }
    }
}
