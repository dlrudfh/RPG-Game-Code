using UnityEngine;

//숨겨진 길 기믹 스크립트
public class HiddenRoad : MonoBehaviour
{
    GameObject chest;
    void Start()
    {
        // 맵에 존재하는 상자 서치
        chest = transform.Find("Chest").gameObject;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 닿으면 숨겨진 공간이 나타남
        if (collision.CompareTag("Player"))
        {
            GetComponent<TileChange>().ChangeTiles();
            chest.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //플레이어가 떨어지면 공간이 원래 상태로 복귀
        if (collision.CompareTag("Player"))
        {
            GetComponent<TileChange>().RecoverTiles();
            chest.SetActive(false);
        }
    }
}
