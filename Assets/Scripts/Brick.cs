using UnityEngine;

public class Brick : MonoBehaviour
{

    public int m_maxHits = 3;
    private int numberOfHits = 0;
    [SerializeField] Sprite[] brickSprites;
	private int randomColourIndex = 0;
	public BrickSpawner brickSpawner; // Assigned when the brick gets spawned

    
	// Assign colour to brick from the array of sprites
	public void AssignColour(int mainColourIndex)
	{
		// Get sprite renderer of that specific game object 
		SpriteRenderer Renderer = gameObject.GetComponent<SpriteRenderer>();
		if (Renderer != null)
		{
			// Assign colour
			randomColourIndex = mainColourIndex;
			Renderer.sprite = brickSprites[randomColourIndex];
		}
	}

    public void GotHit()
    {
        numberOfHits ++;
		
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
			SpriteRenderer Renderer = gameObject.GetComponent<SpriteRenderer>();
			int nextSpriteIndex = randomColourIndex + numberOfHits;
			Renderer.sprite = brickSprites[nextSpriteIndex];
		}
	}
}
