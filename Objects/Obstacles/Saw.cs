using UnityEngine;

// 톱니바퀴
public class Saw : MonoBehaviour
{
    [SerializeField] float speed = 3; // 톱니 이동속도
    // x, y좌표 이동범위
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    // x, y좌표 이동방향
    int dirX = 0;
    int dirY = 0;
    void Update()
    {
        // 방향 변수, 속도 변수 값에 따라 반복 이동
        transform.position += new Vector3(dirX, dirY, 0) * speed * Time.deltaTime;
        // x의 이동 반경이 0이 아닐 때
        if (maxX != minX)
        {
            // 좌우 반복 이동
            if (transform.position.x >= maxX) dirX = -1;
            else if (transform.position.x <= minX) dirX = 1;
        }
        // y의 이동 반경이 0이 아닐 때(else if가 아니기 때문에 x, y좌표 동시 이동 가능)
        if (maxY != minY)
        {
            // 상하 반복 이동
            if (transform.position.y >= maxY) dirY = -1;
            else if (transform.position.y <= minY) dirY = 1;
        }
    }
}
