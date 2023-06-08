using System.Collections;
using UnityEngine;

// 재화 오브젝트
public class Gold : MonoBehaviour
{
    private bool alreadyPick; // 획득 이벤트가 한 번만 작동되도록 제한하기 위한 변수
    private AudioSource pick; // 획득 효과음

    private void Awake()
    {
        pick = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFFECT");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 해당 오브젝트에 최초로 충돌했을 때
        if (collision.CompareTag("Player") && alreadyPick == false)
        {
            alreadyPick = true; // 이벤트 재발동 방지를 위해 값 변경
            if(CompareTag("coin")) PlayerPrefs.SetInt("GOLD", PlayerPrefs.GetInt("GOLD")+1);
            else PlayerPrefs.SetInt("GOLD", PlayerPrefs.GetInt("GOLD") + 5);
            Pick();
        }
        else if (collision.CompareTag("wall")) // 바닥에 닿았을 경우 정지
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.up * 0;
        }
    }

	public void Drop()
	{
        // 몬스터 사망 시 오브젝트가 잠깐 위로 튀어오른 후 바닥에 떨어지는 애니메이션
		GetComponent<Rigidbody2D>().velocity = Vector2.up * 7.0f;
        GetComponent<Rigidbody2D>().gravityScale = 3;
	}

    public void Pick()
    {
        // 획득 효과음 재생
        pick.Play();
        GetComponent<Rigidbody2D>().velocity = Vector2.up * 7.0f;
        GetComponent<Rigidbody2D>().gravityScale = 3;
        // 획득 직후 점점 흐려지며 마지막에 제거되는 애니메이션
        StartCoroutine("FadeOut");
        Destroy(gameObject, 0.5f);
    }

    // 오브젝트 점점 투명해짐
    private IEnumerator FadeOut()
    {
        float a = 1;
        while (a != 0 && CompareTag("coin")) 
        {
            a = GetComponent<SpriteRenderer>().color.a;
            // 50프레임에 걸쳐 점차 투명해짐
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, a - 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        while (a != 0 && CompareTag("money"))
        {
            a = GetComponent<SpriteRenderer>().color.a;
            // 50프레임에 걸쳐 점차 투명해짐
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, a - 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
    }

}
