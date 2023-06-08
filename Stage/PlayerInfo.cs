using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private void Update()
    {
        // 화면 좌측 상단에 플레이어 정보 출력 UI에서 텍스트 부분을 담당함 
        gameObject.GetComponent<TextMeshProUGUI>().text = "\n<color=#f000ff>LV." + PlayerPrefs.GetInt("LV") + //분홍
                                                          "\n<color=#000000>" + PlayerPrefs.GetFloat("CHP") + "/" + PlayerPrefs.GetFloat("HP") + //검정
                                                          "\n<color=#000000>" + PlayerPrefs.GetFloat("XP") / PlayerPrefs.GetInt("LV") * 100 + "%";
    }
}
