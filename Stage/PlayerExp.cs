using UnityEngine.UI;
using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    private void Update()
    {
        // 플레이 화면 좌측 상단의 경험치 바 조절
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("XP")
                                     / PlayerPrefs.GetInt("LV");
    }
}