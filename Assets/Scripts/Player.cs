using UnityEngine;

public class Player : MonoBehaviour
{
	private float m_speed = 5.0f;
	private Rigidbody2D rigidBody;
	private float m_moveInput;


	// Start is called before the first frame update
	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		// Get horizontal input (-1 = left, 1 = right)
		m_moveInput = Input.GetAxisRaw("Horizontal");
	}

	private void FixedUpdate()
	{
		//Apply movement to Player
		rigidBody.velocity = new Vector2(m_moveInput * m_speed, rigidBody.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Collided with: " + collision.gameObject.name);
	}
}
