using UnityEngine;

public class Movement2D : MonoBehaviour
{
	private float moveSpeed;
	private Vector3 moveDirection;
	public int jumpCount; // ����Ƚ��(�̴����� Ȱ��)
	bool doNotJump; // ������ �� �� ���� ����
	public bool doNotDash; // ��ø� �� �� ���� ����
	public bool dashCooltime;
	private Animator anime;

	public void Setup(float speed, Vector3 direction)
	{
		moveDirection = direction;
		moveSpeed = speed;
	}

	// ����
    public void Jump(Animator a, AudioSource j)
    {
		anime = a;
		// ������ �� �� �ִ� ���¶��
		if (doNotJump == false)
        {
			// ���� ���� true(���� �ִϸ��̼� ���)
			anime.SetBool("isJumping", true);
			// ����ī��Ʈ�� 1 ������ ��(���� ���� Ƚ���� �����ִٸ�)
			if (jumpCount <= 1)
            {
				j.Play();
				jumpCount++;
				// ���� 10�� �ӵ��� ����
				GetComponent<Rigidbody2D>().velocity = Vector2.up * 10;
			}
		}
	}

	// ���� ����
	public void Down(Animator a, AudioSource j)
	{
		anime = a;
		// �÷��̾��� y�� �ӵ��� 0�� ���(���������� �Ϸ��� �ٴڿ� ��� �־�� �ϱ� ����)
		if (GetComponent<Rigidbody2D>().velocity.y == 0)
		{
			// �ϰ� ���� true(�ϰ� �ִϸ��̼� ���)
			anime.SetBool("isFalling", true);
			j.Play();
			PlayerPrefs.SetInt("FALL", 1);
		}
	}

	// ���
	public void Dash(int dir, Animator a, AudioSource d)
    {
		// ��ø� �� �� �ִ� ���¶��
		if(!doNotDash && !dashCooltime)
        {
			d.Play();
			// ��� �߿��� ���, ���� ��� �Ұ���
			doNotDash = true;
			doNotJump = true;
			dashCooltime = true;
			anime = a;
			// ��� ���� true(��� �ִϸ��̼� ���)
			anime.SetBool("isDashing", true);
			// �Լ��� ���ڷ� �־��� �÷��̾� ���� ���� ������� 15�� �ӵ��� �������� ���
			gameObject.GetComponent<Rigidbody2D>().velocity = ((dir==1 ? Vector2.right : Vector2.left)) * 15.0f;
			// ��� �߿��� �Ʒ��� �������� ����
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			// 0.2�� ���� ��� �� ����
			Invoke("DashEnd", 0.2f);
		}
    }

	// ��� ����
	private void DashEnd()
    {
		// ��� �ִϸ��̼� ����, �ϰ� �ִϸ��̼� ����(�ٴڿ��� ��ø� ������� ���
		// �ϰ� �ִϸ��̼��� �����ǰ� idle �ִϸ��̼����� �ڵ� ��ȯ��)
		anime.SetBool("isFalling", true);
		anime.SetBool("isDashing", false);
		// �÷��̾��� �߷� �� ����
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
		// ��ð� ����Ǿ����Ƿ� ���� ����
		doNotJump = false;
		Invoke("CanDash", 0.1f);
		// ��� ������ ���� ��Ÿ�� �ο�
		Invoke("DoNotDash", 3.3f-(0.3f*PlayerPrefs.GetInt("DASHLV")));
	}

	// ��ð� ������ �ٷ� ��ð� ������ ���¸� ����� �ְ� �Ǹ�
	// y�� �ӵ��� 0�� �� ����ī��Ʈ �˻縦 �ϰ� �Ǿ�
	// ��ø� �� ������ ������ 1ȸ �߰��� �� �� �ִ� ������ �����Ƿ�
	// �ణ�� ���� �ΰ� ������ �ٲ���
	private void CanDash()
    {
		doNotDash = false;
	}

	private void DoNotDash()
    {
		// Invoke�� ���� ��Ÿ�� ��ŭ�� �ð��� ����� �� �ٽ� ��ð� ��������
		dashCooltime = false;
	}

	private void Update()
	{
		// ���ο� ��ġ = ���� ��ġ + (���� * �ӵ�)
		transform.position += moveDirection * moveSpeed * Time.deltaTime;
	}
}

