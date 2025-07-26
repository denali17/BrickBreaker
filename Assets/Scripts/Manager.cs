using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
	public static Manager instance = null;
	public GameObject gameOverPanel = null;
	[SerializeField] private GameObject congratsPanel = null;
	[SerializeField] private GameObject pauseMenu = null;

	private bool _isPaused = false;

	[SerializeField] private TextMeshProUGUI scoreText;
	private int _score = 0;

	[SerializeField] private TextMeshProUGUI highScoreText;
	private int _highScore = 0;

	[SerializeField] private int numberOfLives = 3;
	[SerializeField] private LivesSpawner livesSpawner;

	[SerializeField] private AudioSource deathSound;
	[SerializeField] private AudioSource powerUpSound;

	private void Awake()
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
		livesSpawner.SpawnLives(numberOfLives);

		_highScore = PlayerPrefs.GetInt("High Score", 0);
		highScoreText.text = _highScore.ToString();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (_isPaused)
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
		_score += points;
		scoreText.text = _score.ToString();

		if (_score > _highScore)
		{
			_highScore = _score;
			highScoreText.text = _highScore.ToString();
			PlayerPrefs.SetInt("High Score", _highScore);
		}
	}

	public void ClearSave()
	{
		// Reset high score to current score
		if (_highScore > _score)
		{
			_highScore = _score;
			highScoreText.text = _highScore.ToString();
			PlayerPrefs.SetInt("High Score", _highScore);
		}
	}

	public void LoseLives()
	{
		if (numberOfLives > 0)
		{
			numberOfLives--;
			livesSpawner.UpdateHearts(numberOfLives);

			deathSound.Play();
		}

		if (numberOfLives <= 0)
		{
			GameOver();
		}
	}

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		pauseMenu.SetActive(true);
		_isPaused = true;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1.0f;
		pauseMenu.SetActive(false);
		_isPaused = false;
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

	public void PlayPowerUpAudio()
	{
		powerUpSound.pitch = Random.Range(0.8f, 1.2f);
		powerUpSound.Play();
	}
}
