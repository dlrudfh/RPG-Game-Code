using UnityEngine;
using UnityEngine.EventSystems;

// 옵션UI창 관련 스크립트
public class Option : MonoBehaviour, IDragHandler
{
    RectTransform rectTransform;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject InputWindow;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    //UI내 존재하는 슬라이더를 이용해 음악 볼륨 조절 가능
    public void MusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MUSIC", volume);
    }

    //UI내 존재하는 슬라이더를 이용해 효과음 조절이 가능
    public void EffectVolume(float volume)
    {
        PlayerPrefs.SetFloat("EFFECT", volume);
    }

    // Option창은 마우스로 드래그가 가능하다.
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // 키세팅 함수
    public void KeySetting(string key)
    {
        // 변경을 윈하는 키 값을 string으로 받아온 뒤 KEY에 저장
        PlayerPrefs.SetString("KEY", key);
        // 단축키 입력 안내 창을 활성화
        InputWindow.SetActive(true);
    }

}
