using UnityEngine;

//������ �� ��� ��ũ��Ʈ
public class HiddenRoad : MonoBehaviour
{
    GameObject chest;
    [SerializeField] GameObject img;
    void Start()
    {
        // �ʿ� �����ϴ� ���� ��ġ
        chest = transform.Find("Chest").gameObject;
    }

    //�÷��̾ ������ ������ ������ ��Ÿ��
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ���� ������ �̹����� ��ü���� ��� Ÿ���� �ǵ����� ����
            if (img != null) img.SetActive(false);
            //Ÿ���� ���� ����
            else GetComponent<TileChange>().ChangeTiles();
            chest.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ���� ������ �̹����� ��ü���� ��� Ÿ���� �ǵ����� ����
            if (img != null) img.SetActive(true);
            //Ÿ���� ���� ����
            else GetComponent<TileChange>().RecoverTiles();
            chest.SetActive(false);
        }
    }
}
