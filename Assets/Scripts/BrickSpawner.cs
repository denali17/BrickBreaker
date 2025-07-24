using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
	public Ball ball;
	public Brick m_Brick;
	public List<GridType> m_gridTypes = new();
	public List<Vector2Int> m_gridSizes = new();
	public Vector2 m_spacing = new(2.0f, 2.0f);
	private Vector2 m_brickSize;
	public List<Brick> m_ListOfBricks = new();
	private int levelIndex = 0;

	private bool superBallActive = false;
	private int superBallMaxHits = 3;
	private int superBallHitsRemaining = 0;

	[SerializeField] private Sprite normalBallSprite;
	[SerializeField] private Sprite superBallSprite;

	//Indices in this order: Red, orange, yellow, green, blue, purple
	List<int> MainColourIndices = new() { 35, 27, 19, 11, 3, 43 };

	public enum GridType
	{
		Grid,
		Checkerboard,
		Diagonals
	}

	// Start is called before the first frame update
	void Start()
    {
		// Get brick size from prefab renderer
		Renderer brickRenderer = m_Brick.GetComponent<Renderer>();
		m_brickSize = brickRenderer.bounds.size;

		LoadNextLevel();

		ActivateSuperBall();
	}

	public void ActivateSuperBall()
	{
		superBallActive = true;
		superBallHitsRemaining = superBallMaxHits;

		ball.GetComponent<SpriteRenderer>().sprite = superBallSprite;

		foreach (Brick brick in m_ListOfBricks)
		{
			brick.SetTriggerMode(true);
		}
	}

	public void DeactivateSuperBall()
	{
		superBallActive = false;
		ball.GetComponent<SpriteRenderer>().sprite = normalBallSprite;

		foreach (Brick brick in m_ListOfBricks)
		{
			brick.SetTriggerMode(false);
		}
	}

	public void BrickGotHit(Brick hitBrick)
	{
		if (superBallActive)
		{
			superBallHitsRemaining--;
			hitBrick.DestroyBrick();
			
			if (superBallHitsRemaining == 0)
			{
				DeactivateSuperBall();
			}
		}
	}

	private void SpawnGrid(Vector2Int gridSize, Vector2 spacing)
	{
		// Calculate total grid size
		float totalGridSizeX = (gridSize.x * m_brickSize.x) + m_spacing.x * (gridSize.x - 1);
		float totalGridSizeY = (gridSize.y * m_brickSize.y) + m_spacing.y * (gridSize.y - 1);

		// Offset position to the bottom-center of the spawner
		Vector2 startPosition = new(
			(-totalGridSizeX * 0.5f) + (m_brickSize.x * 0.5f),
			(-totalGridSizeY) + (m_brickSize.y * 0.5f));

		for (int x = 0; x < gridSize.x; x++)  
		{
			for (int y = 0; y < gridSize.y; y++)
			{
				bool isCorner =
				(x == 0 && y == 0) ||							// bottom-left
				(x == 0 && y == gridSize.y - 1) ||				// top-left
				(x == gridSize.x - 1 && y == 0) ||				// bottom-right
				(x == gridSize.x - 1 && y == gridSize.y - 1);	// top-right

				if (gridSize.x >= 3 &&  gridSize.y >= 3 && isCorner)
				{
					continue; // Skip spawing the corners if the grid size is at least 3x3 
				}

				SpawnBricksAt(x, y, gridSize, spacing, startPosition);
			}
		}
	}

	private void SpawnCheckerboardGrid(Vector2Int gridSize, Vector2 spacing)
	{
		// Calculate total grid size
		float totalGridSizeX = (gridSize.x * m_brickSize.x) + m_spacing.x * (gridSize.x - 1);
		float totalGridSizeY = (gridSize.y * m_brickSize.y) + m_spacing.y * (gridSize.y - 1);

		// Offset position to the bottom-center of the spawner
		Vector2 startPosition = new(
			(-totalGridSizeX * 0.5f) + (m_brickSize.x * 0.5f),
			(-totalGridSizeY) + (m_brickSize.y * 0.5f));

		for (int x = 0; x < gridSize.x; x++)
		{
			for (int y = 0; y < gridSize.y; y++)
			{
				if ((x + y) % 2 == 0) // Skip alternating bricks 
				{
					continue;
				}

				SpawnBricksAt(x, y, gridSize, spacing, startPosition);
			}
		}
	}

	private void SpawnDiagonalsGrid(Vector2Int gridSize, Vector2 spacing)
	{
		// Calculate total grid size
		float totalGridSizeX = (gridSize.x * m_brickSize.x) + m_spacing.x * (gridSize.x - 1);
		float totalGridSizeY = (gridSize.y * m_brickSize.y) + m_spacing.y * (gridSize.y - 1);

		// Offset position to the bottom-center of the spawner
		Vector2 startPosition = new(
			(-totalGridSizeX * 0.5f) + (m_brickSize.x * 0.5f),
			(-totalGridSizeY) + (m_brickSize.y * 0.5f));

		for (int x = 0; x < gridSize.x; x++)
		{
			for (int y = 0; y < gridSize.y; y++)
			{
				if ((x - y) % 3 == 0) // Skip third brick
				{
					SpawnBricksAt(x, y, gridSize, spacing, startPosition);
				}
				
			}
		}
	}

	private void SpawnBricksAt(int x, int y, Vector2Int gridSize, Vector2 spacing, Vector2 startPosition)
	{
		float xPosition = x * (m_brickSize.x + spacing.x);
		float yPosition = y * (m_brickSize.y + spacing.y);

		// Apply start position offset when spawning
		Vector2 spawnPosition = startPosition + new Vector2(xPosition, yPosition);


		Brick copy = Instantiate(m_Brick, transform); // Spawn brick

		copy.brickSpawner = this; // Assign this brickspawner to the new brick
		m_ListOfBricks.Add(copy); // Add new brick to the list of bricks

		// Use local position to keep it relative to brick spawner
		copy.transform.localPosition = spawnPosition;

		// Assign each row a colour from top to bottom
		int reverseColourIndex = (gridSize.y - 1) - y; // Reverse colour order, so that top row starts with red
		reverseColourIndex = reverseColourIndex % MainColourIndices.Count; // Cycling rainbow effect

		int colourIndex = MainColourIndices[reverseColourIndex];
		copy.AssignColour(colourIndex);
	}

	public void LoadNextLevel()
	{
		// Load new level based on grid type and gridsize
		GridType gridType = m_gridTypes[levelIndex];
		Vector2Int gridSize = m_gridSizes[levelIndex];

		switch (gridType)
		{
			case GridType.Grid:
				SpawnGrid(gridSize, m_spacing);
				break;
			case GridType.Checkerboard:
				SpawnCheckerboardGrid(gridSize, m_spacing);
				break;
			case GridType.Diagonals:
				SpawnDiagonalsGrid(gridSize, m_spacing);
				break;
		}

		levelIndex++;
	}

	public void RemoveBrick(Brick thisBrick)
	{
		m_ListOfBricks.Remove(thisBrick);

		if (m_ListOfBricks.Count == 0)
		{
			if (levelIndex < m_gridSizes.Count)
			{
				ball.ResetLaunch();
				LoadNextLevel();
			}
			else
			{
				Manager.instance.DestroyedAllBricks();
			}
		}
	}
}
