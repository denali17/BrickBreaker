using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
	public static Manager instance = null;

	public Text m_scoreText;
	private int m_score = 0;

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

	public void IncreaseScore(int points)
	{
		m_score += points;
		m_scoreText.text = m_score.ToString();
	}
}
