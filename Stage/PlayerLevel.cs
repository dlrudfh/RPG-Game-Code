using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerLevel : MonoBehaviour
{
    public int level;
    public float damage;
    public float exp;
    public float maxHp;
    public float curHp;
    public int point;
    public int gold;
    public int chargeshot;
    public int dash;

    //경험치 획득 함수
    public void GetExp(float enemyExp)
    {
        level = PlayerPrefs.GetInt("LV");
        exp = PlayerPrefs.GetFloat("XP");

        //경험치 획득
        exp += enemyExp;
        //레벨업이 가능할 때
        if(exp>= level)
        {
            //경험치가 100% 이상일 때(경험치를 한 번에 많이 얻어 여러 레벨이
            //올라야 할 수 있으므로 while사용
            while (exp >= PlayerPrefs.GetInt("LV"))
            {
                //레벨업에 필요한 요구 경험치 만큼 경험치를 감소시킴
                exp -= PlayerPrefs.GetInt("LV");
                //레벨, 포인트 증가
                PlayerPrefs.SetInt("LV", PlayerPrefs.GetInt("LV")+1);
                PlayerPrefs.SetInt("PTS", PlayerPrefs.GetInt("PTS")+1);
                // 체력을 최대로 회복
                PlayerPrefs.SetFloat("CHP", PlayerPrefs.GetFloat("HP"));
            }
            // 레벨에 따른 안내 메시지(여러 레벨이 오르면 새로 습득이 가능해진
            // 스킬 중 가장 높은 레벨의 스킬, 없다면 레벨업 메시지 출력)
            if (level < 10 && PlayerPrefs.GetInt("LV") >= 10)
                PlayerPrefs.SetString("Notice", "Now you can use Heal!!");
            else if (level < 5 && PlayerPrefs.GetInt("LV") >= 5)
                PlayerPrefs.SetString("Notice", "Now you can use Dash!!");
            else if (level < 3 && PlayerPrefs.GetInt("LV") >= 3)
                PlayerPrefs.SetString("Notice", "Now you can use Charge Shot!!");
            else
                PlayerPrefs.SetString("Notice", "LEVEL UP!!");
        }
        
        //감소된 경험치를 PlayerPrefs에 반영
        PlayerPrefs.SetFloat("XP", exp);
        point = PlayerPrefs.GetInt("PTS");
    }

    //데미지 증가 함수
    public void DamageUp()
    {
        point = PlayerPrefs.GetInt("PTS");
        damage = PlayerPrefs.GetFloat("DMG");
        // 포인트가 있을 경우
        if (point > 0)
        {
            //포인트를 소모해 데미지업
            PlayerPrefs.SetFloat("DMG", ++damage);
            PlayerPrefs.SetInt("PTS", --point);
        }
        
    }

    //포인트 획득 함수
    public void PointUp()
    {
        point = PlayerPrefs.GetInt("PTS");
        gold = PlayerPrefs.GetInt("GOLD");
        // 충분한 골드가 있을 경우
        if (gold >= 100)
        {
            // 골드를 소모해 포인트 획득
            PlayerPrefs.SetInt("PTS", ++point);
            PlayerPrefs.SetInt("GOLD", gold-100);
        }

    }

    // 체력 증가 함수
    public void HpUp()
    {
        point = PlayerPrefs.GetInt("PTS");
        maxHp = PlayerPrefs.GetFloat("HP");
        curHp = PlayerPrefs.GetFloat("CHP");
        // 포인트가 있을 경우
        if (point > 0)
        {
            // 포인트를 소모해 최대 체력을 증가시킴(현재 체력도 동일 수치만큼 증가)
            PlayerPrefs.SetFloat("HP", ++maxHp);
            PlayerPrefs.SetFloat("CHP", ++curHp);
            PlayerPrefs.SetInt("PTS", --point);
        }

    }

    //스킬 레벨업 함수(버튼으로부터 string형태로 스킬명을 전달받아 해당 스킬 레벨을 올림)
    public void SkillLVUP(string skillLV)
    {
        // 포인트가 있고, 스킬이 만렙이 아닐 경우
        if (PlayerPrefs.GetInt("PTS") > 0 && PlayerPrefs.GetInt(skillLV) < 10)
        {
            //포인트를 소모해 스킬 레벨을 올림
            PlayerPrefs.SetInt(skillLV, PlayerPrefs.GetInt(skillLV) + 1);
            PlayerPrefs.SetInt("PTS", PlayerPrefs.GetInt("PTS") - 1);
        }
    }

    // 피격 데미지 함수
    public void TakeDamage(float enemyDmg, Animator a)
    {
        // 플레이어의 현재 체력이 적의 공격력만큼 감소
        PlayerPrefs.SetFloat("CHP", PlayerPrefs.GetFloat("CHP")-enemyDmg);
        //체력이 0 이하일 경우
        if (PlayerPrefs.GetFloat("CHP") <= 0)
        {
            //사망 상태
            a.SetBool("isDying", true);
            a.SetBool("isRunning", false);
            a.SetBool("isJumping", false);
            a.SetBool("isDashing", false);
            a.SetBool("isFalling", false);
            //1초 후 게임오버 씬으로 전환
            Invoke("Gameover", 1.0f);
        }
    }

    private void Gameover()
    {
        // 보스를 처치하지 않고 게임 오버 맵으로 이동하기 때문에
        // 이동 금지 변수를 조정해서 맵이동이 불가능하지 않도록 설정
        PlayerPrefs.SetInt("SwitchLock", 0);
        // UI를 전부 비활성화(게임 오버 창에서는 UI가 열리지 않음
        GameObject.Find("System").GetComponent<UIinfo>().DeActivate();
        SceneManager.LoadScene("GameOver");
    }

}
