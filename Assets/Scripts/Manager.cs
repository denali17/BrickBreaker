using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
	public static Manager instance = null;
	public GameObject gameOverPanel = null;
	public GameObject congratsPanel = null;
	public GameObject pauseMenu = null;

	private bool m_isPaused = false;

	public TextMeshProUGUI scoreText;
	private int score = 0;

	public TextMeshProUGUI highScoreText;
	private int highScore = 0;
	
	public int m_numberOfLives = 3;
	public LivesSpawner livesSpawner;

	void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Duplicate manager!");
			Destroy(gameObject);
			return;
		}

		instance = this;
	}

	private void Start()
	{
		livesSpawner.SpawnLives(m_numberOfLives);

		highScore = PlayerPrefs.GetInt("High Score", 0);
		highScoreText.text = highScore.ToString();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (m_isPaused)
			{
				ResumeGame();
			}
			else
			{
				PauseGame();
			}
		}
	}

	public void IncreaseScore(int points)
	{
		score += points;
		scoreText.text = score.ToString();

		if (score > highScore)
		{
			highScore = score;
			highScoreText.text = highScore.ToString();
			PlayerPrefs.SetInt("High Score", highScore);
		}
	}

	public void ClearSave()
	{
		// Reset high score to current score
		if (highScore > score)
		{
			highScore = score;
			highScoreText.text = highScore.ToString();
			PlayerPrefs.SetInt("High Score", highScore);
		}
	}

	public void LoseLives()
	{
		if (m_numberOfLives > 0)
		{
			m_numberOfLives--;
			livesSpawner.UpdateHearts(m_numberOfLives);
		}

		if (m_numberOfLives <= 0)
		{
			Manager.instance.GameOver();
		}
	}

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		pauseMenu.SetActive(true);
		m_isPaused = true;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1.0f;
		pauseMenu.SetActive(false);
		m_isPaused = false;
	}

	public void GameOver()
	{
		// Pause the game and activate game over screen
		Time.timeScale = 0.0f;
		gameOverPanel.SetActive(true);
	}

	public void DestroyedAllBricks()
	{
		// Pause the game and activate win screen
		Time.timeScale = 0.0f;
		congratsPanel.SetActive(true);
	}
}
