using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	private static bool _musicPlaying = false;

    private AudioSource _audioSource;

	private void Awake()
	{
		if (_musicPlaying)
		{
			Destroy(gameObject);
			return;
		}

		_musicPlaying = true;
		DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
    }

	private void Start()
    {
        _audioSource.Play();
    }
}
