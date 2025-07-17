using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    public int m_maxHits = 3;
    private int numberOfHits = 0;
    [SerializeField] Sprite[] brickSprites;
	private int randomColourIndex = 0;
    
	public void AssignColour(int mainColourIndex)
	{
		SpriteRenderer Renderer = gameObject.GetComponent<SpriteRenderer>();
		if (Renderer != null)
		{
			randomColourIndex = mainColourIndex;
			Renderer.sprite = brickSprites[randomColourIndex];
		}
	}

    public void GotHit()
    {
        numberOfHits ++;

        if (numberOfHits == m_maxHits)
        {
			Destroy(gameObject);
			Manager.instance.IncreaseScore(10);
		}

		else
		{
			SpriteRenderer Renderer = gameObject.GetComponent<SpriteRenderer>();
			int nextSpriteIndex = randomColourIndex + numberOfHits;
			Renderer.sprite = brickSprites[nextSpriteIndex];
		}
	}
}
