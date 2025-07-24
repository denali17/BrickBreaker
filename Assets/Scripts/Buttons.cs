using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
	public Sprite[] m_brickSprites;
	public Image m_image;
	private int m_pressCount = 0;

	public void OnButtonPressed()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.pitch = Random.Range(0.8f, 1.2f); // Random pitch
		audioSource.Play();

		m_pressCount++;
	
		if (m_pressCount >= 3)
		{
			m_image.gameObject.SetActive(false);
			// Give a little extra time for the sound to play before loading the new scene
			Invoke("LoadGame", 0.1f);
		}
		else
		{
			m_image.sprite = m_brickSprites[m_pressCount];
		}
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

	public void FullScreen()
	{
		if (Screen.fullScreen == true)
		{
			// When switching to windowed mode set resolution to half of the screen resolution  
			Screen.SetResolution(
				Screen.currentResolution.width / 2,
				Screen.currentResolution.height / 2,
				false);
		}
		else
		{
			// When switching to fullscreen mode set resolution to screen resolution
			Screen.SetResolution(
				Screen.currentResolution.width,
				Screen.currentResolution.height,
				true);
		}

		Debug.LogWarning("Toggle Fullscreen");
	}
}
