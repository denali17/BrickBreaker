using System.Collections.Generic;
using UnityEngine;

public class LivesSpawner : MonoBehaviour
{
	[SerializeField] private Heart heart;

	private List<Heart> _listOfHearts = new();

	[SerializeField] private float spacing = 0.0f;

	public void SpawnLives(int numberOfLives)
	{
		SpriteRenderer heartrenderer = heart.GetComponent<SpriteRenderer>();
		Vector2 heartsize = heartrenderer.bounds.size;

		for (int i = 0; i < numberOfLives; i++)
		{
			float xPosition = i * (heartsize.x + spacing);
			Vector2 spawnPosition = new(xPosition, 0);

			// Spawn hearts
			Heart copy = Instantiate(heart, transform);

			// Use local position to keep it relative to lives spawner
			copy.transform.localPosition = spawnPosition;

			//Add new instance to list
			_listOfHearts.Add(copy);
		}
	}

	public void UpdateHearts(int livesRemaining)
	{
		for (int i = 0; i < _listOfHearts.Count; i++)
		{
			if (i < livesRemaining)
			{
				_listOfHearts[i].SetFull();
			}
			else
			{
				_listOfHearts[i].SetEmpty();
			}
		}
	}
}
