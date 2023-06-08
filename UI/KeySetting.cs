using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

// 단축키 설정
public class KeySetting : MonoBehaviour
{
    // 키보드 입력이 감지될 때까지
    private void OnGUI()
    {
        Event e = Event.current;
        // 키보드 입력이 감지되면
        if (e != null && e.type == EventType.KeyDown)
        {
            // 현재 변경하려는 단축키
            string key = PlayerPrefs.GetString("KEY");
            // 변경하려는 단축키를 입력된 키로 변경
            PlayerPrefs.SetString(key, e.keyCode.ToString());
            // 단축키를 실제 플레이에 적용시킴
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Key();
            GameObject.FindGameObjectWithTag(key).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString(key);
            transform.parent.parent.GetComponentInParent<UIinfo>().ApplyKey();
            // 단축키 입력 창 삭제
            gameObject.SetActive(false);
        }
    }
}
