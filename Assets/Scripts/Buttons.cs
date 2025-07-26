using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
	[SerializeField] private Sprite[] brickSprites;
	[SerializeField] private Image image;

	private int _pressCount = 0;
	private AudioSource _audioSource;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void StartGame()
	{
		_audioSource.pitch = Random.Range(0.8f, 1.2f); // Random pitch
		_audioSource.Play();

		_pressCount++;
	
		if (_pressCount >= 3)
		{
			image.gameObject.SetActive(false);
			// Give a little extra time for the sound to play before loading the new scene
			Invoke("LoadGame", 0.1f);
		}
		else
		{
			image.sprite = brickSprites[_pressCount];
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

	public void QuitToTitle()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("TitleScreen");
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
	}
}
