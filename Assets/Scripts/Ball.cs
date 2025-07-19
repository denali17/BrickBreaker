using UnityEngine;

public class Ball : MonoBehaviour
{
	public float m_speed = 10.0f;
	private Rigidbody2D rigidBody;
	private Vector2 m_startPosition;
	private AudioSource m_audioSource;


	// Start is called before the first frame update
	void Start()
	{
		m_startPosition = transform.localPosition;

		rigidBody = GetComponent<Rigidbody2D>();

		m_audioSource = GetComponent<AudioSource>();

		LaunchBall();
	}

	private void LaunchBall()
	{
		Vector2 initialDirection = Vector2.down;
		rigidBody.velocity = initialDirection * m_speed;
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

			// Stop ball movement and reset position
			rigidBody.velocity = Vector2.zero;
			transform.localPosition = m_startPosition;

			// Add short delay for re-launch after losing life
			Invoke(nameof(LaunchBall), 1f);

		}
	}
}
