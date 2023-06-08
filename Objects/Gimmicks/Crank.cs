using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 레버
public class Crank : MonoBehaviour
{
    [SerializeField] Sprite down;
    [SerializeField] Sprite up;
    public bool on = false;
    bool nearby; // 플레이어가 접근했는지
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어가 접근했다면 밝기를 살짝 낮춰 활성화가 가능함을 표시
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
            nearby = true;
        }
    }

    void Update()
    {
        // 레버의 동작 음량은 이펙트 음량과 동일함
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFFECT");
        // 액션 키를 눌렀고, 플레이어가 주변에 있을 때
        if (Input.GetKeyUp((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ACTION"), true)) && nearby)
        {
            GetComponent<AudioSource>().Play(); // 레버 동작 음량 재생
            if (on)
            {
                GetComponent<SpriteRenderer>().sprite = up; // 레버 상태 변경
                GetComponent<TileChange>().RecoverTiles();  // 상응하는 타일 상태 변경
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = down; // 레버 상태 변경
                GetComponent<TileChange>().ChangeTiles();   // 상응하는 타일 상태 변경
            }
            on = !on; // 온오프 상태 변경
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 멀어지면
        if (collision.gameObject.CompareTag("Player"))
        {
            // 밝기를 원래 상태로 되돌림
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            nearby = false;
        }
    }
}
