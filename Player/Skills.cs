using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    bool healCooldown;

    // 힐 스킬
    public void Heal(AudioSource a)
    {
        float chp = PlayerPrefs.GetFloat("CHP");
        float hp = PlayerPrefs.GetFloat("HP");
        // 재사용 대기시간일 경우 동작하지 않음
        if (healCooldown) return;
        // 체력이 이미 가득차 있을 경우 스킬이 발동되지 않고 체력이 가득차 있다는 메시지 출력
        if (chp == hp) PlayerPrefs.SetString("Notice", "Your HP is already full!");
        //체력이 가득 차 있지 않을 경우
        else
        {
            // 파티클 애니메이션 및 효과음 재생
            GameObject.Find("healParticle").GetComponent<ParticleSystem>().Play();
            a.Play();
            // 쿨타임 측정 시작
            healCooldown = true;
            int amount;
            //스킬 레벨에 따라 회복량이 달라짐
            if (PlayerPrefs.GetInt("HEALLV") <= 4) amount = 1;
            else if (PlayerPrefs.GetInt("HEALLV") <= 9) amount = 2;
            else amount = 3;
            //회복량 만큼 체력을 회복함(최대 체력을 넘기지 않음)
            PlayerPrefs.SetFloat("CHP", (chp + amount > hp) ? hp : (chp + amount));
        }
        // 일정 시간 동안 쿨타임 
        Invoke("HealCooldown", 30 - 2 * PlayerPrefs.GetInt("HEALLV"));
    }

    void HealCooldown()
    {
        //일정 시간 후 해당 함수가 호출되면 스킬 재사용이 가능함
        healCooldown = false;
    }
}
