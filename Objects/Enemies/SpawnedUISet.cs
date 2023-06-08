using UnityEngine;
using UnityEngine.UI;

// 몬스터 체력 바 위치 세팅
public class SpawnedUISet : MonoBehaviour
{
    private Vector3 distance = Vector3.up * 30.0f; // 몬스터 오브젝트 상단에 체력 바 위치
    private Transform targetTransform;
    private RectTransform rectTransform;
    public void Setup(Transform target)
    {
        targetTransform = target; // 적 트랜스폼 값 대입
        rectTransform = GetComponent<RectTransform>(); // 체력 바의 트랜스폼
    }
    private void LateUpdate()
    {
        // 적 트랜스폼이 존재하지 않을 때(적이 처치되었을 때)
        if(targetTransform == null)
        {
            // 체력 바 또한 제거
            Destroy(gameObject);
            return;
        }
        // 적 트랜스폼의 이동 경로와 동일하게 이동함
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition + distance;
    }
}
