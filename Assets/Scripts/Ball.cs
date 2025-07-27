using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private Rigidbody2D _rigidBody;
	private Collider2D _collider;
	private AudioSource _audioSource;
	private TrailRenderer _trail;

	[SerializeField] private Transform player;
	[SerializeField] private TextMeshProUGUI launchPrompt;
	[SerializeField] private Vector2 offset = new();
	[SerializeField] private float speed = 10.0f;
	private Vector2 _playerTransform;
	private bool _hasLaunched = false;

	public bool superBallActive = false;

	private void Start()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
		_audioSource = GetComponent<AudioSource>();
		_collider = GetComponent<Collider2D>();
		_trail = GetComponent<TrailRenderer>();
	}

	private void Update()
	{
		if (!_hasLaunched)
		{
			// Attach ball to the player for launching
			_playerTransform = new Vector2(player.position.x, player.position.y);
			transform.position = _playerTransform + offset;

			// Launch with spacebar
			if (Input.GetKeyDown(KeyCode.Space))
			{
				LaunchBall();
			}
		}
	}

	private void LaunchBall()
	{
		// Launch the ball in an upward direction, with a chance of going slightly off center
		Vector2 initialDirection = new Vector2(Random.Range(-0.08f, 0.08f), 1.0f).normalized;
		_rigidBody.velocity = initialDirection * speed;
		
		_hasLaunched = true;
		_collider.enabled = true;
		SetTrailEnabled(true); // Turn on trail if superball active
		launchPrompt.gameObject.SetActive(false); // Turn off launch prompt
	}

	private void FixedUpdate()
	{
		// Keep the speed of the ball constant
		_rigidBody.velocity = _rigidBody.velocity.normalized * speed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Brick"))
		{
			BrickCollisions(collision.collider);
		}
		else if (collision.gameObject.CompareTag("Paddle"))
		{
			Vector2 ballCenter = transform.position;
			Vector2 paddleCenter = collision.transform.position;
			float paddleWidth = collision.collider.bounds.size.x;

			// Collision position from the center of the paddle, in range -1 to 1
			float hitPoint = ballCenter.x - paddleCenter.x;

			// Normalize value into the range of -1 to 1
			float normalizedHitPoint = hitPoint / (paddleWidth * 0.5f);

			// Greater influence towards the end of the paddle and less influence near the center
			float influence = Mathf.SmoothStep(0f, Mathf.Sign(normalizedHitPoint), Mathf.Abs(normalizedHitPoint));

			// Apply new direction based on where the ball hit the paddle
			Vector2 newDirection = new Vector2(influence, 1.0f).normalized;
			_rigidBody.velocity = newDirection * speed;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Brick"))
		{
			BrickCollisions(collision);
		}
		else if (collision.gameObject.name == "BottomWall")
		{
			Manager.instance.LoseLives();

			ResetLaunch();
		}
	}

	public void ResetLaunch()
	{
		// Stop ball movement
		_rigidBody.velocity = Vector2.zero;

		// Reset launch
		_hasLaunched = false;
		_collider.enabled = false; // Turn off collider so bricks can't get hit during resetting
		SetTrailEnabled(false);

		// Show launch prompt, except when game over panel is active
		if (!Manager.instance.gameOverPanel.activeInHierarchy)
		{
			launchPrompt.gameObject.SetActive(true);
		}
	}

	public void SetTrailEnabled(bool enabled)
	{
		if (enabled && _hasLaunched && superBallActive)
		{
			_trail.enabled = true; // Only enable if conditions are met
		}
		else
		{
			_trail.enabled = false;
		}
	}

	private void BrickCollisions(Collider2D collision)
	{
		_audioSource.pitch = Random.Range(0.8f, 1.2f); // Randomly shift pitch with every hit
		_audioSource.Play();

		Brick thisBrick = collision.gameObject.GetComponent<Brick>();
		thisBrick.GotHit();
	}
}
