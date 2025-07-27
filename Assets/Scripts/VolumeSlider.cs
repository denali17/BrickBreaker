using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
	[SerializeField] private Slider volumeSlider;

	private void Start()
	{
		volumeSlider.onValueChanged.AddListener(SetVolume);

		// If there is no save data, set default volume to 50%
		float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = volume;
		SetVolume(volume);
	}

    public void SetVolume(float sliderValue)
    {
		// Adjust volume through main menu slider
        AudioListener.volume = sliderValue;
		// Save to player prefs
		PlayerPrefs.SetFloat("Volume", sliderValue);
	}
}
