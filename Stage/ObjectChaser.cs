using UnityEngine;
using System.Collections;

// 카메라가 플레이어를 추적하기 위한 스크립트
public class ObjectChaser : MonoBehaviour
{
    [SerializeField]
    private StageData stage;
    private Vector2 velocity;
    public GameObject player;
    
    private void Start()
    {
        //플레이어 오브젝트를 탐색
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        // 플레이어를 추적함
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MUSIC");
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, 0);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, 0);
        transform.position = new Vector3(posX, posY, transform.position.z);
    }
    private void LateUpdate()
    {
        // 일정 범위를 벗어나면 더이상 추적하지 않음(벽 끝까지 이동할 경우
        // 카메라가 더이상 추적하지 않고 플레이어가 화면 끝까지 이동함)
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stage.LimitMin.x + 9f, stage.LimitMax.x - 9f),
                                         Mathf.Clamp(transform.position.y, stage.LimitMin.y+4, stage.LimitMax.y-5), transform.position.z);
    }
}