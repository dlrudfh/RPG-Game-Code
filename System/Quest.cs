using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Quest : MonoBehaviour, IDragHandler
{
    private int questCount = 0;
    [SerializeField] private int c = 0;
    [SerializeField] private bool complete;
    [SerializeField] GameObject questTab;
    RectTransform rectTransform;
    [SerializeField] Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // 퀘스트 창 UI를 마우스로 드래그가 가능하도록 함
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    void Update()
    {
        // 새로운 퀘스트가 존재할 경우
        // CurrentQuest는 평소에는 값이 0이지만, 새로운 퀘스트가 발생하면 해당 퀘스트의 번호 값을 가지게 됨
        if (PlayerPrefs.GetInt("CurrentQuest") != 0)
        {
            //퀘스트 번호를 i에 저장
            int i = PlayerPrefs.GetInt("CurrentQuest");
            // 퀘스트 UI에 탭 생성
            GameObject questTabClone = Instantiate(questTab, new Vector3(0, 0, 1), Quaternion.identity);
            questTabClone.name = i.ToString();
            // 퀘스트가 활성화되었으므로 값 초기화
            PlayerPrefs.SetInt("CurrentQuest", 0);
            // 생성된 퀘스트 탭을 퀘스트 UI의 자식으로 설정
            questTabClone.transform.SetParent(gameObject.transform);
            // 상속되었으므로 스케일, 위치 조정
            questTabClone.transform.localScale = new Vector3(1, 1, 1);
            // 퀘스트 탭의 경우 현재 진행중인 퀘스트가 하나씩 쌓이는 구조이기 때문에
            // 퀘스트 수에 따라 탭의 위치를 배치함
            questTabClone.transform.localPosition = new Vector3(0, 270 - 60 * questCount++, 1);
            // 퀘스트 탭에 퀘스트 번호를 부여함
            questTabClone.GetComponent<QuestTab>().n = i;
            // 퀘스트 탭에 해당 퀘스트의 타이틀을 입력
            questTabClone.GetComponentInChildren<TextMeshProUGUI>().text = QuestSummary.title[i];
        }
    }
    
    //퀘스트 설명 창
    // 퀘스트 탭을 클릭 시 퀘스트 UI우측에 설명 창이 등장하고, 자세한 설명과 함께 진행도와 보상 등을 확인할 수 있다.
    public void Description(int n)
    {
        // 퀘스트 설명 창 오브젝트
        Transform des = transform.GetChild(1);
        //설명 오브젝트 활성화
        des.gameObject.SetActive(true);
        c = n;
        // 설명과 보상을 클릭한 퀘스트 번호에 맞게 출력함
        des.Find("script").GetComponent<TextMeshProUGUI>().text = QuestSummary.description[n];
        des.Find("reward").GetComponent<TextMeshProUGUI>().text = "Reward : " + QuestSummary.reward[n] + "G";
        // 퀘스트의 진행도에 따라 출력값에 변화를 줌
        if ((QuestSummary.progress[n] >= QuestSummary.goal[n]))
        {
            // 퀘스트가 완료되었을 경우 complete = true
            complete = true;
            // 완료 버튼에 Complete가 출력됨
            des.Find("Complete").GetComponentInChildren<TextMeshProUGUI>().text = "Complete";
        }
        else
        {
            //퀘스트가 아직 완료되지 않았을 경우 진행도가 출력됨
            des.Find("Complete").GetComponentInChildren<TextMeshProUGUI>().text = QuestSummary.progress[n]+"/"+QuestSummary.goal[n];
        } 
    }

    //퀘스트 완료
    public void Complete()
    {
        if (complete)
        {
            // 완료 보상 지급
            QuestSummary.Complete(c);
            //퀘스트 탭 제거 후 설명 창을 닫음
            Destroy(GameObject.Find(c.ToString()));
            DesOff();
        }
        
    }

    // 설명 창 닫기
    public void DesOff()
    {
        c = 0;
        // 다른 퀘스트를 열었을 때 퀘스트가 완료되도록 하지 않기 위해 complete = false
        complete = false;
        // 설명 창 비활성화
        GameObject.Find("Description").SetActive(false);
    }
}
