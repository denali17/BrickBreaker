using System.Collections.Generic;
using UnityEngine;

public class LivesSpawner : MonoBehaviour
{
	public Heart m_heart;
	private Vector2 m_heartsize;
	private List<Heart> listOfHearts = new();
	public float m_spacing = 0.0f;

	public void SpawnLives(int numberOfLives)
	{
		SpriteRenderer heartrenderer = m_heart.GetComponent<SpriteRenderer>();
		m_heartsize = heartrenderer.bounds.size;

		for (int i = 0; i < numberOfLives; i++)
		{
			float xPosition = i * (m_heartsize.x + m_spacing);
			Vector2 spawnPosition = new Vector2(xPosition, 0);

			// Spawn hearts
			Heart copy = Instantiate(m_heart, transform);

			// Use local position to keep it relative to lives spawner
			copy.transform.localPosition = spawnPosition;

			//Add new instance to list
			listOfHearts.Add(copy);
		}
	}

	public void UpdateHearts(int livesRemaining)
	{
		for (int i = 0; i < listOfHearts.Count; i++)
		{
			if (i < livesRemaining)
			{
				listOfHearts[i].SetFull();
			}
			else
			{
				listOfHearts[i].SetEmpty();
			}
		}
	}

}
