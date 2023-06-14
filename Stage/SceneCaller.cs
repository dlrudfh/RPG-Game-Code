using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCaller : MonoBehaviour
{
    //씬 이름을 string형태의 인자로 받아 씬을 호출하는 함수
    public void SceneCall(string scene)
    {
        // 체력 최대치로 회복(게임 오버 후 리플레이에 활용)
        PlayerPrefs.SetFloat("CHP", PlayerPrefs.GetFloat("HP"));
        // 게임 오버했을 때 비활성화한 UI를 재활성화
        GameObject.Find("System").GetComponent<UIinfo>().Activate();
        // 씬 저장
        PlayerPrefs.SetString("Scene", scene);
        // 씬 로드
        SceneManager.LoadScene(scene);
    }

    // 게임을 처음 시작할 때 호출되는 씬 로드 함수
    public void GameStart()
    {
        //체력 최대치로 회복
        PlayerPrefs.SetFloat("CHP", PlayerPrefs.GetFloat("HP"));
        // 좌측 상단의 UI를 활성화
        Transform s = GameObject.Find("System").transform;
        s.GetChild(0).gameObject.SetActive(true);
        s.GetChild(1).gameObject.SetActive(true);
        s.GetChild(2).gameObject.SetActive(true);
        s.GetChild(3).gameObject.SetActive(true);
        // 첫 시작 시 village 맵에서 시작
        if (PlayerPrefs.GetString("Scene") == "")
        {
            SceneManager.LoadScene("Village");
            //플레이어를 알맞은 위치로 이동시키기 위한 좌표
            PlayerPrefs.SetFloat("x", -6);
            PlayerPrefs.SetFloat("y", -4.5f);
        }
        // 이전에 종료한 씬에서 재시작
        else SceneManager.LoadScene(PlayerPrefs.GetString("Scene"));
    }
}
