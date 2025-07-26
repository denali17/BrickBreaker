using System.Collections;
using UnityEngine;

public class Brick : MonoBehaviour
{
	[HideInInspector] public BrickSpawner brickSpawner; // Assigned when the brick gets spawned
	[SerializeField] private Sprite[] brickSprites;

	[HideInInspector] public int maxHits = 3;
	[HideInInspector] public int numberOfHits = 0;
	[SerializeField] private float flashTime;

	private Collider2D _collider;
	private SpriteRenderer _spriteRenderer;
	private int _randomColourIndex = 0;

	private void Awake()
	{
		// Get sprite renderer and collider of this specific game object 
		_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		_collider = GetComponent<Collider2D>();
	}

	public void AssignColour(int mainColourIndex)
	{
		if (_spriteRenderer != null)
		{
			// Assign colour to brick
			_randomColourIndex = mainColourIndex;
			_spriteRenderer.sprite = brickSprites[_randomColourIndex];
		}
	}

	private IEnumerator DamageFlash()
	{
		Material material = _spriteRenderer.material;

		float time = 0.0f;

		// Decrease flash amount over time 
		while (time < flashTime)
		{
			float currentFlashAmount = Mathf.Lerp(1.0f, 0.0f, time / flashTime);
			material.SetFloat("_FlashAmount", currentFlashAmount);
			time += Time.deltaTime;

			yield return null;
		}
		// Reset flash to zero
		material.SetFloat("_FlashAmount", 0.0f);
	}

	public void GotHit()
    {
		// Bricks set to trigger should be destroyed immediately
		if (_collider.isTrigger)
		{
			numberOfHits = maxHits;
		}
		else
		{
			numberOfHits++;
		}

		// Make the brick flash when it gets hit 
		StartCoroutine(DamageFlash());

		// Set collider to be trigger on the second to last hit, so that
		// Ball moves through the brick on the last hit (makes it feel like the ball has more punch)
		if (numberOfHits == maxHits-1)
		{
			_collider.isTrigger = true;
		}

		// Destroy brick when max hits has been reached
		if (numberOfHits >= maxHits)
        {
			DestroyBrick();
		}
		// Change sprite when brick gets hit
		else
		{
			int nextSpriteIndex = _randomColourIndex + numberOfHits;
			_spriteRenderer.sprite = brickSprites[nextSpriteIndex];
		}

		brickSpawner.BrickGotHit(this);
	}

	private void DestroyBrick()
	{
		brickSpawner.RemoveBrick(this); // Remove brick from list 
		Destroy(gameObject); // Destroy brick
		Manager.instance.IncreaseScore(10); // Increase Score
	}

	public void SetTriggerMode(bool isEnabled)
	{
		_collider.isTrigger = isEnabled;
	}
}
