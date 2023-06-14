using UnityEngine;

//숨겨진 길 기믹 스크립트
public class HiddenRoad : MonoBehaviour
{
    GameObject chest;
    [SerializeField] GameObject img;
    void Start()
    {
        // 맵에 존재하는 상자 서치
        chest = transform.Find("Chest").gameObject;
    }

    //플레이어가 닿으면 숨겨진 공간이 나타남
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 기존 지형을 이미지로 대체했을 경우 타일은 건들이지 않음
            if (img != null) img.SetActive(false);
            //타일을 직접 변경
            else GetComponent<TileChange>().ChangeTiles();
            chest.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 기존 지형을 이미지로 대체했을 경우 타일은 건들이지 않음
            if (img != null) img.SetActive(true);
            //타일을 직접 변경
            else GetComponent<TileChange>().RecoverTiles();
            chest.SetActive(false);
        }
    }
}
