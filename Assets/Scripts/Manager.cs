using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
	public static Manager instance = null;
	public GameObject gameOverPanel = null;

	public Text scoreText;
	private int score = 0;
	
	public Text livesText;
	private int lives = 3;

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
		livesText.text = lives.ToString();
	}

	public void IncreaseScore(int points)
	{
		score += points;
		scoreText.text = score.ToString();
	}

	public void LoseLives()
	{
		if (lives > 0)
		{
			lives--;
			livesText.text = lives.ToString();
		}

		if (lives <= 0)
		{
			GameOver();
		}
	}

	public void GameOver()
	{
		// Pause the game and activate game over screen
		Time.timeScale = 0.0f;
		gameOverPanel.SetActive(true);
	}
}
