using UnityEngine;

//총알 오브젝트
public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알이 벽에 닿으면 없어짐
        if (collision.gameObject.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 총알이 무한정 날아가지 않고 생성 1초 뒤 삭제됨
        Destroy(gameObject, 1);
    }
}
