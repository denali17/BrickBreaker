using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
	[SerializeField] private Ball ball;
	[SerializeField] private Brick brick;
	[SerializeField] private PowerUp powerUp;

	[SerializeField] private List<GridType> gridTypes = new();
	[SerializeField] private List<Vector2Int> gridSizes = new();
	private List<Brick> _listOfBricks = new();

	[SerializeField] private Vector2 spacing = new();
	private Vector2 _brickSize;
	private int _levelIndex = 0;
	private int _bricksUntilNextPowerUp = 0;

	[SerializeField] private int superBallMaxHits = 3;
	private int _superBallHitsRemaining = 0;

	[SerializeField] private Sprite normalBallSprite;
	[SerializeField] private Sprite superBallSprite;
	[SerializeField] private TrailRenderer superBallTrail;
	[SerializeField] private ParticleSystem superBallParticles;

	//Indices in this order: red, orange, yellow, green, blue, purple
	private List<int> _mainColourIndices = new() { 35, 27, 19, 11, 3, 43 };

	public enum GridType
	{
		Grid,
		Checkerboard,
		Diagonals
	}

	private void Start()
	{
		// Get brick size from prefab renderer
		Renderer brickRenderer = brick.GetComponent<Renderer>();
		_brickSize = brickRenderer.bounds.size;

		LoadNextLevel();
		ResetPowerUpCounter();
	}

	private void SpawnGrid(Vector2Int gridSize, Vector2 spacing)
	{
		// Calculate total grid size
		float totalGridSizeX = (gridSize.x * _brickSize.x) + this.spacing.x * (gridSize.x - 1);
		float totalGridSizeY = (gridSize.y * _brickSize.y) + this.spacing.y * (gridSize.y - 1);

		// Offset position to the bottom-center of the spawner
		Vector2 startPosition = new(
			(-totalGridSizeX * 0.5f) + (_brickSize.x * 0.5f),
			(-totalGridSizeY) + (_brickSize.y * 0.5f));

		for (int x = 0; x < gridSize.x; x++)  
		{
			for (int y = 0; y < gridSize.y; y++)
			{
				bool isCorner =
				(x == 0 && y == 0) ||							// bottom-left
				(x == 0 && y == gridSize.y - 1) ||				// top-left
				(x == gridSize.x - 1 && y == 0) ||				// bottom-right
				(x == gridSize.x - 1 && y == gridSize.y - 1);	// top-right

				if (gridSize.x >= 3 && gridSize.y >= 3 && isCorner)
				{
					continue; // Skip spawing the corners (if the grid size is at least 3x3)
				}

				SpawnBricksAt(x, y, gridSize, spacing, startPosition);
			}
		}
	}

	private void SpawnCheckerboardGrid(Vector2Int gridSize, Vector2 spacing)
	{
		// Calculate total grid size
		float totalGridSizeX = (gridSize.x * _brickSize.x) + this.spacing.x * (gridSize.x - 1);
		float totalGridSizeY = (gridSize.y * _brickSize.y) + this.spacing.y * (gridSize.y - 1);

		// Offset position to the bottom-center of the spawner
		Vector2 startPosition = new(
			(-totalGridSizeX * 0.5f) + (_brickSize.x * 0.5f),
			(-totalGridSizeY) + (_brickSize.y * 0.5f));

		for (int x = 0; x < gridSize.x; x++)
		{
			for (int y = 0; y < gridSize.y; y++)
			{
				if ((x + y) % 2 == 0) // Skip alternating bricks to make checkerboard pattern
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
		float totalGridSizeX = (gridSize.x * _brickSize.x) + this.spacing.x * (gridSize.x - 1);
		float totalGridSizeY = (gridSize.y * _brickSize.y) + this.spacing.y * (gridSize.y - 1);

		// Offset position to the bottom-center of the spawner
		Vector2 startPosition = new(
			(-totalGridSizeX * 0.5f) + (_brickSize.x * 0.5f),
			(-totalGridSizeY) + (_brickSize.y * 0.5f));

		for (int x = 0; x < gridSize.x; x++)
		{
			for (int y = 0; y < gridSize.y; y++)
			{
				if ((x - y) % 3 == 0) // Skip third brick to make diagonal lines pattern
				{
					SpawnBricksAt(x, y, gridSize, spacing, startPosition);
				}
				
			}
		}
	}

	private void SpawnBricksAt(int x, int y, Vector2Int gridSize, Vector2 spacing, Vector2 startPosition)
	{
		float xPosition = x * (_brickSize.x + spacing.x);
		float yPosition = y * (_brickSize.y + spacing.y);

		// Apply start position offset when spawning
		Vector2 spawnPosition = startPosition + new Vector2(xPosition, yPosition);

		Brick copy = Instantiate(brick, transform); // Spawn brick

		copy.brickSpawner = this; // Assign this brickspawner to the new brick
		_listOfBricks.Add(copy); // Add new brick to the list of bricks

		// Use local position to keep position relative to brickspawner
		copy.transform.localPosition = spawnPosition;

		if (ball.superBallActive)
		{
			copy.SetTriggerMode(true);
		}

		// Assign each row a colour from top to bottom
		int reverseColourIndex = (gridSize.y - 1) - y; // Top row starting with red
		reverseColourIndex = reverseColourIndex % _mainColourIndices.Count; // Cycling rainbow effect

		int colourIndex = _mainColourIndices[reverseColourIndex];
		copy.AssignColour(colourIndex);
	}

	public void LoadNextLevel()
	{
		// Load new level based on grid type and grid size
		GridType gridType = gridTypes[_levelIndex];
		Vector2Int gridSize = gridSizes[_levelIndex];

		switch (gridType)
		{
			case GridType.Grid:
				SpawnGrid(gridSize, spacing);
				break;
			case GridType.Checkerboard:
				SpawnCheckerboardGrid(gridSize, spacing);
				break;
			case GridType.Diagonals:
				SpawnDiagonalsGrid(gridSize, spacing);
				break;
		}

		_levelIndex++;
	}

	public void ActivateSuperBall()
	{
		ball.superBallActive = true;
		_superBallHitsRemaining = superBallMaxHits;

		// Change to super ball appearance
		ball.GetComponent<SpriteRenderer>().sprite = superBallSprite;
		superBallTrail.Clear();
		ball.SetTrailEnabled(true);
		superBallParticles.Play();

		// Bonus score for collecting the powerup
		Manager.instance.IncreaseScore(300);

		foreach (Brick brick in _listOfBricks)
		{
			brick.SetTriggerMode(true);
		}

		Manager.instance.PlayPowerUpAudio();
	}

	public void DeactivateSuperBall()
	{
		ball.superBallActive = false;
		
		// Reset to normal ball appearance
		ball.GetComponent<SpriteRenderer>().sprite = normalBallSprite;
		ball.SetTrailEnabled(false);
		superBallParticles.Stop();
		superBallParticles.Clear();

		foreach (Brick brick in _listOfBricks)
		{
			if (brick.numberOfHits == brick.maxHits - 1)
			{
				continue; // Bricks that only have one hit left should stay in trigger mode
			} 
			
			brick.SetTriggerMode(false);
		}
	}

	private void ResetPowerUpCounter()
	{
		_bricksUntilNextPowerUp = Random.Range(5, 8);
	}

	public void RemoveBrick(Brick thisBrick)
	{
		_listOfBricks.Remove(thisBrick);
		_bricksUntilNextPowerUp--;

		if (_bricksUntilNextPowerUp <= 0)
		{
			SpawnPowerUp(thisBrick.transform.position);
			ResetPowerUpCounter();
		}

		if (_listOfBricks.Count == 0)
		{
			if (_levelIndex < gridSizes.Count)
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
	
	private void SpawnPowerUp(Vector2 position)
	{
		PowerUp copy = Instantiate(powerUp, position, Quaternion.identity);

		copy.brickSpawner = this; // Assign this brickspawner to the new instance of the powerup
	}

	public void BrickGotHit(Brick hitBrick)
	{
		if (ball.superBallActive)
		{
			_superBallHitsRemaining--;

			if (_superBallHitsRemaining <= 0)
			{
				DeactivateSuperBall();
			}
		}
	}
}
