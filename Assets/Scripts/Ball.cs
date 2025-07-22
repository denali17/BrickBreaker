using UnityEngine;

public class Ball : MonoBehaviour
{
	public float m_speed = 10.0f;
	private Rigidbody2D rigidBody;
	private AudioSource m_audioSource;
	private bool m_hasLaunched = false;
	public Transform player;
	private Vector2 m_playerTransform;
	public Vector2 m_offset = new Vector2(0.0f, 0.5f);

	
	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();

		m_audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (!m_hasLaunched)
		{
			// Attach ball to the player for launching
			m_playerTransform = new Vector2(player.position.x, player.position.y);
			transform.position = m_playerTransform + m_offset;

			if (Input.GetKeyDown(KeyCode.Space))
			{
				LaunchBall();
			}
		}
	}

	private void LaunchBall()
	{
		Vector2 initialDirection = Vector2.up;
		rigidBody.velocity = initialDirection * m_speed;
		m_hasLaunched = true;
	}

	private void FixedUpdate()
	{
		// Keep the speed of the ball constant
		rigidBody.velocity = rigidBody.velocity.normalized * m_speed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Brick"))
		{
			m_audioSource.pitch = Random.Range(0.8f, 1.2f);
			m_audioSource.Play();

			Brick thisBrick = collision.gameObject.GetComponent<Brick>();
			thisBrick.GotHit();
		}

		else if (collision.gameObject.CompareTag("Paddle"))
		{
			ContactPoint2D contact = collision.GetContact(0);
			Vector2 constactPoint = contact.point;
			Vector2 paddleCenter = collision.transform.position;
			float paddleWidth = collision.collider.bounds.size.x;

			// Collision position from the center of the paddle, in range -1 to 1
			float hitPoint = (constactPoint.x - paddleCenter.x);
			// Normalize value into the range of -1 to 1
			float normalizedHitPoint = hitPoint / (paddleWidth * 0.5f);

			// Defining a larger zone near the center of the paddle which makes the ball shoot straight up
			float midZone = 0.1f;
			if (hitPoint > -midZone && hitPoint < midZone)
			{
				normalizedHitPoint = 0.0f;
			}

			// Greater influence towards the end of the paddle and less influence near the center
			float influence = Mathf.SmoothStep(0f, Mathf.Sign(normalizedHitPoint), Mathf.Abs(normalizedHitPoint));

			// Apply new direction based on where the ball hit the paddle
			Vector2 newDirection = new Vector2(influence, 1.0f).normalized;
			rigidBody.velocity = newDirection * m_speed;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "BottomWall")
		{	
			Manager.instance.LoseLives();

			// Stop ball movement
			rigidBody.velocity = Vector2.zero;

			// Reset launch
			m_hasLaunched = false;
		}
	}
}
