using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
	private void Start()
	{
		
	}
	public void LoadGame()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("Game");
	}

	public void QuitGame()
	{
		Application.Quit();
		Debug.LogWarning("Quit Game");
	}
}
