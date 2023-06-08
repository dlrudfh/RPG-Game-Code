using UnityEditor;
using UnityEngine;

//퀘스트 관련 정보가 정리된 스크립트
public class QuestSummary : MonoBehaviour
{
    // 퀘스트의 제목, 설명, 진행도, 목표, 보상, 아이템이 정리된 일종의 텍스트형 스크립트이다.
    public static string[] title = { "", "Mask Dude's request" };
    public static string[] description = { "", "Make Dude has a trouble with ladybugs. I can kill 30 ladybugs for him." };
    public static int[] progress = { 0, 0 };
    public static int[] goal = { 0, 30 };
    public static int[] reward = { 0, 100 };
    public static string[] item = { "", "key" };

    //퀘스트 완료 보상 지급 함수
    public static void Complete(int n)
    {
        // 보상 골드 지급
        PlayerPrefs.SetInt("GOLD", PlayerPrefs.GetInt("GOLD") + reward[n]);
        // 보상 아이템이 특별히 존재할 경우(아직 미사용)
        if (item[n] != "")
        {
        }
    }

}