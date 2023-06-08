using UnityEngine;
using UnityEngine.SceneManagement;

// 맵(씬) 전환 스크립트
// 맵의 끝으로 이동하면 자동으로 다음 맵으로 넘어가도록 하는 스크립트
public class MapSwitch : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private float x;
    [SerializeField] private float y;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 맵 이동 오브젝트에 충돌하였고,
        // 보스전 등 맵 이동이 불가능한 상태가 아닐 때
        if (collision.gameObject.CompareTag("Player") && PlayerPrefs.GetInt("SwitchLock") == 0)
        {
            // 씬 전환 후 좌표 설정
            SceneManager.LoadScene(scene);
            PlayerPrefs.SetFloat("x", x);
            PlayerPrefs.SetFloat("y", y);
            // 미끄러짐 상태 해제
            PlayerPrefs.SetInt("SLIP", 0);
        }
    }
}
