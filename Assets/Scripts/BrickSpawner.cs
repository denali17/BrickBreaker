using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
	public GameObject m_Brick;
	public Vector2Int m_gridSize = new(4, 4);
	public Vector2 m_spacing = new(2.0f, 2.0f);
	private Vector2 m_brickSize;

	// Start is called before the first frame update
	void Start()
    {
		//Get brick size from prefab renderer
		Renderer brickRenderer = m_Brick.GetComponent<Renderer>();
		m_brickSize = brickRenderer.bounds.size;

		BrickSpawnerGrid(m_gridSize, m_spacing);
	}

	private void BrickSpawnerGrid(Vector2Int gridSize, Vector2 spacing)
	{
		//Calculate total grid size
		float totalGridSizeX = (m_gridSize.x * m_brickSize.x) + m_spacing.x * (m_gridSize.x - 1);
		float totalGridSizeY = (m_gridSize.y * m_brickSize.y) + m_spacing.y * (m_gridSize.y - 1);

		//Offset position to the center of the spawner
		Vector2 startPosition = new(
			(-totalGridSizeX * 0.5f) + (m_brickSize.x * 0.5f),
			(-totalGridSizeY * 0.5f) + (m_brickSize.y * 0.5f));

		for (int x = 0; x < gridSize.x; x++)  
		{
			for (int y = 0; y < gridSize.y; y++)
			{
				float xPosition = x * (m_brickSize.x + spacing.x);
				float yPosition = y * (m_brickSize.y + spacing.y);

				//Apply start position offset when spawning
				Vector2 spawnPosition = startPosition + new Vector2(xPosition,yPosition);

				GameObject copy = Instantiate(m_Brick, transform);

				// Use local position to keep it relative to brick spawner
				copy.transform.localPosition = spawnPosition;
			}
		}
	}
}
