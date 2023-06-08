using UnityEngine;
using UnityEngine.SceneManagement;

// 포탈(맵 이동)
public class Portal : MonoBehaviour
{
    [SerializeField] private string scene; // 이동할 씬
    [SerializeField] private float x; // 도착 씬에서 플레이어가 스폰될 x좌표
    [SerializeField] private float y;// 도착 씬에서 플레이어가 스폰될 y좌표
    bool nearby; // 플레이어가 가까이 있는지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 가까울 경우 밝기를 조절해 표시
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
            nearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //플레이어가 멀어질 경우 밝기를 초기화
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            nearby = false;
        }
    }

    private void Update()
    {
        // 포탈의 활성화 음량은 이펙트 음량을 계승함
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFFECT");
        // 포탈에 접근해 액션 버튼을 눌렀을 때
        if (Input.GetKeyUp((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ACTION"), true)) && nearby)
        {
            //씬 로드 후 좌표 설정, 효과음 재생
            SceneManager.LoadScene(scene);
            PlayerPrefs.SetFloat("x", x);
            PlayerPrefs.SetFloat("y", y);
            GetComponent<AudioSource>().Play();
            nearby = false;
        }
    }
}
