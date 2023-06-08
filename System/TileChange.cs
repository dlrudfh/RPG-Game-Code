using UnityEngine;
using UnityEngine.Tilemaps;

//타일 변경 스크립트
public class TileChange : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] int minX = 0;
    [SerializeField] int maxX = 0;
    [SerializeField] int minY = 0;
    [SerializeField] int maxY = 0;
    [SerializeField] Vector3Int[] vector; // 타일을 변경할 위치 벡터 집합
    [SerializeField] TileBase[] t1; // 변경할 타일 집합
    [SerializeField] TileBase[] t2; // 기존 타일 집합
    
    void Start()
    {
        int i = 0;
        if (minX <= maxX && minY <= maxY)
        {
            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    //inspector에서 설정된 벡터 값을 배열에 대입
                    vector[i++] = new Vector3Int(x, y, 0);
                }
            }
        }
    }

    public void ChangeTiles()
    {
        //타일 변경
        tilemap.SetTiles(vector, t1);
    }

    public void RecoverTiles()
    {
        //타일 복구
        tilemap.SetTiles(vector, t2);
    }
}
