using System.Collections;
using UnityEngine;

public class Brick : MonoBehaviour
{

    public int m_maxHits = 3;
    private int numberOfHits = 0;
    [SerializeField] Sprite[] brickSprites;
	private SpriteRenderer m_spriteRenderer;
	private int randomColourIndex = 0;
	public BrickSpawner brickSpawner; // Assigned when the brick gets spawned

	[SerializeField] private float m_flashTime = 0.2f;

	private void Awake()
	{
		// Get sprite renderer of this specific game object 
		m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}

	// Assign colour to brick from the array of sprites
	public void AssignColour(int mainColourIndex)
	{
		if (m_spriteRenderer != null)
		{
			// Assign colour
			randomColourIndex = mainColourIndex;
			m_spriteRenderer.sprite = brickSprites[randomColourIndex];
		}
	}

	private IEnumerator DamageFlash()
	{
		Material material = m_spriteRenderer.material;

		float time = 0.0f;

		// Decrease flash amount over time 
		while (time < m_flashTime)
		{
			float currentFlashAmount = Mathf.Lerp(1.0f, 0.0f, time / m_flashTime);
			material.SetFloat("_FlashAmount", currentFlashAmount);
			time += Time.deltaTime;

			yield return null;
		}
		// Reset flash to zero
		material.SetFloat("_FlashAmount", 0.0f);
	}

	public void GotHit()
    {
		numberOfHits ++;

		// Make the brick flash when it gets hit 
		StartCoroutine(DamageFlash());

		// Set collider to be trigger on the second to last hit, so that
		// Ball moves through the brick on the last hit (makes it feel like the ball has more punch)
		if (numberOfHits == m_maxHits-1)
		{
			Collider2D collider = gameObject.GetComponent<Collider2D>();
			collider.isTrigger = true;
		}

		// Destroy brick when max hits has been reached
		if (numberOfHits == m_maxHits)
        {
			brickSpawner.RemoveBrick(this); // Remove brick from list 
			Destroy(gameObject); // Destroy brick
			Manager.instance.IncreaseScore(10); // Increase Score
		}
		
		// Change sprite when brick gets hit
		else
		{
			int nextSpriteIndex = randomColourIndex + numberOfHits;
			m_spriteRenderer.sprite = brickSprites[nextSpriteIndex];
		}
	}
}
