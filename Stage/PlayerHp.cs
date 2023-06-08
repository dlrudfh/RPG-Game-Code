using UnityEngine.UI;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    private void Update()
    {
        // 플레이 화면 좌측 상단의 체력 바 조절
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("CHP")
                                     / PlayerPrefs.GetFloat("HP");
    }
}
