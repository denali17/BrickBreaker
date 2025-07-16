using UnityEngine;

public class Ball : MonoBehaviour
{
	public float m_size = 1.0f;
	public float m_speed = 10.0f;
	private Rigidbody2D rigidBody;


	// Start is called before the first frame update
	void Start()
	{
		transform.localScale = new Vector3 (m_size, m_size, 1.0f);
		rigidBody = GetComponent<Rigidbody2D>();

		Vector2 initialDirection = Vector2.down;
		rigidBody.velocity = initialDirection * m_speed;
	}

	private void FixedUpdate()
	{
		// Keep the speed constant
		rigidBody.velocity = rigidBody.velocity.normalized * m_speed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Brick"))
		{
			Destroy(collision.gameObject);
			Manager.instance.IncreaseScore(10);
		}
	}
}
