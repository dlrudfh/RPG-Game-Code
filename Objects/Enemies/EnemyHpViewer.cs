using UnityEngine.UI;
using UnityEngine;

// 적 체력 바 조작
public class EnemyHpViewer : MonoBehaviour
{
    private Enemy enemyHp;
    private Slider hpSlider;

    public void Setup(Enemy enemy)
    {
        enemyHp = enemy;
        hpSlider = GetComponent<Slider>();
    }
    private void Update()
    {
        // 슬라이더 값을 적의 체력 비율에 맞게 업데이트
        hpSlider.value = (float)enemyHp.CurHp / (float)enemyHp.MaxHp;
    }
}
