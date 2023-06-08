using UnityEngine;

public class QuestTab : MonoBehaviour
{
    public int n;

    public void Description()
    {
        // 퀘스트 설명 창을 열어서 퀘스트 정보를 보고자 할 때
        // 클릭한 퀘스트 번호 값을 인자로 넘겨줌으로써 해당 번호의 퀘스트 정보가 출력되도록 함
        transform.parent.GetComponent<Quest>().Description(n);
    }
}
