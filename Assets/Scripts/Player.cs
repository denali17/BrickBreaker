using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float speed;

	private Rigidbody2D _rigidBody;
	private float _moveInput;

	private void Start()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		// Get horizontal input (-1 = left, 1 = right)
		_moveInput = Input.GetAxisRaw("Horizontal");
	}

	private void FixedUpdate()
	{
		// Apply movement to Player
		_rigidBody.velocity = new Vector2(_moveInput * speed, _rigidBody.velocity.y);
	}
}
