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

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Brick"))
		{
			m_audioSource.pitch = Random.Range(0.8f, 1.2f);
			m_audioSource.Play();

			Brick thisBrick = other.gameObject.GetComponent<Brick>();
			thisBrick.GotHit();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "BottomWall")
		{	
			Manager.instance.LoseLives();

			// Stop ball movement
			rigidBody.velocity = Vector2.zero;

			// Reset launch
			m_hasLaunched = false;
		}
	}
}
