using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    public int m_maxHits = 3;
    private int numberOfHits = 0;
    [SerializeField] Sprite[] brickSprites;
	private int randomColourIndex = 0;
    
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
		
		// Destroy brick when out of lives
        if (numberOfHits == m_maxHits)
        {
			Destroy(gameObject);
			Manager.instance.IncreaseScore(10);
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
