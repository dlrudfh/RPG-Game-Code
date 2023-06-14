using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCaller : MonoBehaviour
{
    //�� �̸��� string������ ���ڷ� �޾� ���� ȣ���ϴ� �Լ�
    public void SceneCall(string scene)
    {
        // ü�� �ִ�ġ�� ȸ��(���� ���� �� ���÷��̿� Ȱ��)
        PlayerPrefs.SetFloat("CHP", PlayerPrefs.GetFloat("HP"));
        // ���� �������� �� ��Ȱ��ȭ�� UI�� ��Ȱ��ȭ
        GameObject.Find("System").GetComponent<UIinfo>().Activate();
        // �� ����
        PlayerPrefs.SetString("Scene", scene);
        // �� �ε�
        SceneManager.LoadScene(scene);
    }

    // ������ ó�� ������ �� ȣ��Ǵ� �� �ε� �Լ�
    public void GameStart()
    {
        //ü�� �ִ�ġ�� ȸ��
        PlayerPrefs.SetFloat("CHP", PlayerPrefs.GetFloat("HP"));
        // ���� ����� UI�� Ȱ��ȭ
        Transform s = GameObject.Find("System").transform;
        s.GetChild(0).gameObject.SetActive(true);
        s.GetChild(1).gameObject.SetActive(true);
        s.GetChild(2).gameObject.SetActive(true);
        s.GetChild(3).gameObject.SetActive(true);
        // ù ���� �� village �ʿ��� ����
        if (PlayerPrefs.GetString("Scene") == "")
        {
            SceneManager.LoadScene("Village");
            //�÷��̾ �˸��� ��ġ�� �̵���Ű�� ���� ��ǥ
            PlayerPrefs.SetFloat("x", -6);
            PlayerPrefs.SetFloat("y", -4.5f);
        }
        // ������ ������ ������ �����
        else SceneManager.LoadScene(PlayerPrefs.GetString("Scene"));
    }
}
