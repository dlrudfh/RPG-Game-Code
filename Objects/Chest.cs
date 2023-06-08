using UnityEngine;

// 상자 오브젝트
public class Chest : MonoBehaviour
{
    bool nearby;
    bool open;
    [SerializeField] int reward;
    [SerializeField] Sprite img;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
            nearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            nearby = false;
        }
    }

    private void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFFECT");
        // 열리지 않은 상태 & 플레이어 인접 & 액션 키 활성화
        if (!open && nearby && Input.GetKeyUp((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ACTION"), true)))
        {
            open = true;
            // 시스템 공지 창으로 상자 보상 획득 메시지를 출력함
            PlayerPrefs.SetString("Notice", "You got " + reward + "G!");
            // 상자 오픈 효과음 재생
            GetComponent<AudioSource>().Play();
            // 보상 획득
            PlayerPrefs.SetInt("GOLD", PlayerPrefs.GetInt("GOLD") + reward);
            // 열려있는 상자 이미지로 변경
            GetComponent<SpriteRenderer>().sprite = img;
        }
    }
}
