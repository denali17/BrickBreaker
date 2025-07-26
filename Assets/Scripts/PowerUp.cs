using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public BrickSpawner brickSpawner; // Assigned when the powerup gets spawned
	[SerializeField] private float fallSpeed;

	private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed* Time.deltaTime);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Paddle"))
        {
			brickSpawner.ActivateSuperBall();
			// Destroy power up after short delay, so that it disappears once it's underneath the paddle
			Destroy(gameObject, 0.1f); 
		}
        else if (collision.gameObject.name == "BottomWall")
        {
			// Destroy power up after short delay, so that it goes off screen before disappearing
			Destroy(gameObject, 0.5f); 
		}
	}
}
