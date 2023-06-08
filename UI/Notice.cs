using TMPro;
using UnityEngine;

// 공지 메시지 출력 스크립트(화면 중앙에 출력되는 레벨업 메시지 등)
public class Notice : MonoBehaviour
{
    private void Update()
    {
        // Notice문자열에 무언가 입력될 경우
        if (PlayerPrefs.GetString("Notice") != "")
        {
            //해당 문자열을 공지로 출력 후 Notice를 초기화
            GetComponent<TextMeshProUGUI>().text = (PlayerPrefs.GetString("Notice"));
            PlayerPrefs.SetString("Notice", "");
            // 공지는 2초 후 제거됨
            Invoke("clean", 2f);
        }
    }

    private void clean()
    {
        GetComponent<TextMeshProUGUI>().text = "";
    }
}
