using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

//Skill UI창
public class SkillUI : MonoBehaviour, IDragHandler
{

    RectTransform rectTransform;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject ChargeShot;
    [SerializeField] GameObject Dash;
    [SerializeField] GameObject Heal;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 스킬 습득 제한 레벨 미만일 경우 관련 문구 출력
        // 스킬을 마스터했을 경우 마스터했다는 문구 출력
        // 스킬을 습득하는 중일 경우 스킬 레벨 출력
        if (PlayerPrefs.GetInt("LV") < 3)
            ChargeShot.GetComponent<TextMeshProUGUI>().text = "Required LV.3";
        else if (PlayerPrefs.GetInt("CHARGESHOTLV") == 10)
            ChargeShot.GetComponent<TextMeshProUGUI>().text = "Mastered";
        else
            ChargeShot.GetComponent<TextMeshProUGUI>().text = "ChargeShot LV." + PlayerPrefs.GetInt("CHARGESHOTLV");

        if (PlayerPrefs.GetInt("LV") < 5)
            Dash.GetComponent<TextMeshProUGUI>().text = "Required LV.5";
        else if (PlayerPrefs.GetInt("DASHLV") == 10)
            Dash.GetComponent<TextMeshProUGUI>().text = "Mastered";
        else
            Dash.GetComponent<TextMeshProUGUI>().text = "Dash LV." + PlayerPrefs.GetInt("DASHLV");

        if (PlayerPrefs.GetInt("LV") < 10)
            Heal.GetComponent<TextMeshProUGUI>().text = "Required LV.10";
        else if (PlayerPrefs.GetInt("HEALLV") == 10)
            Heal.GetComponent<TextMeshProUGUI>().text = "Mastered";
        else
            Heal.GetComponent<TextMeshProUGUI>().text = "Heal LV." + PlayerPrefs.GetInt("HEALLV");

    }

    //Skill UI는 마우스로 드래그가 가능하다.
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}