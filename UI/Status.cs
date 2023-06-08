using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

//상태창
public class Status : MonoBehaviour, IDragHandler
{

    RectTransform rectTransform;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject LV;
    [SerializeField] GameObject DMG;
    [SerializeField] GameObject HP;
    [SerializeField] GameObject XP;
    [SerializeField] GameObject GOLD;
    [SerializeField] GameObject PTS;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //플레이어 상태 관련 정보를 출력함
        LV.GetComponent<TextMeshProUGUI>().text = "LV." + PlayerPrefs.GetInt("LV");
        DMG.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("DMG").ToString();
        HP.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("CHP") + "/" + PlayerPrefs.GetFloat("HP");
        XP.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("XP") / PlayerPrefs.GetInt("LV") * 100 + "%";
        GOLD.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("GOLD") + "G";
        PTS.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("PTS") + "PTS";
    }

    // 상태창은 마우스로 드래그가 가능하다.
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

}